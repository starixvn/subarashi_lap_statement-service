using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.ExtService.Core.Middleware
{
    public class JWToken
    {
        private readonly RequestDelegate _next;
        public JWToken(RequestDelegate next)
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
