namespace Fanex.Athena.ExtService.Report.Models.MetaModel
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using Attributes;
    using Common.Models.DTO;
    using Constant;

    public class QueryClosingBalanceMetaModel : IBaseReportMetaModel
    {
        [Required(ErrorMessage = "Date format is incorrect.")]
        [CheckDate(ErrorMessage = "Value for Date must be between {1:MM/dd/yyyy} and {2:MM/dd/yyyy}")]
        public DateTime? WinLostDate => DateTime.TryParseExact(
            Date,
            ReportOptions.DateFormatPattern,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out var value)
            ? value
            : null;

        [Required(AllowEmptyStrings = false)]
        public string Date { get; set; }

        public string AgentListXml { get; set; }

        public int SpecialPlatform { get; set; }

        public BalanceUp BalanceUp { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Value for {0} must be greater than 0")]
        public int PageIndex { get; set; } = ReportOptions.DefaultPageIndex;

        [Range(1, ReportOptions.ClosingBalanceMaxPageSize, ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public int PageSize { get; set; } = ReportOptions.ClosingBalanceDefaultPageSize;
    }
}