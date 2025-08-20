namespace Fanex.Athena.ExtService.BetList.Controllers
{
    using Common;
    using Common.Models.DTO;
    using Common.Models.ViewModels;
    using Core.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.MetaModel;

    [Authorize]
    public class BaseController : Controller
    {
        protected readonly IContextService _contextService;

        protected BaseController(IContextService contextService)
        {
            _contextService = contextService;
        }

        protected void AssignCommonViewModelData(IBaseMetaModel metaModel, BaseViewModel viewModel)
        {
            viewModel.BaseCurrency = metaModel.BaseCurrency;
            viewModel.AccessToken = HttpContext.Request.Query["access_token"];
            viewModel.Lang = metaModel.Lang;
        }

        protected static void AssignCommonMetaModelValue(IBaseBetListMetaModel metaModel, LicenseeInfo licenseeInfo)
        {
            metaModel.HasPlayerCom = licenseeInfo.IsAllowedCustomerComm;
            metaModel.AgentListXml = SystemHelper.GenerateAgentListXml(licenseeInfo);
        }

        protected LicenseeInfo GetLicenseeInfo()
            => new LicenseeInfo
            {
                SiteName = _contextService.SiteName,
                IsAllowedCustomerComm = _contextService.IsAllowedCustomerComm
            };
    }
}