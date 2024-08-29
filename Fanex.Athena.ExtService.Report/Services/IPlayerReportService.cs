namespace Fanex.Athena.ExtService.Report.Services
{
    using System.Threading.Tasks;
    using Models.DTO;
    using Models.MetaModel;
    using Models.ResultModel;

    public interface IPlayerReportService
    {
        Task<ReportResultBaseModel<PlayerClosingBalance>> QueryClosingBalance(QueryClosingBalanceMetaModel metaModel);

        Task<ReportResultWithBaseCurrencyModel<PlayerReportByProduct>> QueryDataGroupByProduct(QueryDataGroupByProductMetaModel metaModel);

        Task<ReportResultWithBaseCurrencyModel<PlayerReportByPlatform>> QueryDataGroupByPlatform(QueryDataGroupByPlatformMetaModel metaModel);
    }
}