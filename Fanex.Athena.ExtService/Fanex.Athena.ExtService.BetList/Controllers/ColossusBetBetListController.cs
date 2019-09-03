using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Fanex.Athena.ExtService.BetList.Models.ViewModel;
using Fanex.Athena.ExtService.Shared;
using Fanex.Athena.ExtService.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.ExtService.BetList.Controllers
{
    [Authorize]
    public class ColossusBetBetListController: Controller
    {
        private readonly ICommonService _commonService;
        private readonly IBetListApiClient _betListApiClient;
        public ColossusBetBetListController(ICommonService commonService, IBetListApiClient betListApiClient)
        {
            _commonService = commonService;
            _betListApiClient = betListApiClient;
        }

        [HttpGet]
        [ResponseCache(NoStore = true, Duration = 0, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> Detail(ColossusBetDetailMetaModel metaModel)
        {
            var licenseeInfo = _commonService.GetLicenseeInfo(HttpContext.User.Identity.Name);
            metaModel.AgentListXml = SystemHelper.GenerateAgentListXml(licenseeInfo);

            var viewModel = await _betListApiClient.PostAsync<ColossusBetDetailViewModel>("/ColossusBetBetList/Detail",
                metaModel,
                metaModel.Lang
                ).ConfigureAwait(false);

            return this.View("~/Views/Desktop/BetList/ColossusBetDetail/Main.cshtml", viewModel);
        }
    }
}
