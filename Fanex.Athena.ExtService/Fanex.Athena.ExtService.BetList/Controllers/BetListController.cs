using System.Collections.Generic;
using System.Threading.Tasks;
using Fanex.Athena.ExtService.BetList.Models.DataDomain;
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
        [ResponseCache(Duration = SystemConstants.ThirtySecond, NoStore = false,
            VaryByQueryKeys = new[] { "refId", "urs", "lang", "bc" })]
        public async Task<IActionResult> RefNo(RefNoBetListMetaModel metaModel)
        {
            var siteName = HttpContext.User.Identity.Name;
            var licenseeInfo = await _commonService.GetLicenseeInfoAsync(siteName);
            var viewModel = new RefNoViewModel
            {
                Info = licenseeInfo,
                UserName = metaModel.UserName,

            };
            if (metaModel.IsValid)
            {
                AssignCommonMetaModelValue(metaModel, licenseeInfo);
                var result = await _betListApiClient.PostAsync<RefNoDataResult>("/BettingTicketCenter/GetByRefno",
                metaModel,
                metaModel.Lang
                ).ConfigureAwait(false);

                viewModel.Data = result.Data ?? new List<TicketText>();
            }
            ViewBag.Title = "Bet List By RefNo";

            AssignCommonViewModelData(metaModel, viewModel);

            return View(viewModel);
        }

        [HttpGet]
        [ServiceFilter(typeof(CheckCustomerAttribute))]
        [ResponseCache(Duration = SystemConstants.ThirtySecond, NoStore = false,
            VaryByQueryKeys = new[] { "cid", "lang", "bc", "wd", "pc", "ps", "pi", "mv", "s" })]
        public async Task<IActionResult> ByGroup(BetListByGroupMetaModel metaModel)
        {
            var siteName = HttpContext.User.Identity.Name;
            var licenseeInfo = await _commonService.GetLicenseeInfoAsync(siteName, metaModel.CustId);
            var viewModel = new BetListByProductViewModel
            {
                Info = licenseeInfo,
                Data = new List<TicketText>()
            };

            if (metaModel.IsValid)
            {
                AssignCommonMetaModelValue(metaModel, licenseeInfo);
                var result = await _betListApiClient.PostAsync<RefNoDataResult>("/BetListByProduct/Index",
                metaModel,
                metaModel.Lang
            ).ConfigureAwait(false);

                viewModel.Data = result.Data ?? new List<TicketText>();
            }

            ViewBag.Title = "Bet List By Group";
            AssignCommonViewModelData(metaModel, viewModel);
            viewModel.ProductCode = metaModel.ProductCode;
            viewModel.TransDesc = TransDescValue.GetTransactionDescription(metaModel.TransType);
            return View(viewModel);
        }

    }
}
