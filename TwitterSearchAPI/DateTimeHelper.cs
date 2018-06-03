using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterSearchAPI
{
    internal class DateTimeHelper
    {
        public static DateTime? FromDateTimeMs(string value)
        {
            if (long.TryParse(value, out long msValue))
            {
                var ts = TimeSpan.FromMilliseconds(msValue);
                var dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                    .Add(ts);
                return dt;
            }
            return null;
        }
    }
}
