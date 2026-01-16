using Summervik.Common.Extensions;

namespace Summervik.Common.Tests.Extensions;

public class DateTimeExtensionTests
{
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
