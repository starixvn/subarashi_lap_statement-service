using System.Threading.Tasks;
using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Fanex.Athena.ExtService.BetList.Models.ViewModel;
using Fanex.Athena.ExtService.Common.Models.DataDomain;
using Fanex.Athena.ExtService.Common.Services;
using Fanex.Athena.ExtService.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.ExtService.BetList.Controllers
{
    [Authorize]
    public class CashOutBetListController : BaseController
    {
        private readonly ICommonService _commonService;
        private readonly IBetListApiClient _betListApiClient;
        private readonly IBrowserService _browserService;

        public CashOutBetListController(
            ICommonService commonService,
            IBetListApiClient betListApiClient,
            IBrowserService browserService)
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

            var viewModel = await _betListApiClient.PostAsync<CashOutBetListDetailViewModel>("/CashOutBetList/Detail",
                metaModel, metaModel.Lang).ConfigureAwait(false);
            viewModel.Info = licenseeInfo;

            AssignCommonViewModelData(metaModel, viewModel);
            ViewBag.Title = "Cash Out BetList";

            return View($"~/Views/CashOutBetList/Detail{(isMobileView ? "Mobile" : "")}.cshtml", viewModel);
        }
    }
}
