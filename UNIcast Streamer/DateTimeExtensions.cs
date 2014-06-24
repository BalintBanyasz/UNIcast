using System;
using System.Collections.Generic;
using System.Text;

namespace UNIcast_Streamer
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Converts a given DateTime into a Unix timestamp with millisecond precision.
        /// Based on: http://stackoverflow.com/a/22539971.
        /// </summary>
        /// <param name="value">Any DateTime</param>
        /// <returns>The given DateTime in Unix timestamp format</returns>
        public static long ToUnixTimestamp(this DateTime value)
        {
            return (long)Math.Truncate((value.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds);
        }
    }
}
