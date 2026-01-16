using Summervik.Common.Extensions;

namespace Summervik.Common.Tests.Extensions;

public class DateOnlyExtensionTests
{
    [Fact]
    public void DateOnly_Add0Weekdays_SameDate()
    {
        var dt = DateOnly.FromDateTime(DateTime.UtcNow);
        while (dt.DayOfWeek != DayOfWeek.Monday)
            dt = dt.AddDays(1);

        var dt2 = dt.AddWeekdays(0);
        Assert.Equal(dt, dt2);
    }

    [Fact]
    public void DateOnly_Add1Weekday_NextDay()
    {
        var dt = DateOnly.FromDateTime(DateTime.UtcNow);
        while (dt.DayOfWeek != DayOfWeek.Monday)
            dt = dt.AddDays(1);

        var dt2 = dt.AddWeekdays(1);
        Assert.Equal(dt.AddDays(1), dt2);
    }

    [Fact]
    public void DateOnly_AddMinus1Weekday_NextDay()
    {
        var dt = DateOnly.FromDateTime(DateTime.UtcNow);
        while (dt.DayOfWeek != DayOfWeek.Tuesday)
            dt = dt.AddDays(1);

        var dt2 = dt.AddWeekdays(-1);
        Assert.Equal(dt.AddDays(-1), dt2);
    }

    [Fact]
    public void DateOnly_SkipWeekend_FridayToMonday()
    {
        var dt = DateOnly.FromDateTime(DateTime.UtcNow);
        while (dt.DayOfWeek != DayOfWeek.Friday)
            dt = dt.AddDays(1);

        var dt2 = dt.AddWeekdays(1);
        Assert.Equal(dt.AddDays(3), dt2);
        Assert.Equal(DayOfWeek.Monday, dt2.DayOfWeek);
    }

    [Fact]
    public void DateOnly_SkipWeekendBackwards_MondayToFriday()
    {
        var dt = DateOnly.FromDateTime(DateTime.UtcNow);
        while (dt.DayOfWeek != DayOfWeek.Monday)
            dt = dt.AddDays(1);

        var dt2 = dt.AddWeekdays(-1);
        Assert.Equal(dt.AddDays(-3), dt2);
        Assert.Equal(DayOfWeek.Friday, dt2.DayOfWeek);
    }
}
