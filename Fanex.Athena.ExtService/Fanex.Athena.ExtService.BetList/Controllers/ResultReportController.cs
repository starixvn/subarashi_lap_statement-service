using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fanex.Athena.ExtService.BetList.Models.MetaModel;
using Fanex.Athena.ExtService.Common.Model.ViewModels;
using Fanex.Athena.ExtService.Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.ExtService.BetList.Controllers
{
    [Authorize]
    public class ResultReportController : BaseController
    {
        private readonly IBetListApiClient _betListApiClient;

        public ResultReportController(IBetListApiClient betListApiClient)
        {
            _betListApiClient = betListApiClient;
        }

        [HttpGet]
        public async Task<IActionResult> ResultPopup(ResultPopupMetalModel metaModel)
        {
            var result = await _betListApiClient.PostAsyncString(
                 "/ResultReport/ResultPopup",
                 metaModel,
                 metaModel.Lang
                 ).ConfigureAwait(false);

            ViewBag.Data = result;
            var viewModel = new BaseViewModel
            {
                AccessToken = HttpContext.Request.Query["access_token"],
                Lang = metaModel.Lang
            };
            return View(viewModel);


        }

        [HttpPost]
        public async Task<IActionResult> ShowRngCasinoSingleWalletResult(string refNo, string lang)
        {
            var result = await _betListApiClient.PostAsyncString(
                "/ResultReport/ShowRngCasinoSingleWalletResult",
                new { refNo },
                lang
                )
                .ConfigureAwait(false);

            return Content(result, "application/json");
        }

        [HttpGet]
        public async Task<string> VoidbridgeResult(string custId, string gamePlayId, string lang)
        {
            var model = new
            {
                custId,
                gamePlayId,
                languageCode = lang
            };
            return await _betListApiClient.PostAsyncString("/VoidbridgeResultPopup/Index",
                model, lang).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<string> PPResultPopup(string custId, string gamePlayId, string lang)
        {
            var model = new
            {
                custId,
                gamePlayId,
                languageCode = lang
            };
            return await _betListApiClient.PostAsyncString("/PPResultPopup/Index", model, lang).ConfigureAwait(false);
        }
        [HttpGet]
        public async Task<ActionResult> AllBetResult(string transDesc, string betCheck, string betType, string lang)
        {
            var model = new
            {
                transDesc,
                betCheck,
                betType,
                languageCode = lang
            };
            var data = await _betListApiClient.PostAsyncString("/AllBetResultPopup/Index", model, lang).ConfigureAwait(false);
            ViewBag.Data = data.Replace("{rootUrl}", Url.Content("~/"));
            var viewModel = new BaseViewModel
            {
                AccessToken = HttpContext.Request.Query["access_token"],
                Lang = lang
            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<string> GlobalGamingResult(string custId, string gameId, string lang)
        {
            var model = new
            {
                custId,
                gameId
            };
            return await _betListApiClient.PostAsyncString("/GlobalGamingResultPopup/Index",
                model, lang).ConfigureAwait(false);
        }

    }
}
