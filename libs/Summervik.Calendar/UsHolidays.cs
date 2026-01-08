using System.ComponentModel;
using System.Reflection;

namespace Summervik.Calendar;

/// <summary>
/// Represents the calendar of U.S. holidays.
/// <seealso href="https://www.usa.gov/holidays"/>
/// </summary>
public static class UsHolidays
{
    /// <summary>
    /// A collection of holidays supported by this class.
    /// </summary>
    public static class Names
    {
        public const string NewYears = "New Year's Day";
        public const string MartinLutherKingJr = "Martin Luther King Jr's Birthday";
        public const string InaugurationDay = "Inauguration Date";
        public const string PresidentsDay = "Presidents Day";
        public const string Valentines = "St. Valentine's Day";
        public const string Easter = "Easter Sunday";
        public const string MemorialDay = "Memorial Day";
        public const string Juneteenth = "Juneteenth";
        public const string IndependenceDay = "Independence Day";
        public const string LaborDay = "Labor Day";
        public const string ColumbusDay = "Columbus Day";
        public const string VeteransDay = "Veterans Day";
        public const string Thanksgiving = "Thanksgiving Day";
        public const string ChristmasEve = "Christmas Eve Day";
        public const string Christmas = "Christmas Day";
        public const string NewYearsEve = "New Year's Eve Day";

