﻿using System;
using abremir.postcrossing.engine.Assets;

namespace abremir.postcrossing.engine.Extensions
{
    public static class DateTimeOffsetExtensions
    {
        /// <summary>
        /// <para>Truncates a DateTimeOffset to a specified resolution.</para>
        /// <para>A convenient source for resolution is TimeSpan.TicksPerXXXX constants.</para>
        /// </summary>
        /// <param name="date">The DateTimeOffset object to truncate</param>
        /// <param name="resolution">e.g. to round to nearest second, TimeSpan.TicksPerSecond</param>
        /// <returns>Truncated DateTimeOffset</returns>
        public static DateTimeOffset Truncate(this DateTimeOffset date, long resolution)
        {
            return new DateTimeOffset(date.Ticks - (date.Ticks % resolution), date.Offset);
        }

        public static DateTimeOffset ToHundredthOfSecond(this DateTimeOffset date)
        {
            return date.Truncate(PostcrossingTrackerConstants.TicksPerHundredthOfSecond);
        }
    }
}
