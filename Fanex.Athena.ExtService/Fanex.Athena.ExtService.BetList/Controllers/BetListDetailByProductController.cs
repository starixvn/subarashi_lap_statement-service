using System;
using System.Threading.Tasks;
using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Fanex.Athena.ExtService.BetList.Models.ViewModel;
using Fanex.Athena.ExtService.Common.Services;
using Fanex.Athena.ExtService.Core.Constant;
using Fanex.Athena.ExtService.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.ExtService.BetList.Controllers
{
    [Authorize]
    public class BetListDetailByProductController : BaseController
    {
        private readonly ICommonService _commonService;
        private readonly IBetListApiClient _betListApiClient;
        private readonly IBrowserService _browserService;

        public BetListDetailByProductController(
            ICommonService commonService,
            IBetListApiClient betListApiClient,
            IBrowserService browserService)
        {
            _commonService = commonService;
            _betListApiClient = betListApiClient;
            _browserService = browserService;
        }

        [HttpGet]
        public async Task<IActionResult> Detail(BetListDetailByProductMetaModel metaModel)
        {
            _browserService.SetContext(HttpContext);
            var isMobileView = !metaModel.IsDesktopViewOnly && _browserService.IsMobile();
            var licenseeInfo = await _commonService.GetLicenseeInfoAsync(HttpContext.User.Identity.Name, metaModel.CustId);
            bool isShowIp = _commonService.IsShowIp(licenseeInfo.SiteName);

            metaModel.IsMobileView = isMobileView;
            metaModel.IsShowIpvn = IsShowIpvn() && isShowIp;
            metaModel.IsShowIp = isShowIp;

            AssignCommonMetaModelValue(metaModel, licenseeInfo);
            var result =
                 await _betListApiClient.PostAsync<BetListResultViewModel>("/BetList/GetBetList", metaModel, metaModel.Lang).ConfigureAwait(false);
            result.HasTotalRow = metaModel.HasTotal;
            result.HasLicWl = metaModel.HasLicWl;
            result.IsDesktopViewOnly = metaModel.IsDesktopViewOnly;
            AssignCommonViewModelData(metaModel, result);

            ViewBag.Title = "Bet List - Product";
            result.Info = licenseeInfo;

            var viewSuffix = isMobileView ? "Mobile" : "";
            return View($"~/Views/BetListDetailByProduct/Detail{viewSuffix}.cshtml", result);
        }

        private bool IsShowIpvn()
        {
            if (string.IsNullOrEmpty(HttpContext.User.FindFirst(CustomClaimType.IsShowIPVN)?.Value))
            {
                return true;
            }
            return Convert.ToBoolean(HttpContext.User.FindFirst(CustomClaimType.IsShowIPVN).Value);
        }
    }
}
