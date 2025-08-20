namespace Fanex.Athena.ExtService.BetList.Controllers
{
    using System.Threading.Tasks;
    using Common.Attributes;
    using Common.Models.DataDomain;
    using Common.Services;
    using Core.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.DataDomain;
    using Models.MetaModel;
    using Models.ViewModel;
    using Services;

    [Authorize]
    [ServiceFilter(typeof(CheckCustomerAttribute))]
    public class StatementController : BaseController
    {
        private readonly ICommonService _commonService;

        public StatementController(ICommonService commonService, IContextService contextService) : base(contextService)
        {
            _commonService = commonService;
        }

        [HttpGet]
        [ResponseCache(Duration = SystemConstants.ThirtySecond, NoStore = false,
            VaryByQueryKeys = new[] { "vt", "cid", "fd", "td", "bc", "ps", "pi", "sb", "lang" })]
        public async Task<IActionResult> Summary([FromServices] ISummaryStatementService service,
            SummaryStatementMetaModel metalModel)
        {
            var viewModel = metalModel.IsValid ? await service.QueryAsync(metalModel).ConfigureAwait(false)
                : new SummaryStatementViewModel();
            AssignCommonViewModelData(metalModel, viewModel);

            viewModel.CustId = metalModel.CustId;
            viewModel.PageIndex = metalModel.PageIndex;
            viewModel.PageSize = metalModel.PageSize;
            viewModel.FromDate = metalModel.FromDate;
            viewModel.ToDate = metalModel.ToDate;
            viewModel.IsDesktopViewOnly = metalModel.IsDesktopViewOnly;
            viewModel.ViewType = metalModel.ViewType;

            ViewBag.ShowBreadCrumb = metalModel.ShowBreadCrumb;
            ViewBag.IsEnableSingleWallet = await _commonService.IsEnableSingleWallet(HttpContext.User.Identity.Name);
            return View(viewModel);
        }

        [ResponseCache(Duration = SystemConstants.ThirtySecond, NoStore = false,
    VaryByQueryKeys = new[] { "vt", "cid", "wd", "bc", "mv", "lang", "ps", "pi", "fd", "td", "sb" })]

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
            viewModel.IsDesktopViewOnly = metalModel.IsDesktopViewOnly;

            ViewBag.PageSize = metalModel.PageSize;
            ViewBag.PageIndex = metalModel.PageIndex;
            ViewBag.ShowBreadCrumb = metalModel.ShowBreadCrumb;
            ViewBag.FromDate = metalModel.FromDate;
            ViewBag.ToDate = metalModel.ToDate;
            ViewBag.IsEnableSingleWallet = await _commonService.IsEnableSingleWallet(HttpContext.User.Identity.Name);
            ViewBag.ViewType = metalModel.ViewType;

            return View(viewModel);
        }
    }
}