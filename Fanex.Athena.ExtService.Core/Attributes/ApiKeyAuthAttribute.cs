namespace Fanex.Athena.ExtService.Core.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Options;
    using Models.DTO;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class ApiKeyAuthAttribute : Attribute, IAuthorizationFilter
    {
        private const string ApiKeyHeaderName = "ApiKey";
        private readonly List<ApiKey> _apiKeyList;

        public ApiKeyAuthAttribute(IOptions<List<ApiKey>> apiKeyListAccessor)
        {
            _apiKeyList = apiKeyListAccessor.Value;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
            {
                SetUnauthorizedResponse(context.HttpContext);

                context.Result = new UnauthorizedObjectResult("API key is required.");
                return;
            }

            if (!IsValidApiKey(extractedApiKey, context.HttpContext))
            {
                SetUnauthorizedResponse(context.HttpContext);

                context.Result = new UnauthorizedObjectResult("API key is invalid.");
            }
        }

        private static void SetUnauthorizedResponse(HttpContext httpContext)
        {
            httpContext.Response.Clear();
            httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            httpContext.Response.ContentType = "text/plain";
        }

        private bool IsValidApiKey(string apiKey, HttpContext httpContext)
        {
            if (_apiKeyList is null || _apiKeyList.Count == 0)
            {
                return true;
            }

            var keyConfig = _apiKeyList.FirstOrDefault(x => x.Key.Equals(apiKey));

            if (keyConfig is null)
            {
                return false;
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, keyConfig.SiteName)
            };

            var appIdentity = new ClaimsIdentity(claims);
            httpContext.User = new ClaimsPrincipal(appIdentity);
            return true;
        }
    }
}