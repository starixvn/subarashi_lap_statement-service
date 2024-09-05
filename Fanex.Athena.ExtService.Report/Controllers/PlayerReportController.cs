namespace Fanex.Athena.ExtService.Report.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Services;
    using Core.Attributes;
    using Microsoft.AspNetCore.Mvc;
    using Models.MetaModel;
    using Services;

    [ApiController]
    [ServiceFilter(typeof(ApiKeyAuthAttribute))]
    [Route("api/{controller}/{action}")]
    public class PlayerReportController : ReportBaseController
    {
        private readonly ICommonService _commonService;
        private readonly IPlayerReportService _playerReportService;

        public PlayerReportController(
            IPlayerReportService playerReportService,
            ICommonService commonService)
        {
            _playerReportService = playerReportService;
            _commonService = commonService;
        }

        [HttpPost]
        public async Task<ActionResult> QueryClosingBalance([FromBody] QueryClosingBalanceMetaModel metaModel)
        {
            var (isValid, errorMsg) = IsValidRequest();

            if (!isValid)
            {
                return BadRequest(errorMsg);
            }

            await AssignCommonMetaModel(metaModel);
            var currencies = await _commonService.GetAllCurrencyInfo();

            var result = await _playerReportService.QueryClosingBalance(metaModel, currencies);

            return JsonResult(result);
        }

        [HttpPost]
        public async Task<ActionResult> QueryDataGroupByProduct([FromBody] QueryDataGroupByProductMetaModel metaModel)
        {
            var (isValid, errorMsg) = IsValidRequest();

            if (!isValid)
            {
                return BadRequest(errorMsg);
            }

            await AssignCommonMetaModel(metaModel);

            var result = await _playerReportService.QueryDataGroupByProduct(metaModel);

            return JsonResult(result);
        }

        [HttpPost]
        public async Task<ActionResult> QueryDataGroupByPlatform([FromBody] QueryDataGroupByPlatformMetaModel metaModel)
        {
            var (isValid, errorMsg) = IsValidRequest();

            if (!isValid)
            {
                return BadRequest(errorMsg);
            }

            await AssignCommonMetaModel(metaModel);

            var result = await _playerReportService.QueryDataGroupByPlatform(metaModel);

            return JsonResult(result);
        }

        private (bool isValid, string errorMsg) IsValidRequest()
        {
            if (ModelState.IsValid)
            {
                return (true, "");
            }

            var allErrors = ModelState.Values.SelectMany(v => v.Errors);
            var errorMsg = $"Invalid Request: {string.Join(", ", allErrors.Select(x => x.ErrorMessage))}";
            return (false, errorMsg);
        }

        private async Task AssignCommonMetaModel(IBaseReportMetaModel meaModel)
        {
            var siteName = HttpContext.User.Identity.Name;
            var licenseeInfo = await _commonService.GetLicenseeInfoAsync(siteName);
            var specialPlatform = _commonService.GetSpecialPlatform(siteName);

            AssignCommonMetaModelValue(meaModel, licenseeInfo, specialPlatform);
        }
    }
}