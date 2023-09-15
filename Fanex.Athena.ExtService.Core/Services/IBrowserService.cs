using Fanex.Athena.ExtService.Core.Models;
using Microsoft.AspNetCore.Http;

namespace Fanex.Athena.ExtService.Core.Services
{
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
