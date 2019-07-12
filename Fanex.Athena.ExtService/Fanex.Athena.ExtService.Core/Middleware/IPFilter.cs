using Fanex.Athena.ExtService.Core.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.ExtService.Core.Middleware
{
    public class IPFilter
    {
        private readonly RequestDelegate _next;
        private readonly ApplicationOptions _applicationOptions;
        public IPFilter(RequestDelegate next, IOptions<ApplicationOptions> applicationOptionAccessor)
        {
            _next = next;
            _applicationOptions = applicationOptionAccessor.Value;

        }
        public async Task Invoke(HttpContext context)
        {
            var ipAddress = context.Connection.RemoteIpAddress;
            var whiteListIPList = _applicationOptions.WhiteList;

            var isInWhiteListIPList = whiteListIPList.Where(x => IPAddress.Parse(x).Equals(ipAddress)).Any();
            if (!isInWhiteListIPList)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }

            await _next.Invoke(context);
        }
    }
}
