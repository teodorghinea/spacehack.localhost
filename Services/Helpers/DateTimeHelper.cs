using DataLayer;
using System;
using System.Globalization;

namespace Services.Helpers
{
    public static class DateTimeHelper
    {

        public static DateTime GetUtcFromEpoch(int epochTime)
        {
            return DateTimeOffset.FromUnixTimeSeconds(epochTime).UtcDateTime;
        }

        public static int GetWeekNumber(DateTime time)
        {
            return CultureInfo.CurrentCulture.Calendar
                .GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        /* for dummy data request */
        public static DateTime GetDateFromDummyRequest(string date)
        {
            var dateSplit = date.Split(" ");
            var year  = 2000 + int.Parse(dateSplit[0]);
            var month = int.Parse(dateSplit[1]);
            var day   = int.Parse(dateSplit[2]);
            var hour  = int.Parse(dateSplit[3]);

            return new DateTime(year, month, day, hour, 0, 0);
        }

    }
}
