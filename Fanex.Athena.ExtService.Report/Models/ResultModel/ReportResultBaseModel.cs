namespace Fanex.Athena.ExtService.Report.Models.ResultModel
{
    using System.Collections.Generic;

    public class ReportResultBaseModel<T> where T : class
    {
        public ReportResultBaseModel(List<T> data, int totalRows, int pageIndex, int pageSize, string date)
        {
            Data = data;
            TotalRows = totalRows;
            PageIndex = pageIndex;
            PageSize = pageSize;
            Date = date;
        }

        public string Date { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalRows { get; set; }

        public IEnumerable<T> Data { get; set; }
    }
}