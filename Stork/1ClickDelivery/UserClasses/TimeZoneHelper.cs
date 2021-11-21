using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _1ClickDelivery.UserClasses
{
    public static class TimeZoneHelper
    {
        public static DateTime GetTodayUTCPlus8()
        {
            var serverTime = DateTime.Now;
            var localTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(serverTime, TimeZoneInfo.Local.Id, "UTC");
            var utc8 = localTime.AddHours(8).ToShortDateString();
            return Convert.ToDateTime(utc8);
        }

        public static string GetTodayUTCPlus8String()
        {
            var serverTime = DateTime.Now;
            var localTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(serverTime, TimeZoneInfo.Local.Id, "UTC");
            var utc8 = localTime.AddHours(8).ToShortDateString();
            return utc8;
        }

        public static DateTime GetTodayWithTimeUTCPlus8()
        {
            var serverTime = DateTime.Now;
            var localTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(serverTime, TimeZoneInfo.Local.Id, "UTC");
            return localTime.AddHours(8);
        }
    }
}