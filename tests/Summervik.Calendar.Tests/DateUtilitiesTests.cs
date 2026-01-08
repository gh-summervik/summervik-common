using System.Collections.Immutable;

namespace Summervik.Calendar.Tests;

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
        var count = DateUtilities.CountWeekDays(d1, d2);
        Assert.Equal(5, count);
    }

    [Fact]
    public void AdjustWeekendDayToNearestWeekday_SatGoesToFri()
    {
        var sat = new DateOnly(2026, 1, 3);
        Assert.True(sat.DayOfWeek is DayOfWeek.Saturday);
        var fri = DateUtilities.AdjustWeekendDayToNearestWeekday(sat);
        Assert.True(fri.DayOfWeek is DayOfWeek.Friday);
    }

    [Fact]
    public void AdjustWeekendDayToNearestWeekday_SunToMon()
    {
        var sun = new DateOnly(2026, 1, 4);
        Assert.True(sun.DayOfWeek is DayOfWeek.Sunday);
        var mon = DateUtilities.AdjustWeekendDayToNearestWeekday(sun);
        Assert.True(mon.DayOfWeek is DayOfWeek.Monday);
    }

    [Fact]
    public void GetWeekDays_CountsAllWeekdays()
    {
        var christmas = UsHolidays.GetHolidayByName(UsHolidays.Names.Christmas, 2025);
        Assert.True(christmas.HasValue);
        var newYears = UsHolidays.GetHolidayByName(UsHolidays.Names.NewYears, 2026);
        Assert.True(newYears.HasValue);
        var actual = DateUtilities.GetWeekDays(christmas.Value, newYears.Value).ToImmutableArray();
        int expected = 6;
        Assert.Equal(expected, actual.Length);
    }
}
