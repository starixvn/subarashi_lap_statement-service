using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Fanex.Athena.ExtService.BetList.Models.DTO;
using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Fanex.Athena.ExtService.BetList.Models.ViewModel;
using Fanex.Athena.ExtService.Common;
using Fanex.Athena.ExtService.Common.Models.DataDomain;
using Fanex.Athena.ExtService.Common.Models.DTO;
using Fanex.Athena.ExtService.Common.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.ExtService.BetList.Controllers
{
    [Authorize]
    public class BetListController : BaseController
    {
        private readonly ICommonService _commonService;
        private readonly IBetListApiClient _betListApiClient;

        public BetListController(ICommonService commonService, IBetListApiClient betListApiClient)
        {
            _commonService = commonService;
            _betListApiClient = betListApiClient;
        }
        [HttpGet]
        [ResponseCache(Duration = SystemConstants.TwoMinInSecond, NoStore = false,
            VaryByQueryKeys = new[] { "refId", "urs", "lang", "bc" })]
        public async Task<IActionResult> RefNo(long refId, string bc, string urs,
            string lang = SystemConstants.DefaultLangCode, bool exct = true)
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
                    x.Equals(bc, StringComparison.OrdinalIgnoreCase)) ? bc : SystemConstants.DefaultBaseCurrency,
                    RefNo = refId,
                    UserName = urs,
                    AgentListXml = SystemHelper.GenerateAgentListXml(licenseeInfo, exct),
                    UserCurrencyList = GetUserCurrencyList(licenseeInfo),
                    HasPlayerCom = licenseeInfo.IsAllowedCustomerComm
                };

                var result = await _betListApiClient.PostAsync<RefNoDataResult>("/BettingTicketCenter/GetByRefno",
                metaModel,
                lang
                ).ConfigureAwait(false);

                viewModel.Data = result.Data ?? new List<TicketText>();
            }
            ViewBag.Title = "Bet List By RefNo";
            return View(viewModel);
        }

        private static Dictionary<int, string> GetUserCurrencyList(LicenseeInfo licenseeInfo)
        {
            return licenseeInfo.Currencies.GroupBy(x => x.CurrencyId).ToDictionary(x => x.Key, x => x.FirstOrDefault().CurrencyName);
        }

        [HttpGet]
        [ResponseCache(Duration = SystemConstants.TwoMinInSecond, NoStore = false,
            VaryByQueryKeys = new[] { "cid", "lang", "bc", "wd", "pc" })]

        public async Task<IActionResult> ByGroup(BetListByGroupMetaModel metaModel)
        {
            var siteName = HttpContext.User.Identity.Name;
            var licenseeInfo = await _commonService.GetLicenseeInfoAsync(siteName, 0);
            var viewModel = new BetListByProductViewModel
            {
                Info = licenseeInfo,
            };

            if (metaModel.IsValid)
            {
                var param = new
                {
                    metaModel.BaseCurrency,
                    metaModel.PageIndex,
                    metaModel.PageSize,
                    metaModel.CustId,
                    metaModel.Date,
                    metaModel.ProductCode,
                    UserCurrencyList = GetUserCurrencyList(licenseeInfo),
                    HasPlayerCom = licenseeInfo.IsAllowedCustomerComm,
                    TransType = metaModel.ModeView,
                    AgentList = SystemHelper.GenerateAgentListXml(licenseeInfo),
                    metaModel.Sort

                };
                var result = await _betListApiClient.PostAsync<RefNoDataResult>("/BetListByProduct/Index",
                param,
                metaModel.Lang
            ).ConfigureAwait(false);

                viewModel.Data = result.Data ?? new List<TicketText>();
            }

            ViewBag.Title = "Bet List By Group";
            AssignCommonViewModelData(metaModel, viewModel);
            viewModel.ProductCode = metaModel.ProductCode;
            return View(viewModel);
        }
    }
}
