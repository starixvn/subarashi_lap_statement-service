using Microsoft.AspNetCore.Builder;
using ONEBook.UM.Extensions.Middleware;

namespace Fanex.Athena.ExtService.Core.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseJWTokenOnPage(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JWTokenHandler>();
        }

        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandler>();
        }

        public static IApplicationBuilder UseUMHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UMHandler>();
        }
    }
}
