using System.Threading.Tasks;
using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Fanex.Athena.ExtService.BetList.Models.ViewModel;
using Fanex.Athena.ExtService.Common;
using Fanex.Athena.ExtService.Common.Models.DataDomain;
using Fanex.Athena.ExtService.Common.Services;
using Fanex.Athena.ExtService.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.ExtService.BetList.Controllers
{
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