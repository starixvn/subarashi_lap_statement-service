using Microsoft.AspNetCore.Builder;

namespace Fanex.Athena.ExtService.Core.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseJWTokenOnPage(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JWToken>();
        }
    }
}
