namespace Fanex.Athena.ExtService.BetList.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Attributes;
    using Common.Constant;
    using Common.Models.DataDomain;
    using Common.Services;
    using Core.Services;
    using Helper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.DataDomain;
    using Models.DTO;
    using Models.MetaModel;
    using Models.ViewModel;

    [Authorize]
    public class BetListController : BaseController
    {
        private readonly ICommonService _commonService;
        private readonly IBetListApiClient _betListApiClient;
        private readonly IBrowserService _browserService;

        public BetListController(
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

        private bool IsShowIpvn()
        {
            if (string.IsNullOrEmpty(HttpContext.User.FindFirst(CustomClaimType.IsShowIPVN)?.Value))
            {
                return true;
            }
            return Convert.ToBoolean(HttpContext.User.FindFirst(CustomClaimType.IsShowIPVN).Value);
        }

        [HttpGet]
        [ResponseCache(Duration = SystemConstants.ThirtySecond, NoStore = false,
            VaryByQueryKeys = new[] { "refId", "vt", "urs", "lang", "bc" })]
        public async Task<IActionResult> RefNo(RefNoBetListMetaModel metaModel)
        {
            var siteName = _contextService.SiteName;
            var licenseeInfo = await _commonService.GetLicenseeInfoAsync(siteName);
            var viewModel = new RefNoViewModel
            {
                Info = licenseeInfo,
                UserName = metaModel.UserName,
                Data = new List<TicketText>(),
                IsDesktopViewOnly = metaModel.IsDesktopViewOnly
            };

            ViewBag.IsDisplayLicenseeWLColumn = metaModel.ViewSabaCoins == SystemConstants.DefaultViewType;
            if (metaModel.IsValid)
            {
                AssignCommonMetaModelValue(metaModel, licenseeInfo);
                metaModel.ViewSabaCoins = --metaModel.ViewSabaCoins;
                var result = await _betListApiClient.PostAsync<RefNoDataResult>(
                    "/api/BettingTicketCenter/GetByRefno",
                    metaModel,
                    metaModel.Lang).ConfigureAwait(false);

                viewModel.Data = result.Data ?? new List<TicketText>();
            }

            ViewBag.Title = "Bet List By RefNo";
            ViewBag.IsEnableSingleWallet = _contextService.IsEnableSingleWallet;
            AssignCommonViewModelData(metaModel, viewModel);
            return GetView(metaModel.IsDesktopViewOnly, "~/Views/BetList/RefNo", viewModel);
        }

        [HttpGet]
        [ResponseCache(Duration = SystemConstants.ThirtySecond, NoStore = false,
            VaryByQueryKeys = new[] { "txId", "urs", "cid", "lang", "bc" })]
        public async Task<IActionResult> TxID(TxIDBetListMetaModel metaModel)
        {
            var siteName = _contextService.SiteName;
            var licenseeInfo = await _commonService.GetLicenseeInfoAsync(siteName);
            var viewModel = new TxIDViewModel
            {
                Info = licenseeInfo,
                UserName = metaModel.UserName,
                Data = new List<TicketText>()
            };

            if (metaModel.IsValid)
            {
                AssignCommonMetaModelValue(metaModel, licenseeInfo);

                var result = await _betListApiClient.PostAsync<TxIDDataResult>(
                    "/api/BettingTicketCenter/GetByTxID",
                    metaModel,
                    metaModel.Lang).ConfigureAwait(false);

                viewModel.Data = result.Data ?? new List<TicketText>();
            }

            ViewBag.Title = "Bet List By TxID";

            AssignCommonViewModelData(metaModel, viewModel);

            return GetView(metaModel.IsDesktopViewOnly, "~/Views/BetList/TxID", viewModel);
        }

        [HttpGet]
        [ServiceFilter(typeof(CheckCustomerAttribute))]
        [ResponseCache(Duration = SystemConstants.ThirtySecond, NoStore = false,
            VaryByQueryKeys = new[] { "vt", "cid", "lang", "bc", "wd", "pc", "ps", "pi", "mv", "st", "so", "fd", "td", "sb", "spi", "sps" })]
        public async Task<IActionResult> ByGroup(BetListByGroupMetaModel metaModel)
        {
            var originModeView = metaModel.ModeView;
            metaModel.ModeView = metaModel.ModeView == TransDescValue.ModeViewPromotion ? 0 : metaModel.ModeView;

            var siteName = HttpContext.User.Identity.Name;
            var licenseeInfo = await _commonService.GetLicenseeInfoAsync(siteName, metaModel.CustId);
            var viewModel = new BetListByProductViewModel
            {
                Info = licenseeInfo,
                Data = new List<TicketText>(),
                PageSize = metaModel.PageSize,
                PageIndex = metaModel.PageIndex,
                ProductCode = metaModel.ProductCode,
                TransType = originModeView,
                SortOrder = metaModel.SortOrder,
                SortType = metaModel.SortType,
                WinlostDate = metaModel.WinlostDate,
                CustId = metaModel.CustId,
                IsDesktopViewOnly = metaModel.IsDesktopViewOnly,
                ViewType = metaModel.ViewType
            };

            bool isShowIp = _commonService.IsShowIp(licenseeInfo.SiteName);
            int specialPlatform = _commonService.GetSpecialPlatform(licenseeInfo.SiteName);

            if (metaModel.IsValid)
            {
                AssignCommonMetaModelValue(metaModel, licenseeInfo);
                metaModel.IsShowIpvn = IsShowIpvn() && isShowIp;
                metaModel.IsShowIp = isShowIp;
                metaModel.ProductCode = GetSubProductCode(metaModel.ProductCode);
                metaModel.SpecialPlatform = specialPlatform;

                var result = await _betListApiClient.PostAsync<RefNoDataResult>(
                    "/api/BetListByProduct/Index",
                    metaModel,
                    metaModel.Lang).ConfigureAwait(false);

                viewModel.Data = result.Data ?? new List<TicketText>();
                viewModel.TotalRows = result.TotalRows;
            }

            ViewBag.Title = "Bet List By Group";
            AssignCommonViewModelData(metaModel, viewModel);
            var productResources = await _commonService.GetAllProductResource(metaModel.Lang);

            viewModel.TransDesc = StatementHelper.GetBetingStatementRemark(
                TransDescValue.GetTransactionDescription(metaModel.TransType),
                        metaModel.ProductCode,
                        productResources);
            ViewBag.FromDate = metaModel.FromDate;
            ViewBag.ToDate = metaModel.ToDate;
            ViewBag.StatementPageIndex = metaModel.StatementPageIndex;
            ViewBag.StatementPageSize = metaModel.StatementPageSize;
            ViewBag.ShowBreadCrumb = metaModel.ShowBreadCrumb;
            ViewBag.IsEnableSingleWallet = await _commonService.IsEnableSingleWallet(HttpContext.User.Identity.Name);

            return GetView(metaModel.IsDesktopViewOnly, "~/Views/BetList/ByGroup", viewModel);
        }

        [HttpGet]
        [ServiceFilter(typeof(CheckCustomerAttribute))]
        [ResponseCache(Duration = SystemConstants.ThirtySecond, NoStore = false,
            VaryByQueryKeys = new[] { "cid", "lang", "ps", "pi" })]
        public async Task<IActionResult> Running(RunningBetListMetaModel metaModel)
        {
            var siteName = HttpContext.User.Identity.Name;
            var licenseeInfo = await _commonService.GetLicenseeInfoAsync(siteName, metaModel.CustId);
            var viewModel = new RunningBetListByViewModel
            {
                Info = licenseeInfo,
                Data = new List<TicketText>(),
                IsDesktopViewOnly = metaModel.IsDesktopViewOnly
            };

            bool isShowIp = _commonService.IsShowIp(licenseeInfo.SiteName);
            int specialPlatform = _commonService.GetSpecialPlatform(licenseeInfo.SiteName);

            if (metaModel.IsValid)
            {
                AssignCommonMetaModelValue(metaModel, licenseeInfo);
                metaModel.IsShowIpvn = IsShowIpvn() && isShowIp;
                metaModel.IsShowIp = isShowIp;
                metaModel.SpecialPlatform = specialPlatform;

                var result = await _betListApiClient.PostAsync<RefNoDataResult>(
                    "/api/PlayerAdminBetList/GetBetList",
                    metaModel,
                    metaModel.Lang).ConfigureAwait(false);

                viewModel.Data = result.Data ?? new List<TicketText>();
            }

            ViewBag.Title = "Running Bet List";
            ViewBag.IsEnableSingleWallet = await _commonService.IsEnableSingleWallet(HttpContext.User.Identity.Name);

            AssignCommonViewModelData(metaModel, viewModel);

            return GetView(metaModel.IsDesktopViewOnly, "~/Views/BetList/Running", viewModel);
        }

        private ViewResult GetView(bool isDesktopViewOnly, string viewPath, object viewModel)
        {
            _browserService.SetContext(HttpContext);
            var isMobileView = !isDesktopViewOnly && _browserService.IsMobile();
            return View($"{viewPath}{(isMobileView ? "Mobile" : "")}.cshtml", viewModel);
        }

        private static string GetSubProductCode(string ProductCode)
        {
            var subProductList = new List<string> { Product.SLT, Product.BTC };

            return ProductCode.Contains(Product.SB, StringComparison.OrdinalIgnoreCase) ? ProductCode + "," + string.Join(',', subProductList) : ProductCode;
        }
    }
}