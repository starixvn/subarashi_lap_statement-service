namespace Fanex.Athena.ExtService.Core.Helper
{
    using System;
    using System.Globalization;

    public static class DateTimeHelper
    {
        public static DateTime UtcNow => DateTimeOffset.UtcNow.DateTime;

        public static DateTime Now => DateTimeOffset.Now.DateTime;

        public static DateTime MinValue => DateTimeOffset.MinValue.DateTime;

        public static DateTime Parse(string time)
        {
            return DateTime.TryParse(time, CultureInfo.InvariantCulture, out DateTime dt) ? dt : MinValue;
        }

        public static DateTime CreateDateTime(int year, int month, int day, int hour = 0, int minute = 0, int second = 0, TimeSpan? offset = null)
        {
            var actualOffset = offset ?? DateTimeOffset.UtcNow.Offset;
            return new DateTimeOffset(year, month, day, hour, minute, second, actualOffset).DateTime;
        }
    }
}