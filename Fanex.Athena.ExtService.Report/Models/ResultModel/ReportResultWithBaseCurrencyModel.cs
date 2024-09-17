namespace Fanex.Athena.ExtService.Report.Models.ResultModel
{
    using System.Collections.Generic;

    public class ReportResultWithBaseCurrencyModel<T> : ReportResultBaseModel<T> where T : class
    {
        public ReportResultWithBaseCurrencyModel(
            List<T> data,
            int totalRows,
            int pageIndex,
            int pageSize,
            string date,
            string baseCurrency) : base(data, totalRows, pageIndex, pageSize, date)
        {
            BaseCurrency = baseCurrency;
        }

        public string BaseCurrency { get; set; }
    }
}