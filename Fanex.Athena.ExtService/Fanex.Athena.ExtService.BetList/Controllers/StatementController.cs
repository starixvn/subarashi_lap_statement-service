using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.ExtService.BetList.Controllers
{
    [Authorize]
    public class StatementController : Controller
    {

        public IActionResult Summary()
        {
            var siteName = HttpContext.User.Identity.Name;
            ViewBag.SiteName = siteName;
            return View();
        }
        public IActionResult ByGroup()
        {
            var siteName = HttpContext.User.Identity.Name;
            ViewBag.SiteName = siteName;
            return View();
        }
    }
}
