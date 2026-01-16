using System.ComponentModel;
using System.Reflection;

namespace Summervik.Common.Calendar;

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
        public static IEnumerable<string> GetAll() => typeof(Names)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string))
                .Select(f => (string)f.GetRawConstantValue()!);

        /// <summary>
        /// Gets all US Federal holidays supported by this class.
        /// </summary>
        /// <remarks>
        /// I excluded Inauguration Day here because it is seldom observed by businesses.
        /// </remarks>
        /// <returns></returns>
        public static IEnumerable<string> GetFederalHolidays() => GetAll()
            .Where(k => k is NewYears or MartinLutherKingJr or PresidentsDay or
                MemorialDay or IndependenceDay or LaborDay or ColumbusDay or VeteransDay or
                Thanksgiving or Christmas);
    }

    [Description(Names.NewYears)]
    public static DateOnly? NewYearsDayDate(int year) => year < 1
        ? throw new ArgumentOutOfRangeException(nameof(year))
        : new(year, 1, 1);

    [Description(Names.NewYears)]
    public static Holiday? NewYearsDay(int year) => year < 1
        ? throw new ArgumentOutOfRangeException(nameof(year))
        : new Holiday(Names.NewYears, new DateOnly(year, 1, 1), true);

    [Description(Names.MartinLutherKingJr)]
    public static DateOnly? MartinLutherKingJrBirthdayDate(int year) => year < 1
        ? throw new ArgumentOutOfRangeException(nameof(year))
        : year < 1986 ? null : FindNthDayOfMonth(year, 1, DayOfWeek.Monday, 3);

    [Description(Names.MartinLutherKingJr)]
    public static Holiday? MartinLutherKingJrBirthday(int year)
    {
        var dt = MartinLutherKingJrBirthdayDate(year);
        return !dt.HasValue ? null : new Holiday()
        {
            Name = Names.MartinLutherKingJr,
            Date = dt.Value
        };
    }

    [Description(Names.InaugurationDay)]
    public static DateOnly? InaugurationDayDate(int year)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(year, 1);
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

        /*
         * This one doesn't use the normal observation rules.
         * It only moves to Monday when it occurs on a Sunday.
         * Saturdays are okay.
         */
        return dt is null ? null
            : dt.Value.DayOfWeek is DayOfWeek.Sunday ? dt.Value.AddDays(1) : dt;
    }

    [Description(Names.InaugurationDay)]
    public static Holiday? InaugurationDay(int year)
    {
        var dt = InaugurationDayDate(year);
        return !dt.HasValue ? null : new Holiday()
        {
            Name = Names.InaugurationDay,
            Date = dt.Value
        };
    }

    [Description(Names.PresidentsDay)]
    public static DateOnly? PresidentsDayDate(int year) => year < 1
        ? throw new ArgumentOutOfRangeException(nameof(year))
        : year < 1885 ? null : FindNthDayOfMonth(year, 2, DayOfWeek.Monday, 3);

    [Description(Names.PresidentsDay)]
    public static Holiday? PresidentsDay(int year)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(year, 1);

        var dt = PresidentsDayDate(year);
        return !dt.HasValue ? null : new Holiday()
        {
            Name = Names.PresidentsDay,
            Date = dt.Value
        };
    }

    /*
     * The true origin of this holiday in the U.S. is fuzzy.
     */
    [Description(Names.Valentines)]
    public static DateOnly? ValentinesDayDate(int year) => year < 1
        ? throw new ArgumentOutOfRangeException(nameof(year))
        : new(year, 2, 14);

    [Description(Names.Valentines)]
    public static Holiday? ValentinesDay(int year) => new Holiday(
        Name: Names.Valentines,
        Date: new(year, 2, 14)
    );

    [Description(Names.Easter)]
    public static DateOnly? EasterSundayDate(int year)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(year, 1);

        int a = year % 19;
        int b = year / 100;
        int c = year % 100;
        int d = b / 4;
        int e = b % 4;
        int f = (b + 8) / 25;
        int g = (b - f + 1) / 3;
        int h = (19 * a + b - d - g + 15) % 30;
        int i = c / 4;
        int k = c % 4;
        int l = (32 + 2 * e + 2 * i - h - k) % 7;
        int m = (a + 11 * h + 22 * l) / 451;
        int month = (h + l - 7 * m + 114) / 31; // 3 = March, 4 = April
        int day = ((h + l - 7 * m + 114) % 31) + 1;

        return new DateOnly(year, month, day);
    }

    [Description(Names.Easter)]
    public static Holiday? EasterSunday(int year)
    {
        var dt = EasterSundayDate(year);
        return !dt.HasValue ? null
            : new Holiday()
            {
                Name = Names.Easter,
                Date = dt.Value
            };
    }

    [Description(Names.MemorialDay)]
    public static DateOnly? MemorialDayDate(int year)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(year, 1);

        if (year < 1868)
            return null;

        var memorialDay = new DateOnly(year, 5, 31);
        while (memorialDay.DayOfWeek != DayOfWeek.Monday)
        {
            memorialDay = memorialDay.AddDays(-1);
        }
        return memorialDay;
    }

    [Description(Names.MemorialDay)]
    public static Holiday? MemorialDay(int year)
    {
        var dt = MemorialDayDate(year);
        return !dt.HasValue ? null
            : new Holiday()
            {
                Date = dt.Value,
                Name = Names.MemorialDay
            };
    }

    [Description(Names.Juneteenth)]
    public static DateOnly? JuneteenthDate(int year) => year < 1
        ? throw new ArgumentOutOfRangeException(nameof(year)) : year < 2021 ? null : new(year, 6, 19);

    [Description(Names.Juneteenth)]
    public static Holiday? Juneteenth(int year)
    {
        var dt = JuneteenthDate(year);
        return !dt.HasValue ? null : new Holiday()
        {
            Date = dt.Value,
            Name = Names.Juneteenth,
            ObservesWeekendAdjustment = true
        };
    }

    [Description(Names.IndependenceDay)]
    public static DateOnly? IndependenceDayDate(int year) => year < 1
        ? throw new ArgumentOutOfRangeException(nameof(year))
        : year < 1870 ? null : new(year, 7, 4);

    [Description(Names.IndependenceDay)]
    public static Holiday? IndependenceDay(int year)
    {
        var dt = IndependenceDayDate(year);
        return !dt.HasValue ? null : new Holiday()
        {
            Date = dt.Value,
            Name = Names.IndependenceDay,
            ObservesWeekendAdjustment = true
        };
    }

    [Description(Names.LaborDay)]
    public static DateOnly? LaborDayDate(int year)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(year, 1);
        if (year < 1887)
            return null;
        var laborDay = new DateOnly(year, 9, 1);
        while (laborDay.DayOfWeek is not DayOfWeek.Monday)
            laborDay = laborDay.AddDays(1);
        return laborDay;
    }

    [Description(Names.LaborDay)]
    public static Holiday? LaborDay(int year)
    {
        var dt = LaborDayDate(year);
        return !dt.HasValue ? null : new Holiday()
        {
            Date = dt.Value,
            Name = Names.LaborDay
        };
    }

    [Description(Names.ColumbusDay)]
    public static DateOnly? ColumbusDayDate(int year) => year < 1
        ? throw new ArgumentOutOfRangeException(nameof(year))
        : year < 1937 ? null : FindNthDayOfMonth(year, 10, DayOfWeek.Monday, 2);

    [Description(Names.ColumbusDay)]
    public static Holiday? ColumbusDay(int year)
    {
        var dt = ColumbusDayDate(year);
        return !dt.HasValue ? null : new Holiday()
        {
            Date = dt.Value,
            Name = Names.ColumbusDay
        };
    }

    [Description(Names.VeteransDay)]
    public static DateOnly? VeteransDayDate(int year) => year < 1
        ? throw new ArgumentOutOfRangeException(nameof(year)) : year < 1938 ? null : new(year, 11, 11);

    [Description(Names.VeteransDay)]
    public static Holiday? VeteransDay(int year)
    {
        var dt = VeteransDayDate(year);
        return !dt.HasValue ? null :
            new Holiday()
            {
                Date = dt.Value,
                Name = Names.VeteransDay,
                ObservesWeekendAdjustment = true
            };
    }

    /*
     * The origin of Thanksgiving is sometimes attributed to Lincoln (with reason),
     * but FDR and Congress made it a thing in 1941.
     */
    [Description(Names.Thanksgiving)]
    public static DateOnly? ThanksgivingDayDate(int year) => year < 1
        ? throw new ArgumentOutOfRangeException(nameof(year)) : year < 1941 ? null :
        FindNthDayOfMonth(year, 11, DayOfWeek.Thursday, 4);

    [Description(Names.Thanksgiving)]
    public static Holiday? ThanksgivingDay(int year)
    {
        var dt = ThanksgivingDayDate(year);
        return !dt.HasValue ? null : new Holiday()
        {
            Date = dt.Value,
            Name = Names.Thanksgiving
        };
    }

    [Description(Names.ChristmasEve)]
    public static DateOnly? ChristmasEveDayDate(int year) => year < 1
        ? throw new ArgumentOutOfRangeException(nameof(year)) : year < 1870 ? null : ChristmasDayDate(year)?.AddDays(-1);

    [Description(Names.ChristmasEve)]
    public static Holiday? ChristmasEveDay(int year)
    {
        var dt = ChristmasEveDayDate(year);
        return !dt.HasValue ? null : new Holiday()
        {
            Date = dt.Value,
            Name = Names.ChristmasEve
        };
    }

    [Description(Names.Christmas)]
    public static DateOnly? ChristmasDayDate(int year) => year < 1
        ? throw new ArgumentOutOfRangeException(nameof(year)) : year < 1870 ? null : new(year, 12, 25);

    [Description(Names.Christmas)]
    public static Holiday? ChristmasDay(int year)
    {
        var dt = ChristmasDayDate(year);
        return !dt.HasValue ? null : new Holiday()
        {
            Date = dt.Value,
            Name = Names.Christmas,
            ObservesWeekendAdjustment = true
        };
    }

    [Description(Names.NewYearsEve)]
    public static DateOnly? NewYearsEveDayDate(int year) => year < 1
        ? throw new ArgumentOutOfRangeException(nameof(year)) : year < 1870 ? null : new(year, 12, 31);

    [Description(Names.NewYearsEve)]
    public static Holiday? NewYearsEveDay(int year)
    {
        var dt = NewYearsEveDayDate(year);
        return !dt.HasValue ? null : new Holiday()
        {
            Date = dt.Value,
            Name = Names.NewYearsEve
        };
    }

    public static string? GetNameForHoliday(DateOnly date)
    {
        var holidaysForYear = GetHolidaysForYear(date.Year);

        var item = holidaysForYear.FirstOrDefault(h => h.HasValue && h.Value.Date.Equals(date));

        return item.Equals(default) ? null : item.Value.Name;
    }

    public static DateOnly GetObservedDate(DateOnly date) => DateUtilities.MoveToNearestWeekday(date);

    public static IEnumerable<DateOnly> GetWeekDaysExcludingHolidays(DateOnly start, DateOnly finish) =>
        DateUtilities.GetWeekdays(start, finish)
            .Except(GetInclusiveHolidayDatesBetweenDates(start, finish)
            .Where(h => h.DayOfWeek is not DayOfWeek.Saturday and not DayOfWeek.Sunday));

    public static int CountWeekDaysExcludingHolidays(DateOnly start, DateOnly finish) => GetWeekDaysExcludingHolidays(start, finish).Count();

    public static IEnumerable<DateOnly> GetInclusiveHolidayDatesBetweenDates(DateOnly start, DateOnly finish)
    {
        if (finish < start)
            (start, finish) = (finish, start);

        foreach (int year in Enumerable.Range(start.Year, (finish.Year - start.Year) + 1))
            foreach (var holiday in GetHolidaysForYear(year).Where(k => k.HasValue && k.Value.ObservedDate >= start && k.Value.ObservedDate <= finish))
                yield return holiday!.Value.Date;
    }

    public static IEnumerable<Holiday> GetInclusiveHolidaysBetweenDates(DateOnly start, DateOnly end)
    {
        if (end < start)
            (start, end) = (end, start);

        foreach (int year in Enumerable.Range(start.Year, (end.Year - start.Year) + 1))
            foreach (var holiday in GetHolidaysForYear(year).Where(k => k.HasValue && k.Value.ObservedDate >= start && k.Value.ObservedDate <= end))
                yield return holiday!.Value;
    }

    public static DateOnly? GetHolidayDateByName(string name, int year)
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
                x.Method.ReturnType == typeof(DateOnly?) &&
                string.Equals(x.Description, name, StringComparison.OrdinalIgnoreCase));

        return method is null ? null : (DateOnly?)method.Method.Invoke(null, [year])!;
    }

    public static Holiday? GetHolidayByName(string name, int year)
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
                x.Method.ReturnType == typeof(Holiday?) &&
                string.Equals(x.Description, name, StringComparison.OrdinalIgnoreCase));

        return method is null ? null : (Holiday?)method.Method.Invoke(null, [year])!;
    }

    public static IEnumerable<Holiday?> GetHolidaysForYear(int year) =>
            typeof(UsHolidays).GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Select(m => new
                {
                    Method = m,
                    m.GetCustomAttribute<DescriptionAttribute>()?.Description
                })
                .Where(x => x.Description != null
                            && x.Method.ReturnType == typeof(Holiday?)
                            && x.Method.GetParameters().Length == 1
                            && x.Method.GetParameters()[0].ParameterType == typeof(int))
                .Select(k => (Holiday?)k.Method.Invoke(null, [year]))
                .Where(k => k is not null)
                .OrderBy(k => k.GetValueOrDefault().Date);

    /// <summary>
    /// Finds the Nth position of a day (identified by the day of week) in a month.
    /// </summary>
    public static DateOnly? FindNthDayOfMonth(int year, int month, DayOfWeek dayOfWeek, int position)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(year, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(month, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(month, 12);
        ArgumentOutOfRangeException.ThrowIfLessThan(position, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(position, 5);

        try
        {
            int lastDayOfMonth;
            if (DateTime.IsLeapYear(year) && month == 2)
                lastDayOfMonth = 29;
            else if (month == 2)
                lastDayOfMonth = 28;
            else if (month is 4 or 6 or 9 or 11)
                lastDayOfMonth = 30;
            else
                lastDayOfMonth = 31;

            var day = Enumerable.Range(1, lastDayOfMonth)
                .Where(d => new DateOnly(year, month, d).DayOfWeek == dayOfWeek)
                .ElementAt(position - 1);

            return new DateOnly(year, month, day);
        }
        catch
        {
            return null;
        }
    }
}
