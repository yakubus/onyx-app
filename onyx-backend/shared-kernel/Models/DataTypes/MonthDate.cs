using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Exceptions;
using Models.Responses;

namespace Models.DataTypes
{
    /// <summary>
    /// Date type which represents specific month of the year. Date format is MM/YYYY.
    /// </summary>
    public sealed record MonthDate
    {
        /// <summary>
        /// Index of the month, starting from 1
        /// </summary>
        public int Month { get; init; }
        public int Year { get; init; }
        private const int maxMonth = 12;
        private const int minMonth = 1;

        public MonthDate(int month, int year)
        {
            if (month is < minMonth or > maxMonth)
            {
                throw new DomainException<MonthDate>("Invalid month value");
            }

            if (year < 0)
            {
                throw new DomainException<MonthDate>("Invalid year value");
            }

            Month = month;
            Year = year;
        }

        public static MonthDate Current => new (DateTime.UtcNow.Month, DateTime.UtcNow.Year);

        public override string ToString() => $"{Month:00}/{Year}";

        // Operators
        public static MonthDate operator ++(MonthDate date) =>
            date.Month == 12 ?
                new MonthDate(1, date.Year + 1) :
                new MonthDate(date.Month + 1, date.Year);

        public static MonthDate operator --(MonthDate date) =>
            date.Month == 1 ?
                new MonthDate(12, date.Year - 1) :
                new MonthDate(date.Month - 1, date.Year);

        public static MonthDate operator +(MonthDate date, int monthsToAdd)
        {
            var yearsToAdd = monthsToAdd / 12;
            var monthsRemaining = monthsToAdd % 12;

            var newMonth = date.Month - monthsRemaining;
            var newYear = date.Year - yearsToAdd;

            return new MonthDate(newMonth, newYear);
        }

        public static MonthDate operator -(MonthDate date, int monthsToSubstract)
        {
            var yearsToSubstract = monthsToSubstract / 12;
            var monthsRemaining = monthsToSubstract % 12;

            var newMonth = date.Month - monthsRemaining;
            var newYear = date.Year - yearsToSubstract;

            return new MonthDate(newMonth, newYear);
        }

        public static bool operator <(MonthDate date1, MonthDate date2) =>
            date1.Year < date2.Year || date1.Month < date2.Month;

        public static bool operator >(MonthDate date1, MonthDate date2) =>
            date1.Year > date2.Year || date1.Month > date2.Month;

        public static bool operator <=(MonthDate date1, MonthDate date2) =>
            date1 == date2 || date1 < date2;

        public static bool operator >=(MonthDate date1, MonthDate date2) =>
            date1 == date2 || date1 > date2;
    }
}
