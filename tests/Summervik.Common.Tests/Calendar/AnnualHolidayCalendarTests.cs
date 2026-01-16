using Summervik.Common.Calendar;

namespace Summervik.Common.Tests.Calendar;

public class AnnualHolidayCalendarTests
{
    [Fact]
    public void Constructor_WithYear_SetsStartAndEndCorrectly()
    {
        var calendar = new AnnualHolidayCalendar(2026);
        Assert.Equal(new DateOnly(2026, 1, 1), calendar.Start);
        Assert.Equal(2026, calendar.FiscalYear);
        Assert.Empty(calendar.Holidays);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_WithInvalidYear_ThrowsArgumentOutOfRange(int year)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new AnnualHolidayCalendar(year));
    }

    [Fact]
    public void Constructor_WithDateOnly_SetsStartAndEndCorrectly()
    {
        var start = new DateOnly(2026, 7, 1);
        var calendar = new AnnualHolidayCalendar(start);
        Assert.Equal(start, calendar.Start); 
        Assert.Equal(new DateOnly(2027, 6, 30), calendar.End);
        Assert.Equal(2027, calendar.FiscalYear);
        Assert.Empty(calendar.Holidays);
    }

    [Fact]
    public void FiscalYear_ReturnsEndYear()
    {
        var calendar = new AnnualHolidayCalendar(2026);
        Assert.Equal(2026, calendar.FiscalYear);
    }

    [Fact]
    public void Holidays_ReturnsSortedReadOnlyCollection()
    {
        var calendar = new AnnualHolidayCalendar(2026)
            .WithHoliday(new DateOnly(2026, 1, 1), "A", true)
            .WithHoliday(new DateOnly(2026, 12, 25), "B");

        var holidays = calendar.Holidays;
        Assert.Equal(2, holidays.Count);
        Assert.Equal("A", holidays.First().Name);
        Assert.Equal("B", holidays.Last().Name);
    }

    [Fact]
    public void GetHolidayByName_ReturnsMatchingHoliday_IgnoreCase()
    {
        var calendar = new AnnualHolidayCalendar(2026)
            .WithHoliday(new DateOnly(2026, 2, 14), "Valentine's");

        var result = calendar.GetHolidayByName("valentine's");
        Assert.NotNull(result);
        Assert.Equal("Valentine's", result.Value.Name);

        result = calendar.GetHolidayByName("NonExistent");
        Assert.Null(result);
    }

    [Fact]
    public void GetHolidayByName_ThrowsOnWhitespaceName()
    {
        var calendar = new AnnualHolidayCalendar(2026);
        Assert.Throws<ArgumentException>(() => calendar.GetHolidayByName(" "));
    }

    [Fact]
    public void GetHolidaysByDate_ReturnsHolidaysOnNominalOrObservedDate()
    {
        var calendar = new AnnualHolidayCalendar(2026)
            .WithHoliday(new DateOnly(2026, 7, 4), "Independence", true);

        var holidaysOnNominal = calendar.GetHolidaysByDate(new DateOnly(2026, 7, 4)).ToList();
        Assert.Single(holidaysOnNominal);
        Assert.Equal("Independence", holidaysOnNominal[0].Name);

        var holidaysOnObserved = calendar.GetHolidaysByDate(new DateOnly(2026, 7, 3)).ToList();
        Assert.Single(holidaysOnObserved);
        Assert.Equal("Independence", holidaysOnObserved[0].Name);

        var empty = calendar.GetHolidaysByDate(new DateOnly(2026, 7, 5)).ToList();
        Assert.Empty(empty);
    }

    [Fact]
    public void IsHoliday_ReturnsTrueForNominalOrObserved()
    {
        var calendar = new AnnualHolidayCalendar(2026)
            .WithHoliday(new DateOnly(2026, 7, 4), "Independence", true);

        Assert.True(calendar.IsHoliday(new DateOnly(2026, 7, 4))); // Nominal
        Assert.True(calendar.IsHoliday(new DateOnly(2026, 7, 3))); // Observed
        Assert.False(calendar.IsHoliday(new DateOnly(2026, 7, 5))); // Neither
    }

    [Fact]
    public void WithFederalUsHolidays_AddsFederalHolidays()
    {
        var calendar = new AnnualHolidayCalendar(2026).WithFederalUsHolidays();
        var holidays = calendar.Holidays;
        Assert.Contains(holidays, h => h.Name == UsHolidays.Names.IndependenceDay);
        Assert.DoesNotContain(holidays, h => h.Name == UsHolidays.Names.Valentines);
        Assert.Equal(UsHolidays.Names.GetFederalHolidays().Count(), holidays.Count);
    }

    [Fact]
    public void WithAllUsHolidays_AddsAllHolidays()
    {
        var calendar = new AnnualHolidayCalendar(2026).WithAllUsHolidays();
        var holidays = calendar.Holidays;
        Assert.Contains(holidays, h => h.Name == UsHolidays.Names.IndependenceDay);
        Assert.Contains(holidays, h => h.Name == UsHolidays.Names.Valentines);
        Assert.True(holidays.Count >= 15);
    }

    [Fact]
    public void WithHoliday_AddsNewHoliday()
    {
        var calendar = new AnnualHolidayCalendar(2026).WithHoliday(new DateOnly(2026, 12, 26), "Boxing Day", true);
        var holiday = calendar.GetHolidayByName("Boxing Day");
        Assert.NotNull(holiday);
        Assert.Equal("Boxing Day", holiday.Value.Name);
        Assert.True(holiday.Value.ObservesWeekendAdjustment);
        Assert.Single(calendar.Holidays);
    }

    [Fact]
    public void WithHoliday_ThrowsOnDateOutsideRange()
    {
        var calendar = new AnnualHolidayCalendar(2026);
        Assert.Throws<ArgumentOutOfRangeException>(() => calendar.WithHoliday(new DateOnly(2025, 12, 31), "Test"));
        Assert.Throws<ArgumentOutOfRangeException>(() => calendar.WithHoliday(new DateOnly(2027, 1, 1), "Test"));
    }

    [Fact]
    public void WithHoliday_FromHoliday_AddsCorrectly()
    {
        var existing = new Holiday("Test", new DateOnly(2026, 1, 1), true);
        var calendar = new AnnualHolidayCalendar(2026).WithHoliday(existing);
        var holiday = calendar.GetHolidayByName("Test");
        Assert.NotNull(holiday);
        Assert.True(holiday.Value.ObservesWeekendAdjustment);
    }

    [Fact]
    public void RemoveHoliday_RemovesByHoliday()
    {
        var holiday = new Holiday("Test", new DateOnly(2026, 1, 1), true);
        var calendar = new AnnualHolidayCalendar(2026).WithHoliday(holiday).RemoveHoliday(holiday);
        Assert.Empty(calendar.Holidays);
    }

    [Fact]
    public void RemoveHoliday_RemovesByDate()
    {
        var calendar = new AnnualHolidayCalendar(2026)
            .WithHoliday(new DateOnly(2026, 7, 4), "A")
            .WithHoliday(new DateOnly(2026, 7, 4), "B", true); // Same date, different name/flag

        calendar.RemoveHoliday(new DateOnly(2026, 7, 4));
        Assert.Empty(calendar.Holidays); // Removes both
    }

    [Fact]
    public void Chaining_Methods_ReturnsSameInstance()
    {
        var calendar = new AnnualHolidayCalendar(2026);
        var chained = calendar.WithFederalUsHolidays().WithHoliday(new DateOnly(2026, 12, 26), "Custom");
        Assert.Same(calendar, chained); // Chaining returns this
    }

    [Fact]
    public void GetHolidaysForYear_2026_ReturnsExpectedHolidays()
    {
        var holidays = UsHolidays.GetHolidaysForYear(2026).Where(h => h.HasValue).ToList();

        Assert.Equal(15, holidays.Count); // All except Inauguration Day

        // Check a fixed date with adjustment
        var independence = holidays.FirstOrDefault(h => h.GetValueOrDefault().Name == UsHolidays.Names.IndependenceDay);
        Assert.NotNull(independence);
        Assert.Equal(new DateOnly(2026, 7, 4), independence.Value.Date); // Nominal Sat
        Assert.Equal(new DateOnly(2026, 7, 3), independence.Value.ObservedDate); // Adjusted to Fri
        Assert.True(independence.Value.ObservesWeekendAdjustment);

        // Check a nth-weekday (no adjustment)
        var mlk = holidays.FirstOrDefault(h => h.GetValueOrDefault().Name == UsHolidays.Names.MartinLutherKingJr);
        Assert.NotNull(mlk);
        Assert.Equal(new DateOnly(2026, 1, 19), mlk.Value.Date);
        Assert.Equal(new DateOnly(2026, 1, 19), mlk.Value.ObservedDate);
        Assert.False(mlk.Value.ObservesWeekendAdjustment); // Assume false in code

        // Check cultural (no adjustment)
        var valentines = holidays.FirstOrDefault(h => h.GetValueOrDefault().Name == UsHolidays.Names.Valentines);
        Assert.NotNull(valentines);
        Assert.Equal(new DateOnly(2026, 2, 14), valentines.Value.Date);
        Assert.Equal(new DateOnly(2026, 2, 14), valentines.Value.ObservedDate);
        Assert.False(valentines.Value.ObservesWeekendAdjustment);
    }

    [Theory]
    [InlineData(2026, 1, DayOfWeek.Monday, 1, "2026-01-05")]
    [InlineData(2026, 1, DayOfWeek.Monday, 3, "2026-01-19")]
    [InlineData(2026, 2, DayOfWeek.Monday, 3, "2026-02-16")]
    [InlineData(2026, 8, DayOfWeek.Monday, 5, "2026-08-31")]
    [InlineData(2024, 2, DayOfWeek.Saturday, 4, "2024-02-24")]
    [InlineData(2026, 4, DayOfWeek.Friday, 4, "2026-04-24")]
    public void FindNthDayOfMonth_ValidInput_ReturnsCorrectDate(int year, int month, DayOfWeek dayOfWeek, int position, string expectedStr)
    {
        var expected = DateOnly.Parse(expectedStr);
        var result = UsHolidays.FindNthDayOfMonth(year, month, dayOfWeek, position);
        Assert.NotNull(result);
        Assert.Equal(expected, result.Value);
    }

    [Theory]
    [InlineData(2026, 2, DayOfWeek.Monday, 5)] 
    [InlineData(2026, 4, DayOfWeek.Monday, 5)] 
    [InlineData(2026, 6, DayOfWeek.Wednesday, 5)] 
    [InlineData(2023, 2, DayOfWeek.Saturday, 5)]
    public void FindNthDayOfMonth_InvalidPosition_ReturnsNull(int year, int month, DayOfWeek dayOfWeek, int position)
    {
        var result = UsHolidays.FindNthDayOfMonth(year, month, dayOfWeek, position);
        Assert.Null(result);
    }

    [Theory]
    [InlineData(0, 1, DayOfWeek.Monday, 1)] // Invalid year
    [InlineData(2026, 0, DayOfWeek.Monday, 1)] // Invalid month low
    [InlineData(2026, 13, DayOfWeek.Monday, 1)] // Invalid month high
    [InlineData(2026, 1, DayOfWeek.Monday, 0)] // Invalid position low
    [InlineData(2026, 1, DayOfWeek.Monday, 6)] // Invalid position high
    public void FindNthDayOfMonth_InvalidArguments_ThrowsArgumentOutOfRange(int year, int month, DayOfWeek dayOfWeek, int position)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => UsHolidays.FindNthDayOfMonth(year, month, dayOfWeek, position));
    }
}