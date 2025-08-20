namespace Fanex.Athena.ExtService.BetList.Controllers
{
    using System.Threading.Tasks;
    using Common;
    using Common.Models.DataDomain;
    using Common.Services;
    using Core.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.MetaModel;
    using Models.ViewModel;

    [Authorize]
    public class SystemParlayBetListController : BaseController
    {
        private readonly ICommonService _commonService;
        private readonly IBetListApiClient _betListApiClient;
        private readonly IBrowserService _browserService;

        public SystemParlayBetListController(
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
        public async Task<IActionResult> Detail(SystemParlayDetailMetaModel metaModel)
        {
            _browserService.SetContext(HttpContext);
            metaModel.IsMobileView = !metaModel.IsDesktopViewOnly && _browserService.IsMobile();

            var licenseeInfo = await _commonService.GetLicenseeInfoAsync(HttpContext.User.Identity.Name);
            metaModel.AgentListXml = SystemHelper.GenerateAgentListXml(licenseeInfo);

            var data = await _betListApiClient.PostAsyncString("/SystemParlayBetList/Detail", metaModel,
                metaModel.Lang).ConfigureAwait(false);

            var viewModel = new SystemParlayDetailViewModel
            {
                DetailText = data
            };

            return View(viewModel);
        }
    }
}