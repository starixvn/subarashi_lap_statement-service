using System.Threading.Tasks;
using Fanex.Athena.ExtService.BetList.Models.DataDomain;
using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Fanex.Athena.ExtService.BetList.Models.ViewModel;
using Fanex.Athena.ExtService.BetList.Services;
using Fanex.Athena.ExtService.Common.Attributes;
using Fanex.Athena.ExtService.Common.Models.DataDomain;
using Fanex.Athena.ExtService.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.ExtService.BetList.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(CheckCustomerAttribute))]
    public class StatementController : BaseController
    {

        [HttpGet]
        [ResponseCache(Duration = SystemConstants.ThirtySecond, NoStore = false,
            VaryByQueryKeys = new[] { "cid", "fd", "td", "bc", "ps", "pi", "bc", "lang" })]
        public async Task<IActionResult> Summary([FromServices] ISummaryStatementService service,
            SummaryStatementMetaModel metalModel)
        {

            var viewModel = metalModel.IsValid ? await service.QueryAsync(metalModel).ConfigureAwait(false)
                : new SummaryStatementViewModel();
            AssignCommonViewModelData(metalModel, viewModel);
            viewModel.CustId = metalModel.CustId;
            return View(viewModel);
        }

        [ResponseCache(Duration = SystemConstants.ThirtySecond, NoStore = false,
    VaryByQueryKeys = new[] { "cid", "wd", "bc", "mv", "lang" })]

        [HttpGet]
        public async Task<IActionResult> ByGroup([FromServices] IStatementByGroupService service,
            StatementByGroupMetaModel metalModel)
        {

            var viewModel = metalModel.IsValid ? await service.QueryAsync(metalModel).ConfigureAwait(false)
                : new StatementByGroupViewModel();
            AssignCommonViewModelData(metalModel, viewModel);

            viewModel.TransDesc = TransDescValue.GetTransactionDescription(metalModel.ModeView);
            viewModel.CustId = metalModel.CustId;
            viewModel.ModeView = metalModel.ModeView;
            return View(viewModel);
        }

    }
}
