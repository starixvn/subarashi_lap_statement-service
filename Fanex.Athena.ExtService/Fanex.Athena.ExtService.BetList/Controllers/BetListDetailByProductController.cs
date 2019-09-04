using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Fanex.Athena.ExtService.BetList.Models.ViewModel;
using Fanex.Athena.ExtService.Common;
using Fanex.Athena.ExtService.Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.ExtService.BetList.Controllers
{
    [Authorize]

    public class BetListDetailByProductController : BaseController
    {
        private readonly ICommonService _commonService;
        private readonly IBetListApiClient _betListApiClient;
        public BetListDetailByProductController(ICommonService commonService, IBetListApiClient betListApiClient)
        {
            _commonService = commonService;
            _betListApiClient = betListApiClient;
        }

        [HttpPost]
        public async Task<IActionResult> Detail(BetListDetailByProductMetaModel metaModel)
        {
            var licenseeInfo = await _commonService.GetLicenseeInfoAsync(HttpContext.User.Identity.Name, metaModel.CustId);

            metaModel.CurrencyList = licenseeInfo.Currencies.GroupBy(x => x.CurrencyId)
.ToDictionary(x => x.Key, x => x.FirstOrDefault().CurrencyName);
            metaModel.HasPlayerCom = licenseeInfo.IsAllowedCustomerComm;
            metaModel.AgentListXml = SystemHelper.GenerateAgentListXml(licenseeInfo);

            var result =
                 await _betListApiClient.PostAsync<BetListResultViewModel>("/BetList/GetBetList", metaModel, metaModel.Lang).ConfigureAwait(false);
            result.HasTotalRow = metaModel.HasTotal;
            AssignCommonViewModelData(metaModel, result);
            
            return View(result);

        }
    }
}
