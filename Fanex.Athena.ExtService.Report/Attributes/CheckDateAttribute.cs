namespace Fanex.Athena.ExtService.Report.Attributes
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using Constant;
    using Core.Helper;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class CheckDateAttribute : ValidationAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, GetMinDate(), GetMaxDate());
        }

        public override bool IsValid(object value)
        {
            if (value is DateTime dateTime)
            {
                return dateTime.Ticks >= GetMinDate().Ticks && dateTime.Ticks <= GetMaxDate().Ticks;
            }

            return false;
        }

        private static DateTime GetMaxDate()
        {
            var date = DateTimeHelper.UtcNow.AddHours(-4).AddDays(-ReportOptions.MinSupportedDaysFromToday);

            return DateTimeHelper.CreateDateTime(date.Year, date.Month, date.Day);
        }

        private static DateTime GetMinDate()
        {
            var date = DateTimeHelper.UtcNow.AddHours(-4).AddDays(-ReportOptions.MaxSupportedDaysFromToday);

            return DateTimeHelper.CreateDateTime(date.Year, date.Month, date.Day);
        }
    }
}