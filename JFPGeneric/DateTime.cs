using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace JFPGeneric
{
    public partial class Functions
    {
        public static DateTime JSONDateParse(String jsonDateString, DateTime defaultDate)
        {
            DateTime retVal = defaultDate;
            try
            {
                if (!DateTime.TryParseExact(jsonDateString, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out retVal))
                    retVal = defaultDate;
            }
            catch (Exception) { retVal = defaultDate; }
            return retVal;
        }

        public static Int32 WeekOfYearISO8601(DateTime date, CultureInfo cultureInfo)
        {
            var calendar = cultureInfo.Calendar;

            var calendarWeekRule = cultureInfo.DateTimeFormat.CalendarWeekRule;
            var firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            var lastDayOfWeek = cultureInfo.LCID == 1033 //En-us
                                ? DayOfWeek.Saturday
                                : DayOfWeek.Sunday;

            var lastDayOfYear = new DateTime(date.Year, 12, 31);

            var weekNumber = calendar.GetWeekOfYear(date, calendarWeekRule, firstDayOfWeek);

            //Check if this is the last week in the year and it doesn`t occupy the whole week
            return weekNumber == 53 && lastDayOfYear.DayOfWeek != lastDayOfWeek
                   ? 1
                   : weekNumber;
        }

        public static DateTime FirstDateOfWeekISO8601(Int32 year, Int32 weekOfYear, CultureInfo cultureInfo)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = cultureInfo.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek <= 1) { weekNum -= 1; }

            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }

        public static DateTime FirstDateOfWeekISO8601(DateTime date, CultureInfo cultureInfo)
        {
            return FirstDateOfWeekISO8601(date.Year, WeekOfYearISO8601(date, cultureInfo), cultureInfo);
        }

        public static Int32 WeekOfMonthISO8601(DateTime date, CultureInfo cultureInfo)
        {
            return WeekOfYearISO8601(date, cultureInfo) - WeekOfYearISO8601(new DateTime(date.Year, date.Month, 1), cultureInfo) + 1; // Or skip +1 if you want the first week to be 0.
        }

        public static DateTime Parse_CardExpirationDate(String cardExpirationDate)
        {
            var retVal = DateTime.MinValue;
            if (DateTime.TryParseExact(cardExpirationDate, "MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out retVal))
                retVal = retVal.LastDateOfMonth();
            return retVal;
        }

        public static Int64 ConvertToUnixTime(DateTime utcDateTime)
        {
            var timeSpan = (utcDateTime - new DateTime(1970, 1, 1, 0, 0, 0));
            return (Int64)timeSpan.TotalSeconds;
        }

        public static DateTime RoundUp(DateTime date, TimeSpan d)
        {
            return new DateTime(((date.Ticks + d.Ticks - 1) / d.Ticks) * d.Ticks);
        }
    }

    public static class DateTimeExtensions
    {
        public static DateTime Tomorrow(this DateTime date)
        {
            return date.AddDays(1);
        }

        public static DateTime Yesterday(this DateTime date)
        {
            return date.AddDays(-1);
        }

        public static Int32 WeekOfYearISO8601(this DateTime date, CultureInfo cultureInfo)
        {
            return Functions.WeekOfYearISO8601(date, cultureInfo);
        }

        public static DateTime FirstDateOfWeekISO8601(this DateTime date, CultureInfo cultureInfo)
        {
            return Functions.FirstDateOfWeekISO8601(date, cultureInfo);
        }

        public static Int32 WeekOfMonthISO8601(this DateTime date, CultureInfo cultureInfo)
        {
            return Functions.WeekOfYearISO8601(date, cultureInfo);
        }

        public static TodayDetailsISO8601 TodayDetailsISO8601(this DateTime date)
        {
            return new TodayDetailsISO8601(date, CultureInfo.InvariantCulture);
        }

        public static TodayDetailsISO8601 TodayDetailsISO8601(this DateTime date, CultureInfo cultureInfo)
        {
            return new TodayDetailsISO8601(date, cultureInfo);
        }

        public static DateTime FirstDateOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static Int32 LastDayOfMonth(this DateTime date)
        {
            return DateTime.DaysInMonth(date.Year, date.Month);
        }

        public static DateTime LastDateOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, LastDayOfMonth(date));
        }

        public static Boolean MonthContainsDay(this DateTime date, Int32 dayOfMonth)
        {
            return dayOfMonth <= LastDayOfMonth(date);
        }

        public static Boolean IsLeapYear(this DateTime date)
        {
            return DateTime.IsLeapYear(date.Year);
        }

        public static Boolean YearContainsDay(this DateTime date, Int32 dayOfYear)
        {
            if (dayOfYear > 366) { return false; }
            else
                if (dayOfYear <= 365) { return true; }
                else { return date.IsLeapYear(); }
        }

        public static Int32 LastDayOfYear(this DateTime date)
        {
            return date.IsLeapYear() ? 366 : 365;
        }

        public static DateTime LastDateOfYear(this DateTime date)
        {
            return new DateTime(date.Year, 12, 31);
        }

        public static DateTime FirstDateOfYear(this DateTime date)
        {
            return new DateTime(date.Year, 1, 1);
        }

        public static DateTime DateFromDayOfMonth(this DateTime date, Int32 dayOfMonth)
        {
            return new DateTime(date.Year, date.Month, dayOfMonth);
        }

        public static DateTime DateFromDayOfYear(this DateTime date, Int32 dayOfYear)
        {
            return date.FirstDateOfYear().AddDays(dayOfYear - 1); 
        }

        public static String GetISO8601Timestamp(this DateTime date)
        {
            if (date.Kind.Equals(DateTimeKind.Utc))
                return date.ToString("yyyy-MM-ddTHH:mm:ss") + "+00:00";
            else
                return date.ToString("yyyy-MM-ddTHH:mm:sszzz");
        }

        public static Int64 UTCToUnixTime(this DateTime utcDateTime)
        {
            return Functions.ConvertToUnixTime(utcDateTime);
        }

        public static DateTime ExpirationDate(Int32 year, Int32 month)
        {
            return LastDateOfMonth(new DateTime(year, month, 1));
        }

        public static DateTime ExpirationDate(this DateTime date)
        {
            return LastDateOfMonth(date);
        }

        public static String ToExpirationDateString(this DateTime date)
        {
            return date.ToString("yyyy-MM");
        }

        public static DateTime SetYear(this DateTime date, Int32 year)
        {
            if ((year < 1) || (year > 9999)) { throw new ArgumentOutOfRangeException("year", "The year value cannot be less than 1 or more than 9999"); }
            var yearDiff = year - date.Year;
            return date.AddYears(yearDiff);
        }

        public static DateTime SetMonth(this DateTime date, Int32 month)
        {
            if ((month < 1) || (month > 12)) { throw new ArgumentOutOfRangeException("month","The month value cannot be less than 1 or more than 12"); }
            var monthDiff = month - date.Month;
            return date.AddMonths(monthDiff);
        }

        public static DateTime SetDay(this DateTime date, Int32 day, Boolean returnNextMonth = false)
        {
            if ((day < 1) || (day > 31)) { throw new ArgumentOutOfRangeException("day", "The day value cannot be less than 1 or more than 31"); }
            var dayDiff = day - date.Day;
            var newDate = date.AddDays(dayDiff);
            if (date.Month.Equals(newDate.Month))
                return newDate;
            else
                if (returnNextMonth)
                    return newDate.FirstDateOfMonth();
                else
                    return date.LastDateOfMonth();
        }

        public static DateTime RoundUp(this DateTime date, TimeSpan d)
        {
            return Functions.RoundUp(date, d);
        }
    }
}
