using Microsoft.AspNetCore.Builder;

namespace Fanex.Athena.ExtService.Core.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseJWTokenOnPage(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JWToken>();
        }

        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandling>();
        }
    }
}
