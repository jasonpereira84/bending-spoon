using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Collections;

namespace JFPGeneric
{
    public partial class Functions
    {
        public static Int32 GetCardinality(BitArray bitArray)
        {
            Int32[] ints = new Int32[(bitArray.Count >> 5) + 1];

            bitArray.CopyTo(ints, 0);

            Int32 count = 0;

            // fix for not truncated bits in last integer that may have been set to true with SetAll()
            ints[ints.Length - 1] &= ~(-1 << (bitArray.Count % 32));

            for (Int32 i = 0; i < ints.Length; i++)
            {
                Int32 c = ints[i];

                unchecked
                {
                    c = c - ((c >> 1) & 0x55555555);
                    c = (c & 0x33333333) + ((c >> 2) & 0x33333333);
                    c = ((c + (c >> 4) & 0xF0F0F0F) * 0x1010101) >> 24;
                }
                count += c;

            }

            return count;
        }

        public static BitArray GetBinary(Int32 numeral)
        {
            return new BitArray(new[] { numeral });
        }

        public static BitArray GetBinary(List<DateTime> attendDates)
        {
            var retVal = new BitArray(32, false);
            foreach (var date in attendDates)
                retVal.Set(date.Day, true);

            return retVal;
        }

        public static Int32 GetNumeral(BitArray binary)
        {
            if (binary == null)
                throw new ArgumentNullException("binary");
            if (binary.Length > 32)
                throw new ArgumentException("must be at most 32 bits long");

            var result = new Int32[1];
            binary.CopyTo(result, 0);
            return result[0];
        }

        public static Int32 GetNumeral(List<DateTime> attendDates)
        {
            return GetNumeral(GetBinary(attendDates));
        }

        public static Int32 GetAttendanceCount(Int32 attendance) 
        {
            return GetCardinality(GetBinary(attendance));
        }

        public static Int32 GetAttendanceCount(List<DateTime> attendDates, DateTime startRangeDate, DateTime stopRangeDate, Boolean includeStartRangeDate = true, Boolean includeStopRangeDate = true)
        {
            if (attendDates == null) { throw new ArgumentNullException("attendDates"); }

            IEnumerable<DateTime> datesInRange;
            if (!includeStartRangeDate) { datesInRange = attendDates.Where(d => d.Date > startRangeDate.Date); }
            else { datesInRange = attendDates.Where(d => d.Date >= startRangeDate.Date); }
            if (!includeStopRangeDate) { datesInRange = datesInRange.Where(d => d.Date < stopRangeDate.Date); }
            else { datesInRange = datesInRange.Where(d => d.Date <= stopRangeDate.Date); }

            var validatedDatesInRange = ValidateDates(datesInRange.ToList());

            return validatedDatesInRange.Count();
        }

        public static Int32 GetAttendanceCount(List<DateTime> attendDates, String clientTimezone, DateTime startRangeDate, Boolean includeStartRangeDate = true, Boolean includeStopRangeDate = true)
        {
            var clientToday = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.Date, TimeZoneInfo.FindSystemTimeZoneById(clientTimezone)).Date;
            return GetAttendanceCount(attendDates, startRangeDate, clientToday, includeStartRangeDate, includeStopRangeDate);
        }

        public static Int32 GetAttendanceCountFromFirstOfMonth(List<DateTime> attendDates, String clientTimezone, Boolean includeStartRangeDate = true, Boolean includeStopRangeDate = true)
        {
            var today = DateTime.UtcNow.Date;
            var clientToday = TimeZoneInfo.ConvertTimeFromUtc(today, TimeZoneInfo.FindSystemTimeZoneById(clientTimezone)).Date;
            return GetAttendanceCount(attendDates, new DateTime(today.Year, today.Month, 1), clientToday, includeStartRangeDate, includeStopRangeDate);
        }

        public static Int32 GetAttendanceCountFromFirstOfYear(List<DateTime> attendDates, String clientTimezone, Boolean includeStartRangeDate = true, Boolean includeStopRangeDate = true)
        {
            var today = DateTime.UtcNow.Date;
            var clientToday = TimeZoneInfo.ConvertTimeFromUtc(today, TimeZoneInfo.FindSystemTimeZoneById(clientTimezone)).Date;
            return GetAttendanceCount(attendDates, new DateTime(today.Year, 1, 1), clientToday, includeStartRangeDate, includeStopRangeDate);
        }

        public static List<DateTime> GetDates(UInt16 year, UInt16 month, BitArray attendanceBits)
        {
            var retVal = new List<DateTime>();
            var DateYearMonth = new DateTime(year, month, 1);
            for (int d = 1; d <= 31; d++)
                if (attendanceBits[d])
                {
                    try { retVal.Add(new DateTime(DateYearMonth.Year, DateYearMonth.Month, d)); }
                    catch (ArgumentOutOfRangeException) { }
                }

            return retVal;
        }

        public static List<DateTime> GetDates(UInt16 year, UInt16 month, Int32 attendance)
        {
            return GetDates(year, month, GetBinary(attendance));
        }

        public static List<DateTime> GetDates(String yearString, String monthString, Int32 attendance)
        {
            ushort year = UInt16.Parse(yearString);
            ushort month = UInt16.Parse(monthString);
            if (year < 1900) { throw new Exception("Invalid year: " + year.ToString()); }
            if ((month < 1) || (month > 12)) { throw new Exception("Invalid month: " + month.ToString()); }

            return GetDates(year, month, attendance);
        }

        public static List<DateTime> ValidateDates(List<DateTime> dates)
        {
            var retVal = new List<DateTime>();

            foreach (var yearGroup in dates.GroupBy(d => d.Year))
                foreach (var monthGroup in yearGroup.GroupBy(d => d.Month))
                    foreach (var dayGroup in monthGroup.GroupBy(d => d.Day))
                        retVal.Add(new DateTime(yearGroup.Key, monthGroup.Key, dayGroup.Key));

            return retVal;
        }
    }
}
