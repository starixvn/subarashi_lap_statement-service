using System.Threading.Tasks;
using Fanex.Athena.ExtService.BetList.Models.DataDomain;
using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Fanex.Athena.ExtService.BetList.Models.ViewModel;
using Fanex.Athena.ExtService.BetList.Services;
using Fanex.Athena.ExtService.Resources;
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
            viewModel.CustId = metalModel.CustId;
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ByGroup([FromServices] IStatementByGroupService service,
            StatementByGroupMetaModel metalModel)
        {

            var viewModel = metalModel.IsValid ? await service.QueryAsync(metalModel).ConfigureAwait(false)
                : new StatementByGroupViewModel();
            AssignCommonViewModelData(metalModel, viewModel);
            switch (metalModel.ModeView)
            {
                case 1:
                    viewModel.TransDesc = TransDescValue.RemarkValues[TransDescValue.BetStatement].Invoke();
                    break;
                case 2:
                    viewModel.TransDesc = TransDescValue.RemarkValues[TransDescValue.VoidCancell].Invoke();
                    break;
                case 3:
                    viewModel.TransDesc = TransDescValue.RemarkValues[TransDescValue.Outstanding].Invoke();
                    break;
                default: viewModel.TransDesc = ViewResources.Statement; break;
            }
            return View(viewModel);
        }

    }
}
