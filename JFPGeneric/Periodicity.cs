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
using MoreLinq;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace JFPGeneric
{
    [JsonObject]
    public class Periodicity
    {
        [JsonConstructor]
        public Periodicity()
        {
            Values = new List<int>();
        }

        [JsonProperty("CategoryID")]
        [Required]
        [Range(0, Int32.MaxValue)]
        [Display(Name = "Category")]
        public Int32 CategoryID { get; set; }

        [JsonProperty("LevelID")]
        [Required]
        [Range(0, Int32.MaxValue)]
        [Display(Name = "Category")]
        public Int32 LevelID { get; set; }

        [JsonProperty("Values")]
        [Required]
        public IList<Int32> Values { get; set; }
    }

    public class TodayDetailsISO8601
    {
        public TodayDetailsISO8601(DateTime date, CultureInfo cultureInfo)
        {
            DayOfYear = date.DayOfYear;
            WeekOfYear = date.WeekOfYearISO8601(cultureInfo);
            MonthOfYear = date.Month;
            DayOfMonth = date.Day;
            WeekOfMonth = date.WeekOfMonthISO8601(cultureInfo);
            DayOfWeek = (int)date.DayOfWeek + 1;
        }

        public Int32 DayOfYear { get; private set; }

        public Int32 WeekOfYear { get; private set; }

        public Int32 MonthOfYear { get; private set; }

        public Int32 DayOfMonth { get; private set; }

        public Int32 WeekOfMonth { get; private set; }

        public Int32 DayOfWeek { get; private set; }
    }

    public partial class Functions
    {
        public static Periodicity[] ValidatePeriodicity(params Periodicity[] periodicities)
        {
            var temp = new List<Periodicity>();

            var period_0 = periodicities.MaxBy(p => p.CategoryID); 
            temp.Add(period_0);
            if (period_0.LevelID > 0)
            {
                var period_1 = periodicities.Except(temp).Single(p => p.CategoryID == period_0.LevelID);
                temp.Add(period_1);
                if (period_1.LevelID > 0)
                {
                    var period_2 = periodicities.Except(temp).Single(p => p.CategoryID == period_1.LevelID);
                    temp.Add(period_2);
                }
            }

            var catGroups_ALL = temp.GroupBy(p => p.CategoryID);
            if (catGroups_ALL.Any(g => g.Count() > 1)) { throw new InvalidDataException("There is a duplicate category/categoryID in periodicities"); }

            var lvlGroups_ALL = temp.GroupBy(p => p.LevelID);
            if (lvlGroups_ALL.Any(g => g.Count() > 1)) { throw new InvalidDataException("There is a duplicate level/levelID in periodicities"); }

            return temp.OrderByDescending(p => p.CategoryID).ToArray();
        }

        public static Result MatchToday2Periodicity(Periodicity[] periodicities,TodayDetailsISO8601 todayDetails, out Boolean matchesPeriodicity)
        {
            Result retVal = null;
            matchesPeriodicity = false;
            try
            {
                var periodicity_L0 = periodicities.Single(p => p.LevelID == 0);
                switch (periodicity_L0.CategoryID)
                {
                    case 3:
                        matchesPeriodicity = periodicity_L0.Values.Contains(todayDetails.DayOfYear);
                        break;
                    case 2:
                        if (!periodicities.Any(p => p.LevelID == 2)) { matchesPeriodicity = periodicity_L0.Values.Contains(todayDetails.DayOfMonth); }
                        else
                        {
                            var periodicity_L2 = periodicities.Single(p => p.LevelID == 2);
                            matchesPeriodicity = periodicity_L2.Values.Contains(todayDetails.MonthOfYear)
                                && periodicity_L0.Values.Contains(todayDetails.DayOfMonth);
                        }
                        break;
                    case 1:
                        if (!periodicities.Any(p => p.LevelID == 1)) { matchesPeriodicity = periodicity_L0.Values.Contains(todayDetails.DayOfWeek); }
                        else
                        {
                            var periodicity_L1 = periodicities.Single(p => p.LevelID == 1);
                            switch (periodicity_L1.CategoryID)
                            {
                                case 3:
                                    matchesPeriodicity = periodicity_L1.Values.Contains(todayDetails.WeekOfYear)
                                        && periodicity_L0.Values.Contains(todayDetails.DayOfWeek);
                                    break;
                                case 2:
                                    if (!periodicities.Any(p => p.LevelID == 2))
                                        matchesPeriodicity = periodicity_L1.Values.Contains(todayDetails.WeekOfMonth)
                                            && periodicity_L0.Values.Contains(todayDetails.DayOfWeek);
                                    else
                                    {
                                        var periodicity_L2 = periodicities.Single(p => p.LevelID == 2);
                                        matchesPeriodicity = periodicity_L2.Values.Contains(todayDetails.MonthOfYear)
                                            && periodicity_L1.Values.Contains(todayDetails.WeekOfMonth)
                                            && periodicity_L0.Values.Contains(todayDetails.DayOfWeek);
                                    }
                                    break;
                                default:
                                    throw new ArgumentException("Invalid Level1-Category");
                            }
                        }
                        break;
                    default:
                        throw new ArgumentException("Invalid Level0-Category");
                }

                if (retVal == null) { retVal = new Result(true); }
            }
            catch (Exception x)
            {
                if (retVal == null) { retVal = new Result(x); }
                else { retVal.AddError(x); }
            }
            return retVal;
        }
    }
}
