using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace Qarth
{
    public static class TimeHelper
    {
        public static DateTime TicksToDateTime(this string target)
        {
            
            if (string.IsNullOrEmpty(target))
            {
                return DateTime.MinValue;
            }
            
            return new DateTime(long.Parse(target));
        }

        public static long PassSecond(this string target)
        {
            
            if (string.IsNullOrEmpty(target))
            {
                return int.MaxValue;
            }

            long ticks = long.Parse(target);
            long now = DateTime.Now.Ticks;
            return (now - ticks) / 10000000;
        }

        public static TimeSpan GetSurplusTime(DateTime targetTime)
        {            
            return targetTime.Subtract(DateTime.Now);
        }

        public static TimeSpan GetPastTime(DateTime targetTime)
        {
            return DateTime.Now.Subtract(targetTime);
        }

        public static bool IsToday(DateTime targetTime)
        {
            if(targetTime.Day == DateTime.Now.Day)
            {
                return true;
            }
            return false;
        }

        public static bool IsYesterday(DateTime targetTime)
        {
            if ((DateTime.Now.Day-targetTime.Day) == 1)
            {
                return true;
            }
            return false;
        }
    }
}
