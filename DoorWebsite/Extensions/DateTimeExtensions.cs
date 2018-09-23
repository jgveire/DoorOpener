namespace DoorWebsite.Extensions
{
    using System;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore.Internal;

    public static class DateTimeExtensions
    {
        public static bool IsWeekend(this DateTime date)
        {
            return DateTime.Now.DayOfWeek == DayOfWeek.Saturday || 
                   DateTime.Now.DayOfWeek == DayOfWeek.Sunday;
        }

        public static bool IsDutchHoliday(this DateTime date)
        {
            var year = DateTime.Today.Year;
            var easter = CalculateEaster(year);
            var holidays = new List<DateTime>();
            holidays.Add(easter.AddDays(-2)); // good Friday
            holidays.Add(easter); // First easter day
            holidays.Add(easter.AddDays(1)); // Second easter day
            holidays.Add(easter.AddDays(39)); // Ascension day
            holidays.Add(easter.AddDays(49)); // Pentecost day
            holidays.Add(easter.AddDays(50)); // Second Pentecost day
            holidays.Add(new DateTime(year, 1, 1)); // New years day
            holidays.Add(new DateTime(year, 4, 27)); // Kings day
            holidays.Add(new DateTime(year, 12, 24)); // First Christmas
            holidays.Add(new DateTime(year, 12, 25)); // Second Christmas day

            return holidays.Contains(DateTime.Today);
        }

        private static DateTime CalculateEaster(int year)
        {
            var y = year;
            var a = y % 19;
            var b = Math.Floor(y / 100D);
            var c = y % 100;
            var d = Math.Floor(b / 4D);
            var e = (int)b % 4;
            var f = Math.Floor((b + 8) / 25D);
            var g = Math.Floor((b - f + 1) / 3D);
            var h = (19 * a + (int)b - (int)d - (int)g + 15) % 30;
            var i = Math.Floor(c / 4D);
            var k = c % 4;
            var l = (32 + 2 * e + 2 * (int)i - h - k) % 7;
            var m = Math.Floor((a + 11 * h + 22 * l) / 451D);
            var month = (int)Math.Floor((h + l - 7 * m + 114) / 31D);
            var day = (int)((h + l - 7 * (int)m + 114) % 31) + 1;

            return new DateTime(year, month, day);
        }
    }
}
