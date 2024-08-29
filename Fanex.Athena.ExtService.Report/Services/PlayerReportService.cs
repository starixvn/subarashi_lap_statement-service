namespace Fanex.Athena.ExtService.Report.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Models.DTO;
    using Models.MetaModel;
    using Models.ResultModel;
    using Utils;

    public class PlayerReportService : IPlayerReportService
    {
        public async Task<ReportResultBaseModel<PlayerClosingBalance>> QueryClosingBalance(QueryClosingBalanceMetaModel metaModel)
        {
            const string spName = "LAP_API_PlayerBalanceSel";
            using var db = ObjectDbFactory.CreateInstance(spName);

            db.SetParameterValue("@AgentList", metaModel.AgentListXml);
            db.SetParameterValue("@Winlostdate", metaModel.WinLostDate);
            db.SetParameterValue("@PageIndex", metaModel.PageIndex);
            db.SetParameterValue("@PageSize", metaModel.PageSize);

            var data = (await db.QueryAsync<PlayerClosingBalance>()).ToList();
            var output = db.GetOutputParams();
            var totalRows = Convert.ToInt32(output["TotalRows"]);

            return new ReportResultBaseModel<PlayerClosingBalance>(
                data,
                totalRows,
                metaModel.PageIndex,
                metaModel.PageSize,
                metaModel.Date
            );
        }

        public async Task<ReportResultWithBaseCurrencyModel<PlayerReportByProduct>> QueryDataGroupByProduct(
            QueryDataGroupByProductMetaModel metaModel)
        {
            const string spName = "LAP_API_PlayerWinlossByProduct";
            using var db = ObjectDbFactory.CreateInstance(spName);

            db.SetParameterValue("@ProductCodes", metaModel.Product);
            db.SetParameterValue("@AgentList", metaModel.AgentListXml);
            db.SetParameterValue("@Winlostdate", metaModel.WinLostDate);
            db.SetParameterValue("@PageIndex", metaModel.PageIndex);
            db.SetParameterValue("@PageSize", metaModel.PageSize);
            db.SetParameterValue("@BaseCurrency", metaModel.BaseCurrency);

            var data = (await db.QueryAsync<PlayerReportByProduct>()).ToList();
            var output = db.GetOutputParams();
            var totalRows = Convert.ToInt32(output["TotalRows"]);

            // get product name
            foreach (var e in data)
            {
                e.Product = ProductMapper.GetProductName(e.ProductCode);
            }

            return new ReportResultWithBaseCurrencyModel<PlayerReportByProduct>(
                data,
                totalRows,
                metaModel.PageIndex,
                metaModel.PageSize,
                metaModel.Date,
                metaModel.BaseCurrency);
        }

        public async Task<ReportResultWithBaseCurrencyModel<PlayerReportByPlatform>> QueryDataGroupByPlatform(
            QueryDataGroupByPlatformMetaModel metaModel)
        {
            const string spName = "LAP_API_PlayerWinlossByPlatform";
            using var db = ObjectDbFactory.CreateInstance(spName);

            db.SetParameterValue("@PlatformTypeIds", metaModel.Platform);
            db.SetParameterValue("@AgentList", metaModel.AgentListXml);
            db.SetParameterValue("@Winlostdate", metaModel.WinLostDate);
            db.SetParameterValue("@PageIndex", metaModel.PageIndex);
            db.SetParameterValue("@PageSize", metaModel.PageSize);
            db.SetParameterValue("@BaseCurrency", metaModel.BaseCurrency);

            var data = (await db.QueryAsync<PlayerReportByPlatform>()).ToList();

            // get platform name
            foreach (var e in data)
            {
                e.Platform = PlatformMapper.GetPlatformName(e.PlatformType);
            }

            var output = db.GetOutputParams();
            var totalRows = Convert.ToInt32(output["TotalRows"]);

            return new ReportResultWithBaseCurrencyModel<PlayerReportByPlatform>(
                data,
                totalRows,
                metaModel.PageIndex,
                metaModel.PageSize,
                metaModel.Date,
                metaModel.BaseCurrency);
        }
    }
}