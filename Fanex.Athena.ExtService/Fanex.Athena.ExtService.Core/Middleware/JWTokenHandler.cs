using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Fanex.Athena.ExtService.Core.Middleware
{
    public class JWTokenHandler
    {
        private readonly RequestDelegate _next;
        public JWTokenHandler(RequestDelegate next)
        {
            _next = next;

        }
        public async Task Invoke(HttpContext context)
        {
            context.Request.Query.TryGetValue("access_token", out StringValues queryData);
            var token = queryData.FirstOrDefault();

            if (!string.IsNullOrEmpty(token) && !context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Request.Headers.Add("Authorization", "Bearer " + token);
            }

            await _next.Invoke(context);
        }
    }
}
