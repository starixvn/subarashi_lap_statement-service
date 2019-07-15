using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.ExtService.BetList.Controllers
{
    [Authorize]
    public class BetListController : Controller
    {

        public IActionResult RefNo()
        {
            var siteName = HttpContext.User.Identity.Name;
            ViewBag.SiteName = siteName;
            return View();
        }
    }
}
