namespace Fanex.Athena.ExtService.BetList.Services
{
    using System;
    using System.Threading.Tasks;
    using Common.Services;
    using Data;
    using Models.DTO;
    using Models.MetaModel;
    using Models.ViewModel;

    public class SummaryStatementService : ISummaryStatementService
    {
        private readonly ICommonService _commonService;

        public SummaryStatementService(ICommonService commonService)
        {
            _commonService = commonService;
        }

        public async Task<SummaryStatementViewModel> QueryAsync(SummaryStatementMetaModel metaModel)
        {
            const string UspName = "LAP_Statement_Mem";
            var totalRows = 0;
            var balanceUpInfo = await _commonService.GetBalanceUpAsync().ConfigureAwait(false);
            using (var db = ObjectDbFactory.CreateInstance(UspName))
            {
                db.SetParameterValue("custId", metaModel.CustId);
                db.SetParameterValue("FromDate", metaModel.FromDate);
                db.SetParameterValue("@ToDate", metaModel.ToDate);
                db.SetParameterValue("@BalanceUpDay", balanceUpInfo.BalanceUpDay);
                db.SetParameterValue("@MoveBetsDay", balanceUpInfo.MoveBetsDay);
                db.SetParameterValue("@PageSize", metaModel.PageSize);
                db.SetParameterValue("@PageIndex", metaModel.PageIndex);
                db.SetParameterValue("@baseCurrency", metaModel.BaseCurrency);
                db.SetParameterValue("@ReportViewType", metaModel.ViewType);
                db.SetParameterValue("@TotalRows", totalRows);
                var statementResult = await db.QueryAsync<SummaryStatement>();
                totalRows = Convert.ToInt32(db.GetOutputParams()["TotalRows"]);

                return new SummaryStatementViewModel
                {
                    DataList = statementResult,
                    TotalRows = totalRows
                };
            }
        }
    }
}