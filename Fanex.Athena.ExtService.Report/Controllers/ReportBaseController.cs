namespace Fanex.Athena.ExtService.Report.Controllers
{
    using Common;
    using Common.Models.DTO;
    using Microsoft.AspNetCore.Mvc;
    using Models.MetaModel;
    using Newtonsoft.Json;

    public class ReportBaseController : Controller
    {
        public static void AssignCommonMetaModelValue(
            IBaseReportMetaModel metaModel,
            LicenseeInfo licenseeInfo,
            int specialPlatform,
            BalanceUp balanceUp)
        {
            metaModel.AgentListXml = SystemHelper.GenerateAgentListXml(licenseeInfo, true);
            metaModel.SpecialPlatform = specialPlatform;
            metaModel.BalanceUp = balanceUp;
        }

        public ActionResult JsonResult(object data)
        {
            var json = JsonConvert.SerializeObject(data);

            return new ContentResult
            {
                Content = json,
                ContentType = "application/json"
            };
        }
    }
}