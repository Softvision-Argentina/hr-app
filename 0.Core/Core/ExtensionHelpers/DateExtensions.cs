using System;

namespace Core.ExtensionHelpers
{
    public static class DateExtensions
    {
        public static bool IsBetween(this DateTime date, DateTime min, DateTime max)
        {
            return date >= min && date <= max;
        }

        public static bool IsInPeriod(this DateTime date, DateTime period)
        {
            var firstDayPeriod = new DateTime(period.Year, period.Month, 1);
            var lastDayPeriod = firstDayPeriod.AddMonths(1).AddDays(-1);

            return date.IsBetween(firstDayPeriod, lastDayPeriod);
        }
    }
}
