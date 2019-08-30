using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fanex.Athena.ExtService.BetList.Models.BetList;
using Fanex.Athena.ExtService.Shared;
using Fanex.Athena.ExtService.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.ExtService.BetList.Controllers
{
    [Authorize]
    public class SystemParlayBetListController : Controller
    {
        private readonly ICommonService _commonService;
        private readonly IBetListApiClient _betListApiClient;
        public SystemParlayBetListController(ICommonService commonService, IBetListApiClient betListApiClient)
        {
            _commonService = commonService;
            _betListApiClient = betListApiClient;
        }
        [HttpGet]
        public async Task<IActionResult> Detail(SystemParlayDetailMetaModel metaModel)
        {
            var balanceUp = _commonService.GetBalanceUp();
            var licenseeInfo = _commonService.GetLicenseeInfo(HttpContext.User.Identity.Name);
            metaModel.BalanceUpDay = DateTime.SpecifyKind(balanceUp.BalanceUpDay.Date, DateTimeKind.Utc);
            metaModel.MoveBetsDay = DateTime.SpecifyKind(balanceUp.MoveBetsDay.Date, DateTimeKind.Utc);
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
