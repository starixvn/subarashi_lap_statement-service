using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Fanex.Athena.ExtService.BetList.Models.BetList;
using Fanex.Athena.ExtService.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.ExtService.BetList.Controllers
{
    [Authorize]
    public class MixParlayBetListController : Controller
    {
        private readonly ICommonService _commonService;
        private readonly IBetListApiClient _betListApiClient;
        public MixParlayBetListController(ICommonService commonService, IBetListApiClient betListApiClient)
        {
            _commonService = commonService;
            _betListApiClient = betListApiClient;
        }
        [HttpGet]
        public async Task<IActionResult> Detail(MixParlayDetailMetaModel metaModel)
        {
            AssignUserCurrency(metaModel);

            var result = await _betListApiClient.PostAsync<IList<TicketText>>("/MixParlayBetList/Detail", metaModel
                , metaModel.Lang).ConfigureAwait(false);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> NewParlayDetail(
            NewParlayDetailMetaModel metaModel)
        {
            AssignUserCurrency(metaModel);
            var result = await _betListApiClient.PostAsync<NewParlayDetailViewModel>("/MixParlayBetList/NewParlayDetail",
                metaModel, metaModel.Lang).ConfigureAwait(false);
            return Json(result);
        }

        private void AssignUserCurrency(MixParlayDetailMetaModel metaModel)
        {
            var licenseeInfo = _commonService.GetLicenseeInfo(HttpContext.User.Identity.Name);

            metaModel.UserCurrencyList = licenseeInfo.Currencies.GroupBy(x => x.CurrencyId)
           .ToDictionary(x => x.Key, x => x.FirstOrDefault().CurrencyName);

        }
    }
}
