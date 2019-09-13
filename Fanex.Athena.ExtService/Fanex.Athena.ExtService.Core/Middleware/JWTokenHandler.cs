using System.Linq;
using System.Threading.Tasks;
using Fanex.Athena.ExtService.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Fanex.Athena.ExtService.Core.Middleware
{
    public class JWTokenHandler
    {
        private readonly RequestDelegate _next;
        private readonly IEncryptService _encryptService;
        const string HeaderAuthKey = "Authorization";
        const string SuffixTokenValue = "Bearer ";

        public JWTokenHandler(RequestDelegate next, IEncryptService encryptService)
        {
            _next = next;
            _encryptService = encryptService;
        }
        public async Task Invoke(HttpContext context)
        {
            var token = GetToken(context);

            if (!string.IsNullOrEmpty(token))
            {
                var plainToken = _encryptService.Decrypt(token);
                if (context.Request.Headers.ContainsKey(HeaderAuthKey))
                {
                    context.Request.Headers.Remove(HeaderAuthKey);
                }
                context.Request.Headers.Add(HeaderAuthKey, SuffixTokenValue + plainToken);
            }

            await _next.Invoke(context);
        }

        private static string GetToken(HttpContext context)
        {
            context.Request.Query.TryGetValue("access_token", out StringValues queryData);
            if (queryData.Any())
            {
                return queryData.FirstOrDefault();
            }

            if (context.Request.Headers.ContainsKey(HeaderAuthKey))
            {
                return context.Request.Headers[HeaderAuthKey].FirstOrDefault()?.Replace(SuffixTokenValue, string.Empty);
            }

            return string.Empty;
        }
    }
}