        /// <summary>
        /// Get the names of all holidays supported by this class.
        /// </summary>
        public static IEnumerable<string> GetAll()
        {
            return typeof(Names)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string))
                .Select(f => (string)f.GetRawConstantValue()!);
        }
    }


    [Description(Names.NewYears)]
    public static DateOnly? NewYearsDay(int year) => new(year, 1, 1);

    [Description(Names.MartinLutherKingJr)]
    public static DateOnly? MartinLutherKingJrBirthday(int year)
    {
        if (year < 1986)
            return null;

        var day = Enumerable.Range(1, 28)
            .Where(d => new DateOnly(year, 1, d).DayOfWeek is DayOfWeek.Monday)
            .ElementAt(2);

        return new DateOnly(year, 1, day);
    }

    [Description(Names.InaugurationDay)]
    public static DateOnly? InaugurationDay(int year)
    {
        if (year % 4 != 1)
            return null;

        DateOnly? dt;
        if (year > 1936)                     // starting in 1936
            dt = new DateOnly(year, 1, 20);
        else if (year == 1789)               // first celebrated
            dt = new DateOnly(year, 4, 30);
        else if (year > 1789)                // from 1793 to 1933
            dt = new DateOnly(year, 3, 4);
        else
            dt = null;

        return dt is null ? null
            : dt.Value.DayOfWeek is DayOfWeek.Sunday ? dt.Value.AddDays(1) : dt;
    }

    [Description(Names.PresidentsDay)]
    public static DateOnly? PresidentsDay(int year)
    {
        if (year < 1885)
            return null;

        var day = Enumerable.Range(1, 28)
            .Where(d => new DateOnly(year, 2, d).DayOfWeek is DayOfWeek.Monday)
            .ElementAt(2);
        return new DateOnly(year, 2, day);
    }

    /*
     * The true origin of this holiday in the U.S. is fuzzy.
     */
    [Description(Names.Valentines)]
    public static DateOnly? ValentinesDay(int year) => new(year, 2, 14);

    [Description(Names.Easter)]
    public static DateOnly? EasterSunday(int year)
    {
        var g = year % 19;
        var c = year / 100;
        var h = (c - c / 4 - (8 * c + 13) / 25 + 19 * g + 15) % 30;
        var i = h - h / 28 * (1 - h / 28 * (29 / (h + 1)) * ((21 - g) / 11));
        var day = i - (year + year / 4 + i + 2 - c + c / 4) % 7 + 28;

        var month = 3;

        if (day > 31)
        {
            month++;
            day -= 31;
        }

        return new DateOnly(year, month, day);
    }

    [Description(Names.MemorialDay)]
    public static DateOnly? MemorialDay(int year)
    {
        if (year < 1868)
            return null;

        var memorialDay = new DateOnly(year, 5, 31);
        while (memorialDay.DayOfWeek != DayOfWeek.Monday)
        {
            memorialDay = memorialDay.AddDays(-1);
        }
        return memorialDay;
    }

    [Description(Names.Juneteenth)]
    public static DateOnly? Juneteenth(int year) => year < 2021 ? null : new(year, 6, 19);

    [Description(Names.IndependenceDay)]
    public static DateOnly? IndependenceDay(int year) => year < 1870 ? null : new(year, 7, 4);

    [Description(Names.LaborDay)]
    public static DateOnly? LaborDay(int year)
    {
        if (year < 1887)
            return null;
        var laborDay = new DateOnly(year, 9, 1);
        while (laborDay.DayOfWeek is not DayOfWeek.Monday)
            laborDay = laborDay.AddDays(1);
        return laborDay;
    }

    [Description(Names.ColumbusDay)]
    public static DateOnly? ColumbusDay(int year)
    {
        if (year < 1937)
            return null;
        var day = Enumerable.Range(1, 30).Where(d =>
            new DateOnly(year, 10, d).DayOfWeek is DayOfWeek.Monday).ElementAt(1);
        return new DateOnly(year, 10, day);
    }

    [Description(Names.VeteransDay)]
    public static DateOnly? VeteransDay(int year) => year < 1938 ? null : new(year, 11, 11);

    /*
     * The origin of Thanksgiving is sometimes attributed to Lincoln (and with reason),
     * but FDR and Congress made it a thing in 1941.
     */
    [Description(Names.Thanksgiving)]
    public static DateOnly? ThanksgivingDay(int year)
    {
        if (year < 1941)
            return null;
        var day = Enumerable.Range(1, 28)
            .Where(d => new DateOnly(year, 11, d).DayOfWeek is DayOfWeek.Thursday)
            .ElementAt(3);

        return new DateOnly(year, 11, day);
    }

    [Description(Names.ChristmasEve)]
    public static DateOnly? ChristmasEveDay(int year) => year < 1870 ? null :
        ChristmasDay(year)?.AddDays(-1);

    [Description(Names.Christmas)]
    public static DateOnly? ChristmasDay(int year) => year < 1870 ? null :
        new(year, 12, 25);

    [Description(Names.NewYearsEve)]
    public static DateOnly? NewYearsEveDay(int year) => year < 1870 ? null :
        new(year, 12, 31);

    public static string? GetNameForHoliday(DateOnly date)
    {
        var holidaysForYear = GetHolidaysForYear(date.Year);

        var item = holidaysForYear.FirstOrDefault(h => h.Value.Equals(date));

        return item.Equals(default) ? null : item.Key;
    }

    public static IEnumerable<DateOnly> GetWeekDaysExcludingHolidays(DateOnly start, DateOnly finish) =>
        DateUtilities.GetWeekdays(start, finish).Except(
            GetInclusiveHolidaysBetweenDates(start, finish).Where(h =>
                DateUtilities.AdjustToObservedWeekday(h).DayOfWeek != DayOfWeek.Saturday
                && DateUtilities.AdjustToObservedWeekday(h).DayOfWeek != DayOfWeek.Sunday));

    public static int CountWeekDaysExcludingHolidays(DateOnly start, DateOnly finish) => GetWeekDaysExcludingHolidays(start, finish).Count();

    public static IEnumerable<DateOnly> GetInclusiveHolidaysBetweenDates(DateOnly start, DateOnly finish)
    {
        if (finish < start)
            (start, finish) = (finish, start);

        foreach (int year in Enumerable.Range(start.Year, (finish.Year - start.Year) + 1))
            foreach (var holiday in GetHolidaysForYear(year).Values.Where(k => k.HasValue && k >= start && k <= finish))
                yield return holiday!.Value;
    }

    public static DateOnly? GetHolidayByName(string name, int year)
    {
        var method = typeof(UsHolidays)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Select(m => new
            {
                Method = m,
                m.GetCustomAttribute<DescriptionAttribute>()?.Description
            })
            .FirstOrDefault(x =>
                x.Description is not null &&
                string.Equals(x.Description, name, StringComparison.OrdinalIgnoreCase));

        return method is null ? null : (DateOnly?)method.Method.Invoke(null, [year])!;
    }

    public static IReadOnlyDictionary<string, DateOnly?> GetHolidaysForYear(int year) =>
            typeof(UsHolidays)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Select(m => new
                {
                    Method = m,
                    m.GetCustomAttribute<DescriptionAttribute>()?.Description
                })
                .Where(x => x.Description != null
                            && x.Method.ReturnType == typeof(DateOnly?)
                            && x.Method.GetParameters().Length == 1
                            && x.Method.GetParameters()[0].ParameterType == typeof(int))
                .ToDictionary(
                    x => x.Description!,
                    x => (DateOnly?)x.Method.Invoke(null, [year])!
                );
}