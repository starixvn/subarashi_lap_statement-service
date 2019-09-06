using System.Collections.Generic;
using System.Linq;
using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Fanex.Athena.ExtService.Common;
using Fanex.Athena.ExtService.Common.Models.DTO;
using Fanex.Athena.ExtService.Common.Models.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.ExtService.BetList.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected  void AssignCommonViewModelData(IBaseMetaModel metaModel, BaseViewModel viewModel)
        {
            viewModel.BaseCurrency = metaModel.BaseCurrency;
            viewModel.AccessToken = HttpContext.Request.Query["access_token"];
            viewModel.Lang = metaModel.Lang;

        }
        protected static void AssignCommonMetaModelValue(IBaseBetListMetaModel metaModel, LicenseeInfo licenseeInfo)
        {
            metaModel.UserCurrencyList = GetUserCurrencyList(licenseeInfo);
            metaModel.HasPlayerCom = licenseeInfo.IsAllowedCustomerComm;
            metaModel.AgentListXml = SystemHelper.GenerateAgentListXml(licenseeInfo);
        }
        private static IDictionary<int, string> GetUserCurrencyList(LicenseeInfo licenseeInfo)
        {
            return licenseeInfo.Currencies.GroupBy(x => x.CurrencyId).ToDictionary(x => x.Key, x => x.FirstOrDefault().CurrencyName);
        }

    }
}
