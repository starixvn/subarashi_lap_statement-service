namespace Fanex.Athena.ExtService.Core.Services
{
    using Microsoft.AspNetCore.Http;
    using Models;

    public interface IBrowserService
    {
        void SetContext(HttpContext httpContext);

        bool IsTablet();

        bool IsMobileOrTablet();

        bool IsMobile();

        bool IsAndroidNativeBrowser(string userAgent);

        BrowserInfo GetBrowserInfo();

        string GetUserAgent();
    }
}