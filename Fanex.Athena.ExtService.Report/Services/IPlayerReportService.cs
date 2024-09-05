namespace Fanex.Athena.ExtService.Report.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models.DTO;
    using Models.MetaModel;
    using Models.ResultModel;

    public interface IPlayerReportService
    {
        Task<ReportResultBaseModel<PlayerClosingBalance>> QueryClosingBalance(
            QueryClosingBalanceMetaModel metaModel,
            Dictionary<int, string> currencies);

        Task<ReportResultWithBaseCurrencyModel<PlayerReportByProduct>> QueryDataGroupByProduct(QueryDataGroupByProductMetaModel metaModel);

        Task<ReportResultWithBaseCurrencyModel<PlayerReportByPlatform>> QueryDataGroupByPlatform(QueryDataGroupByPlatformMetaModel metaModel);
    }
}