namespace Fanex.Athena.ExtService.Report.Models.MetaModel
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using Constant;

    public class QueryDataGroupByPlatformMetaModel : IBaseReportMetaModel
    {
        [Required(AllowEmptyStrings = false)]
        public string Platform { get; set; }

        [Required(ErrorMessage = "Date format is incorrect.")]
        public DateTime? WinLostDate => DateTime.TryParseExact(
            Date,
            ReportOptions.DateFormatPattern,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out var value)
            ? value
            : null;

        public string BaseCurrency { get; } = ReportOptions.DefaultBaseCurrency;

        [Required(AllowEmptyStrings = false)]
        public string Date { get; set; }

        public string AgentListXml { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Value for {0} must be greater than 0")]
        public int PageIndex { get; set; } = ReportOptions.DefaultPageIndex;

        [Range(1, ReportOptions.MaxPageSize, ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public int PageSize { get; set; } = ReportOptions.DefaultPageSize;
    }
}