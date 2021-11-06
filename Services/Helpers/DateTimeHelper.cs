using System;

namespace Services.Helpers
{
    public static class DateTimeHelper
    {

        public static DateTime GetUtcFromEpoch(int epochTime)
        {
            return DateTimeOffset.FromUnixTimeSeconds(epochTime).UtcDateTime;
        }

    }
}
