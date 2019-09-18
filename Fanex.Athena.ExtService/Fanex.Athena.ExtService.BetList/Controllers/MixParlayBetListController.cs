using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Fanex.Athena.ExtService.BetList.Models.DTO;
using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Fanex.Athena.ExtService.BetList.Models.ViewModel;
using Fanex.Athena.ExtService.Common.Attributes;
using Fanex.Athena.ExtService.Common.Models.DataDomain;
using Fanex.Athena.ExtService.Common.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.ExtService.BetList.Controllers
{
    [Authorize]
    public class MixParlayBetListController : BaseController
    {
        private readonly ICommonService _commonService;
        private readonly IBetListApiClient _betListApiClient;
        public MixParlayBetListController(ICommonService commonService, IBetListApiClient betListApiClient)
        {
            _commonService = commonService;
            _betListApiClient = betListApiClient;
        }

        [ResponseCache(Duration = SystemConstants.ThirtySecond)]
        [HttpGet]
        public async Task<IActionResult> Detail(MixParlayDetailMetaModel metaModel)
        {
            await AssignUserCurrency(metaModel);

            var result = await _betListApiClient.PostAsync<IList<TicketText>>("/MixParlayBetList/Detail",
                metaModel,
                metaModel.Lang).ConfigureAwait(false);
            return Json(result);
        }

        [ResponseCache(Duration = SystemConstants.ThirtySecond)]

        [HttpGet]
        public async Task<IActionResult> NewParlayDetail(
            NewParlayDetailMetaModel metaModel)
        {
            await AssignUserCurrency(metaModel);
            var result = await _betListApiClient.PostAsync<NewParlayDetailViewModel>("/MixParlayBetList/NewParlayDetail",
                metaModel, metaModel.Lang).ConfigureAwait(false);
            return Json(result);
        }

        private async Task AssignUserCurrency(MixParlayDetailMetaModel metaModel)
        {
            var licenseeInfo = await _commonService.GetLicenseeInfoAsync(HttpContext.User.Identity.Name, metaModel.CustId);

            metaModel.UserCurrencyList = licenseeInfo.Currencies.GroupBy(x => x.CurrencyId)
           .ToDictionary(x => x.Key, x => x.FirstOrDefault().CurrencyName);

        }
    }
}
