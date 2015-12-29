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
    public struct Birthday
    {
        private DateTime _date;
        private Age _age;

        public Birthday(DateTime? birthdate, DateTime clientToday)
        {
            clientToday = clientToday.Date;

            DateTime date = birthdate ?? DateTime.MinValue;
            date = date.Date;

            if (date >= clientToday) { throw new ArgumentOutOfRangeException("The Birthday cannot be now or in the future"); }

            _date = date;
            _age = Age.CalculateAge(_date, clientToday);
        }

        public Boolean IsNullOrWhitespace
        {
            get { return Age.Years <= 0 ? Age.Months <= 0 ? Age.Days <= 0 ? true : false : false : false; }
        }

        public Int32 Year
        {
            get { return _date.Year; }
        }

        public Int32 Month
        {
            get { return _date.Month; }
        }

        public Int32 Day
        {
            get { return _date.Day; }
        }

        public override string ToString()
        {
            return _date.ToString("MMMM d yyyy");
        }

        public DateTime Date
        {
            get { return _date; } 
        }

        public Age Age
        {
            get { return _age; }
        }

        public String Birthday4Display
        {
            get { return this.ToString(); }
        }

        public String Age4Display
        {
            get { return Age.ToString(); }
        }

        public String BirthdayAge4Display
        {
            get { return this.ToString() + " (" + Age.ToString() + ")"; }
        }
    }

    public partial class Functions
    {
        public static Birthday GetUpcomingBirthday(DateTime birthdate, DateTime clientToday)
        {
            return new Birthday(birthdate, birthdate.SetYear(birthdate.SetYear(clientToday.Year) < clientToday ? clientToday.Year + 1 : clientToday.Year));
        }
    }

    public struct Age
    {
        public readonly int Years;
        public readonly int Months;
        public readonly int Days;

        public Age(int years, int months, int days)
            : this()
        {
            Years = years;
            Months = months;
            Days = days;
        }

        public override string ToString()
        {
            return Years.ToString() + " years, " + Months.ToString() + " months, " + Days.ToString() + " days";
        }

        public static Age CalculateAge(DateTime startDate, DateTime endDate)
        {
            if (startDate.Date > endDate.Date) { throw new ArgumentException("startDate cannot be higher then endDate", "startDate"); }

            int years = endDate.Year - startDate.Year;
            int months = 0;
            int days = 0;

            // Check if the last year, was a full year.
            if (endDate < startDate.AddYears(years) && years != 0)
            {
                years--;
            }

            // Calculate the number of months.
            startDate = startDate.AddYears(years);

            if (startDate.Year == endDate.Year)
            {
                months = endDate.Month - startDate.Month;
            }
            else
            {
                months = (12 - startDate.Month) + endDate.Month;
            }

            // Check if last month was a complete month.
            if (endDate < startDate.AddMonths(months) && months != 0)
            {
                months--;
            }

            // Calculate the number of days.
            startDate = startDate.AddMonths(months);

            days = (endDate - startDate).Days;

            return new Age(years, months, days);
        }
    }
}
