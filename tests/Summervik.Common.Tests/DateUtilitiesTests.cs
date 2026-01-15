using Summervik.Common.Calendar;
using Summervik.Common.Extensions;
using System.Collections.Immutable;

namespace Summervik.Common.Tests;

public class DateUtilitiesTests
{
    [Fact]
    public void GetInclusiveDays_OneDate_IsInclusive()
    {
        var d1 = new DateOnly(2026, 1, 1);
        var d2 = new DateOnly(2026, 1, 1);
        var dates = DateUtilities.GetInclusiveDays(d1, d2).ToImmutableArray();
        Assert.Single(dates);
        Assert.Contains(d1, dates);
    }

    [Fact]
    public void GetInclusiveDays_TwoDates_IsInclusive()
    {
        var d1 = new DateOnly(2026, 1, 1);
        var d2 = new DateOnly(2026, 1, 2);
        var dates = DateUtilities.GetInclusiveDays(d1, d2).ToImmutableArray();
        Assert.Equal(2, dates.Length);
        Assert.Contains(d1, dates);
        Assert.Contains(d2, dates);
    }

    [Fact]
    public void GetInclusiveDays_TwoDates_OutOfOrder_IsInclusive()
    {
        var d1 = new DateOnly(2026, 1, 1);
        var d2 = new DateOnly(2026, 1, 2);
        var dates = DateUtilities.GetInclusiveDays(d2, d1).ToImmutableArray();
        Assert.Equal(2, dates.Length);
        Assert.Contains(d1, dates);
        Assert.Contains(d2, dates);
    }

    [Fact]
    public void GetInclusiveDays_CountDays_IsInclusive()
    {
        var d1 = new DateOnly(2026, 1, 1);
        var d2 = new DateOnly(2026, 1, 2);
        var count = DateUtilities.CountDays(d1, d2);
        Assert.Equal(2, count);
    }

    [Fact]
    public void CountWeekdays_FullWeek_Counts5()
    {
        var d1 = new DateOnly(2026, 1, 1);
        var d2 = d1.AddDays(6);
        var count = DateUtilities.CountWeekdays(d1, d2);
        Assert.Equal(5, count);
    }

    [Fact]
    public void AdjustWeekendDayToNearestWeekday_SatGoesToFri()
    {
        var sat = new DateOnly(2026, 1, 3);
        Assert.True(sat.DayOfWeek is DayOfWeek.Saturday);
        var fri = DateUtilities.AdjustToObservedWeekday(sat);
        Assert.True(fri.DayOfWeek is DayOfWeek.Friday);
    }

    [Fact]
    public void AdjustWeekendDayToNearestWeekday_SunToMon()
    {
        var sun = new DateOnly(2026, 1, 4);
        Assert.True(sun.DayOfWeek is DayOfWeek.Sunday);
        var mon = DateUtilities.AdjustToObservedWeekday(sun);
        Assert.True(mon.DayOfWeek is DayOfWeek.Monday);
    }

    [Fact]
    public void GetWeekDays_CountsAllWeekdays()
    {
        var christmas = UsHolidays.GetHolidayDateByName(UsHolidays.Names.Christmas, 2025);
        Assert.True(christmas.HasValue);
        var newYears = UsHolidays.GetHolidayDateByName(UsHolidays.Names.NewYears, 2026);
        Assert.True(newYears.HasValue);
        var actual = DateUtilities.GetWeekdays(christmas.Value, newYears.Value).ToImmutableArray();
        int expected = 6;
        Assert.Equal(expected, actual.Length);
    }

    [Fact]
    public void ToStartOfDay_MovesToTimeOnlyMinValue()
    {
        var dt = DateTime.Now;
        while (TimeOnly.FromDateTime(dt) == TimeOnly.MinValue)
            dt = DateTime.Now;

        var dt2 = dt.ToStartOfDay();
        Assert.Equal(TimeOnly.MinValue, TimeOnly.FromDateTime(dt2));
    }

    [Fact]
    public void ToStartOfDay_KeepsKind()
    {
        var dt = DateTime.UtcNow;
        while (TimeOnly.FromDateTime(dt) == TimeOnly.MinValue)
            dt = DateTime.UtcNow;

        var dt2 = dt.ToStartOfDay();
        Assert.Equal(dt.Kind, dt2.Kind);
    }

    [Fact]
    public void ToEndOfDay_MovesToTimeOnlyMaxValue()
    {
        var dt = DateTime.Now;
        while (TimeOnly.FromDateTime(dt) == TimeOnly.MaxValue)
            dt = DateTime.Now;

        var dt2 = dt.ToEndOfDay();
        Assert.Equal(TimeOnly.MaxValue, TimeOnly.FromDateTime(dt2));
    }

    [Fact]
    public void ToEndOfDay_KeepsKind()
    {
        var dt = DateTime.UtcNow;
        while (TimeOnly.FromDateTime(dt) == TimeOnly.MaxValue)
            dt = DateTime.UtcNow;

        var dt2 = dt.ToEndOfDay();
        Assert.Equal(dt.Kind, dt2.Kind);
    }

    [Fact]
    public void DateTime_Add0Weekdays_SameDate()
    {
        var dt = DateTime.UtcNow;
        while (dt.DayOfWeek != DayOfWeek.Monday)
            dt = dt.AddDays(1);

        var dt2 = dt.AddWeekdays(0);
        Assert.Equal(dt, dt2);
    }

    [Fact]
    public void DateTime_Add1Weekday_NextDay()
    {
        var dt = DateTime.UtcNow;
        while (dt.DayOfWeek != DayOfWeek.Monday)
            dt = dt.AddDays(1);

        var dt2 = dt.AddWeekdays(1);
        Assert.Equal(dt.AddDays(1), dt2);
    }

    [Fact]
    public void DateTime_AddMinus1Weekday_NextDay()
    {
        var dt = DateTime.UtcNow;
        while (dt.DayOfWeek != DayOfWeek.Tuesday)
            dt = dt.AddDays(1);

        var dt2 = dt.AddWeekdays(-1);
        Assert.Equal(dt.AddDays(-1), dt2);
    }

    [Fact]
    public void DateTime_SkipWeekend_FridayToMonday()
    {
        var dt = DateTime.UtcNow;
        while (dt.DayOfWeek != DayOfWeek.Friday)
            dt = dt.AddDays(1);

        var dt2 = dt.AddWeekdays(1);
        Assert.Equal(dt.AddDays(3), dt2);
        Assert.Equal(DayOfWeek.Monday, dt2.DayOfWeek);
    }

    [Fact]
    public void DateTime_SkipWeekendBackwards_MondayToFriday()
    {
        var dt = DateTime.UtcNow;
        while (dt.DayOfWeek != DayOfWeek.Monday)
            dt = dt.AddDays(1);

        var dt2 = dt.AddWeekdays(-1);
        Assert.Equal(dt.AddDays(-3), dt2);
        Assert.Equal(DayOfWeek.Friday, dt2.DayOfWeek);
    }
}
