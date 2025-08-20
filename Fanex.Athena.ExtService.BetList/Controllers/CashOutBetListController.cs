namespace Fanex.Athena.ExtService.BetList.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Common.Models.DataDomain;
    using Common.Services;
    using Core.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.MetaModel;
    using Models.ViewModel;
    using Resources;

    [Authorize]
    public class CashOutBetListController : BaseController
    {
        private readonly IBetListApiClient _betListApiClient;
        private readonly IBrowserService _browserService;
        private readonly ICommonService _commonService;

        public CashOutBetListController(
            ICommonService commonService,
            IBetListApiClient betListApiClient,
            IBrowserService browserService,
            IContextService contextService)
            : base(contextService)
        {
            _commonService = commonService;
            _betListApiClient = betListApiClient;
            _browserService = browserService;
        }

        [HttpGet]
        [ResponseCache(Duration = SystemConstants.ThirtySecond)]
        public async Task<IActionResult> Detail(CashOutBetListDetailMetaModel metaModel)
        {
            _browserService.SetContext(HttpContext);
            var isMobileView = !metaModel.IsDesktopViewOnly && _browserService.IsMobile();
            metaModel.IsMobileView = isMobileView;

            var licenseeInfo = await _commonService.GetLicenseeInfoAsync(HttpContext.User.Identity.Name);

            AssignCommonMetaModelValue(metaModel, licenseeInfo);

            var viewModel = await _betListApiClient.PostAsync<CashOutBetListDetailViewModel>("/api/CashOutBetList/Detail",
                metaModel, metaModel.Lang).ConfigureAwait(false);
            viewModel.Info = licenseeInfo;

            AssignCommonViewModelData(metaModel, viewModel);
            ViewBag.Title = "Cash Out BetList";

            return View($"~/Views/CashOutBetList/Detail{(isMobileView ? "Mobile" : "")}.cshtml", viewModel);
        }

        [HttpGet]
        [ResponseCache(Duration = SystemConstants.ThirtySecond)]
        public async Task<IActionResult> NewDetail(NewCashOutBetListDetailMetaModel metaModel)
        {
            _browserService.SetContext(HttpContext);
            var isMobileView = !metaModel.IsDesktopViewOnly && _browserService.IsMobile();
            metaModel.IsMobileView = isMobileView;

            var licenseeInfo = await _commonService.GetLicenseeInfoAsync(HttpContext.User.Identity.Name);
            var currency = Array.Find(licenseeInfo.Currencies, unknown => unknown.CurrencyName == metaModel.Currency);
            if (currency != null)
            {
                metaModel.CurrencyId = currency.CurrencyId;
            }

            AssignCommonMetaModelValue(metaModel, licenseeInfo);

            var viewModel = await _betListApiClient.PostAsync<NewCashOutBetListDetailViewModel>("/api/CashOutBetList/History",
                metaModel, metaModel.Lang).ConfigureAwait(false);
            viewModel.Info = licenseeInfo;

            AssignCommonViewModelData(metaModel, viewModel);
            ViewBag.Title = ViewResources.CashOutHistory;

            return View($"~/Views/NewCashOutBetList/Detail{(isMobileView ? "Mobile" : "")}.cshtml", viewModel);
        }
    }
}