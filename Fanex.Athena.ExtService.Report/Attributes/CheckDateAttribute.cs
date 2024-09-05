namespace Fanex.Athena.ExtService.Report.Attributes
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Constant;

    public class CheckDateAttribute : RangeAttribute
    {
        public CheckDateAttribute() : base(
            typeof(DateTime),
            DateTime.UtcNow.AddHours(-4).AddDays(-ReportOptions.MaxSupportedDaysFromToday).ToString(ReportOptions.DateFormatPattern),
            DateTime.UtcNow.AddHours(-4).AddDays(-ReportOptions.MinSupportedDaysFromToday).ToString(ReportOptions.DateFormatPattern))
        {
        }
    }
}