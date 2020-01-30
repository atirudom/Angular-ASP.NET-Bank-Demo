using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2.Utils
{
    // Minute level accuracy
    public static class DateTimeExtensionUtils
    {
        public static bool IsSameMinute(this DateTime datetime1, DateTime datetime2)
        {
            return datetime1.Year == datetime2.Year
                && datetime1.Month == datetime2.Month
                && datetime1.Day == datetime2.Day
                && datetime1.Hour == datetime2.Hour
                && datetime1.Minute == datetime2.Minute;
        }

        public static bool IsSameMinuteInDifferentYear(this DateTime datetime1, DateTime datetime2)
        {
            return datetime1.Month == datetime2.Month
                && datetime1.Day == datetime2.Day
                && datetime1.Hour == datetime2.Hour
                && datetime1.Minute == datetime2.Minute;
        }

        public static bool IsSameMinuteInDifferentMonth(this DateTime datetime1, DateTime datetime2)
        {
            return datetime1.Day == datetime2.Day
                && datetime1.Hour == datetime2.Hour
                && datetime1.Minute == datetime2.Minute;
        }

        public static bool IsSameMinuteInDifferentQuarter(this DateTime datetime1, DateTime datetime2)
        {
            // Monthdiff always return positive value
            var monthDiff = System.Math.Abs(((datetime1.Year - datetime2.Year) * 12) + datetime1.Month - datetime2.Month);
            bool isMonthDiffQuarterly = (monthDiff % 3) == 0;
            return isMonthDiffQuarterly
                && datetime1.Day == datetime2.Day
                && datetime1.Hour == datetime2.Hour
                && datetime1.Minute == datetime2.Minute;
        }
    }
}
