using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanex.Athena.ExtService.BetList.Models.DTO;
using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Fanex.Athena.ExtService.BetList.Models.ViewModel;
using Fanex.Athena.ExtService.Common;
using Fanex.Athena.ExtService.Common.Model.DataDomain;
using Fanex.Athena.ExtService.Common.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.ExtService.BetList.Controllers
{
    [Authorize]
    public class BetListController : BaseController
    {
        private const string DefaultBaseCurrency = string.Empty;
        private const string DefaultLang = "en-US";
        private readonly ICommonService _commonService;
        private readonly IBetListApiClient _betListApiClient;

        public BetListController(ICommonService commonService, IBetListApiClient betListApiClient)
        {
            _commonService = commonService;
            _betListApiClient = betListApiClient;
        }
        public async Task<IActionResult> RefNo(long refId, string bc, string urs, string lang = DefaultLang, bool exct = true)
        {
            var siteName = HttpContext.User.Identity.Name;
            var licenseeInfo = await _commonService.GetLicenseeInfoAsync(siteName, 0);
            var balanceUp = await _commonService.GetBalanceUpAsync();
            var viewModel = new RefNoViewModel
            {
                Info = licenseeInfo,
                BalanceUp = balanceUp,
                BaseCurrency = bc,
                UserName = urs,
                AccessToken = HttpContext.Request.Query["access_token"],
                Lang = lang
            };

            if (refId > 0)
            {
                var metaModel = new RefNoBetListMetaModel
                {
                    BaseCurrency = SystemConstants.BaseCurrencyList.Values.Any(x =>
                    x.Equals(bc, StringComparison.OrdinalIgnoreCase)) ? bc : DefaultBaseCurrency,
                    RefNo = refId,
                    UserName = urs,
                    AgentListXml = SystemHelper.GenerateAgentListXml(licenseeInfo, exct),
                    UserCurrencyList = licenseeInfo.Currencies.GroupBy(x => x.CurrencyId).ToDictionary(x => x.Key, x => x.FirstOrDefault().CurrencyName),
                    HasPlayerCom = licenseeInfo.IsAllowedCustomerComm
                };

                var result = await _betListApiClient.PostAsync<RefNoDataResult>("/BettingTicketCenter/GetByRefno",
                metaModel,
                lang
                ).ConfigureAwait(false);

                viewModel.Data = result.Data ?? new List<TicketText>();
            }
            return View(viewModel);
        }

        public IActionResult ByGroup()
        {
            var siteName = HttpContext.User.Identity.Name;
            ViewBag.SiteName = siteName;
            return View();
        }
    }
}
