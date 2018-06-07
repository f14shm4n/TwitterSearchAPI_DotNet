using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterSearchAPI
{
    /// <summary>
    /// Provides date time actions.
    /// </summary>
    internal class DateTimeHelper
    {
        /// <summary>
        /// Parse string date time value to the <see cref="DateTime"/> type.
        /// </summary>
        /// <param name="value">Date and time as string.</param>
        /// <returns>The DateTime or null.</returns>
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
