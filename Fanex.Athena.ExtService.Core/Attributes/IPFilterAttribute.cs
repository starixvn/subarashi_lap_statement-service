namespace Fanex.Athena.ExtService.Core.Attributes
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Common.Models.DTO;
    using Common.Services;
    using Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Options;
    using Models;
    using Models.DTO;
    using NetTools;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class IPFilterAttribute : ActionFilterAttribute
    {
        private readonly ApplicationOptions _applicationOptions;
        private readonly ICommonService _commonService;
        private readonly AppSettings _settings;

        public IPFilterAttribute(IOptions<ApplicationOptions> applicationOptionAccessor, ICommonService commonService, IOptions<AppSettings> appSettings)
        {
            _applicationOptions = applicationOptionAccessor.Value;
            _commonService = commonService;
            _settings = appSettings.Value;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.Request.FromInternal())
            {
                await next();
                return;
            }

            var siteName = string.Empty;

            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                siteName = context.HttpContext.User.Identity.Name;
            }
            else
            {
                var tokenRequest = context.ActionArguments.ContainsKey("request")
                    ? context.ActionArguments["request"] as TokenRequest
                    : new TokenRequest();

                siteName = GetSiteNameFromTokenRequest(tokenRequest);
            }

            var ipAddress = context.HttpContext.Connection.RemoteIpAddress;
            var whiteListIPListFromDB = await _commonService.GetWhitelistIPFromDB(siteName);
            var whiteListIPListFromAppSettings = _applicationOptions.WhiteList;
            var whiteListIPList = whiteListIPListFromDB.Concat(whiteListIPListFromAppSettings);

            var isInWhiteListIPList = !string.IsNullOrEmpty(_settings?.Environment) && _settings.Environment.Contains("LOCAL")
                || !whiteListIPList.Any()
                || whiteListIPList.Any(_ => IsInRangeIP(_, ipAddress));
            if (!isInWhiteListIPList)
            {
                var result = new ContentResult
                {
                    Content = $"The IP Address: {ipAddress} was blocked"
                };

                context.Result = result;
                return;
            }
            await next();
        }

        private static readonly Func<string, IPAddress, bool> IsInRangeIP = (ipRangeString, ipAddress) =>
        {
            if (!IPAddressRange.TryParse(ipRangeString, out IPAddressRange ipRange))
            {
                throw new Exception($"The WhiteList IP: {ipRangeString} was invalid - client IP: ${ipAddress}");
            }

            return ipRange.Contains(ipAddress);
        };

        private static string GetSiteNameFromTokenRequest(TokenRequest request)
        {
            if (!string.IsNullOrEmpty(request.Username) && request.Username.Contains("@"))
            {
                var temp = request.Username.Split('@');

                return temp[0];
            }

            return request.Username;
        }
    }
}