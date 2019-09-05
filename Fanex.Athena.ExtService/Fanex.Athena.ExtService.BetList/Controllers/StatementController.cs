using System.Threading.Tasks;

using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Fanex.Athena.ExtService.BetList.Models.ViewModel;
using Fanex.Athena.ExtService.BetList.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.ExtService.BetList.Controllers
{
    [Authorize]
    public class StatementController : BaseController
    {

        [HttpGet]
        public async Task<IActionResult> Summary([FromServices] ISummaryStatementService service,
            SummaryStatementMetaModel metalModel)
        {

            var viewModel = metalModel.IsValid ? await service.QueryAsync(metalModel).ConfigureAwait(false)
                : new SummaryStatementViewModel();
            AssignCommonViewModelData(metalModel, viewModel);
            return View(viewModel);
        }
        public IActionResult ByGroup()
        {
            var siteName = HttpContext.User.Identity.Name;
            ViewBag.SiteName = siteName;
            return View();
        }

    }
}
