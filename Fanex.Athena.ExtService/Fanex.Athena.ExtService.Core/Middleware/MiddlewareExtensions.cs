using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fanex.Athena.ExtService.Core.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseIPFilter(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<IPFilter>();
        }
    }
}
