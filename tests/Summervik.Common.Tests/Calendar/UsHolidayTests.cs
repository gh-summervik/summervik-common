using Summervik.Common.Calendar;
using Summervik.Common.Extensions;
using System.Collections.Immutable;

namespace Summervik.Common.Tests.Calendar;

public class UsHolidayTests
{
    [Fact]
    public void HolidayNames_Sixteen()
    {
        var names = UsHolidays.Names.GetAll().ToImmutableArray();
        Assert.Equal(16, names.Length);
        var hs = new HashSet<string>(names); // uniqueness
        Assert.Equal(16, hs.Count);
    }

    [Fact]
    public void NewYearsDay_2026_Jan1()
    {
        var nyd = UsHolidays.NewYearsDayDate(2026);
        var expected = new DateOnly(2026, 1, 1);
        Assert.Equal(expected, nyd);
    }

    [Fact]
    public void NewYearsDay_2028_CelebratedDec31()
    {
        var nyd = UsHolidays.NewYearsDayDate(2028).GetValueOrDefault().MoveToNearestWeekday();
        var expected = new DateOnly(2027, 12, 31);
        Assert.Equal(expected, nyd);
    }

    [Theory]
    [InlineData(1985, null)]
    [InlineData(2026, "2026-01-19")]
    public void MlkJr_2026_Jan17(int year, string? expected)
    {
        DateOnly? exp = expected is null ? null : DateOnly.Parse(expected);
        Assert.Equal(UsHolidays.MartinLutherKingJrBirthdayDate(year), exp);
    }

    [Theory]
    [InlineData(1789, "1789-04-30")]
    [InlineData(1790, null)]
    [InlineData(1793, "1793-03-04")]
    [InlineData(1933, "1933-03-04")]
    [InlineData(1937, "1937-01-20")]
    [InlineData(2025, "2025-01-20")]
    [InlineData(2026, null)]
    [InlineData(2027, null)]
    [InlineData(2028, null)]
    [InlineData(1821, "1821-03-05")]
    [InlineData(1849, "1849-03-05")]
    [InlineData(1877, "1877-03-05")]
    [InlineData(1917, "1917-03-05")]

    public void InaugInaugurationDay(int year, string? expected)
    {
        DateOnly? exp = expected is null ? null : DateOnly.Parse(expected);
        Assert.Equal(UsHolidays.InaugurationDayDate(year), exp);
    }

    [Theory]
    [InlineData(1884, null)]
    [InlineData(2026, "2026-02-16")]
    public void PresidentsDay(int year, string? expected)
    {
        DateOnly? exp = expected is null ? null : DateOnly.Parse(expected);
        Assert.Equal(UsHolidays.PresidentsDayDate(year), exp);
    }

    [Theory]
    [InlineData(2026, "2026-02-14")]
    public void ValentinesDay(int year, string? expected)
    {
        DateOnly? exp = expected is null ? null : DateOnly.Parse(expected);
        Assert.Equal(UsHolidays.ValentinesDayDate(year), exp);
    }

    [Theory]
    [InlineData(2025, "2025-04-20")]
    [InlineData(2026, "2026-04-05")]
    public void Easter(int year, string? expected)
    {
        DateOnly? exp = expected is null ? null : DateOnly.Parse(expected);
        Assert.Equal(UsHolidays.EasterSundayDate(year), exp);
    }

    [Theory]
    [InlineData(2025, "2025-05-26")]
    [InlineData(2026, "2026-05-25")]
    [InlineData(1867, null)]
    public void MemorialDay(int year, string? expected)
    {
        DateOnly? exp = expected is null ? null : DateOnly.Parse(expected);
        Assert.Equal(UsHolidays.MemorialDayDate(year), exp);
    }

    [Theory]
    [InlineData(2020, null)]
    [InlineData(2026, "2026-06-19")]
    public void Juneteenth(int year, string? expected)
    {
        DateOnly? exp = expected is null ? null : DateOnly.Parse(expected);
        Assert.Equal(UsHolidays.JuneteenthDate(year), exp);
    }

    [Fact]
    public void Juneteenth_2027_CelebratedJune18()
    {
        DateOnly exp = new(2027, 6, 18);
        Assert.Equal(exp, UsHolidays.JuneteenthDate(2027).GetValueOrDefault().MoveToNearestWeekday());
    }

    [Theory]
    [InlineData(1869, null)]
    [InlineData(2026, "2026-07-04")]
    public void IndependenceDay(int year, string? expected)
    {
        DateOnly? exp = expected is null ? null : DateOnly.Parse(expected);
        Assert.Equal(UsHolidays.IndependenceDayDate(year), exp);
    }

    [Fact]
    public void IndependenceDay_2027_CelebratedJuly5()
    {
        DateOnly exp = new(2027, 7, 5);
        Assert.Equal(exp, UsHolidays.IndependenceDayDate(2027).GetValueOrDefault().MoveToNearestWeekday());
    }

    [Theory]
    [InlineData(1886, null)]
    [InlineData(2026, "2026-09-07")]
    public void LaborDay(int year, string? expected)
    {
        DateOnly? exp = expected is null ? null : DateOnly.Parse(expected);
        Assert.Equal(UsHolidays.LaborDayDate(year), exp);
    }

    [Theory]
    [InlineData(1936, null)]
    [InlineData(2026, "2026-10-12")]
    public void ColumbusDay(int year, string? expected)
    {
        DateOnly? exp = expected is null ? null : DateOnly.Parse(expected);
        Assert.Equal(UsHolidays.ColumbusDayDate(year), exp);
    }

    [Theory]
    [InlineData(1937, null)]
    [InlineData(2026, "2026-11-11")]
    public void VeteransDay(int year, string? expected)
    {
        DateOnly? exp = expected is null ? null : DateOnly.Parse(expected);
        Assert.Equal(UsHolidays.VeteransDayDate(year), exp);
    }

    [Theory]
    [InlineData(1940, null)]
    [InlineData(2026, "2026-11-26")]
    public void ThanksgivingDay(int year, string? expected)
    {
        DateOnly? exp = expected is null ? null : DateOnly.Parse(expected);
        Assert.Equal(UsHolidays.ThanksgivingDayDate(year), exp);
    }

    [Theory]
    [InlineData(1869, null)]
    [InlineData(2026, "2026-12-24")]
    public void ChristmasEveDay(int year, string? expected)
    {
        DateOnly? exp = expected is null ? null : DateOnly.Parse(expected);
        Assert.Equal(UsHolidays.ChristmasEveDayDate(year), exp);
    }

    [Theory]
    [InlineData(1869, null)]
    [InlineData(2026, "2026-12-25")]
    public void ChristmasDay(int year, string? expected)
    {
        DateOnly? exp = expected is null ? null : DateOnly.Parse(expected);
        Assert.Equal(UsHolidays.ChristmasDayDate(year), exp);
    }

    [Theory]
    [InlineData(2026, "2026-12-31")]
    public void NewYearsEveDay(int year, string? expected)
    {
        DateOnly? exp = expected is null ? null : DateOnly.Parse(expected);
        Assert.Equal(UsHolidays.NewYearsEveDayDate(year), exp);
    }

    [Fact]
    public void ChristmasDay_2027_CelebratedDec24()
    {
        DateOnly exp = new(2027, 12, 24);
        Assert.Equal(exp, UsHolidays.ChristmasDayDate(2027).GetValueOrDefault().MoveToNearestWeekday());
    }

    [Fact]
    public void GetHolidayDateByName_2026()
    {
        var nyd = new DateOnly(2026, 1, 1);
        var mlk = new DateOnly(2026, 1, 19);
        var vald = new DateOnly(2026, 2, 14);
        var pd = new DateOnly(2026, 2, 16);
        var md = new DateOnly(2026, 5, 25);
        var jt = new DateOnly(2026, 6, 19);
        var id = new DateOnly(2026, 7, 4);
        var ld = new DateOnly(2026, 9, 7);
        var cold = new DateOnly(2026, 10, 12);
        var vd = new DateOnly(2026, 11, 11);
        var td = new DateOnly(2026, 11, 26);
        var cd = new DateOnly(2026, 12, 25);
        var nye = new DateOnly(2026, 12, 31);

        Assert.Equal(nyd, UsHolidays.GetHolidayDateByName(UsHolidays.Names.NewYears, 2026).GetValueOrDefault());
        Assert.Equal(mlk, UsHolidays.GetHolidayDateByName(UsHolidays.Names.MartinLutherKingJr, 2026).GetValueOrDefault());
        Assert.Equal(vald, UsHolidays.GetHolidayDateByName(UsHolidays.Names.Valentines, 2026).GetValueOrDefault());
        Assert.Equal(pd, UsHolidays.GetHolidayDateByName(UsHolidays.Names.PresidentsDay, 2026).GetValueOrDefault());
        Assert.Equal(md, UsHolidays.GetHolidayDateByName(UsHolidays.Names.MemorialDay, 2026).GetValueOrDefault());
        Assert.Equal(jt, UsHolidays.GetHolidayDateByName(UsHolidays.Names.Juneteenth, 2026).GetValueOrDefault());
        Assert.Equal(id, UsHolidays.GetHolidayDateByName(UsHolidays.Names.IndependenceDay, 2026).GetValueOrDefault());
        Assert.Equal(ld, UsHolidays.GetHolidayDateByName(UsHolidays.Names.LaborDay, 2026).GetValueOrDefault());
        Assert.Equal(cold, UsHolidays.GetHolidayDateByName(UsHolidays.Names.ColumbusDay, 2026).GetValueOrDefault());
        Assert.Equal(vd, UsHolidays.GetHolidayDateByName(UsHolidays.Names.VeteransDay, 2026).GetValueOrDefault());
        Assert.Equal(td, UsHolidays.GetHolidayDateByName(UsHolidays.Names.Thanksgiving, 2026).GetValueOrDefault());
        Assert.Equal(cd, UsHolidays.GetHolidayDateByName(UsHolidays.Names.Christmas, 2026).GetValueOrDefault());
        Assert.Equal(nye, UsHolidays.GetHolidayDateByName(UsHolidays.Names.NewYearsEve, 2026).GetValueOrDefault());
    }

    [Fact]
    public void GetHolidayByName_2026()
    {
        var newYears = UsHolidays.GetHolidayByName(UsHolidays.Names.NewYears, 2026);
        Assert.NotNull(newYears);
        Assert.Equal(UsHolidays.Names.NewYears, newYears.Value.Name);
        Assert.Equal(new DateOnly(2026, 1, 1), newYears.Value.Date);
        Assert.Equal(new DateOnly(2026, 1, 1), newYears.Value.ObservedDate);
        Assert.True(newYears.Value.ObservesWeekendAdjustment);

        var mlk = UsHolidays.GetHolidayByName(UsHolidays.Names.MartinLutherKingJr, 2026);
        Assert.NotNull(mlk);
        Assert.Equal(UsHolidays.Names.MartinLutherKingJr, mlk.Value.Name);
        Assert.Equal(new DateOnly(2026, 1, 19), mlk.Value.Date);
        Assert.Equal(new DateOnly(2026, 1, 19), mlk.Value.ObservedDate);
        Assert.False(mlk.Value.ObservesWeekendAdjustment);

        var valentines = UsHolidays.GetHolidayByName(UsHolidays.Names.Valentines, 2026);
        Assert.NotNull(valentines);
        Assert.Equal(UsHolidays.Names.Valentines, valentines.Value.Name);
        Assert.Equal(new DateOnly(2026, 2, 14), valentines.Value.Date);
        Assert.Equal(new DateOnly(2026, 2, 14), valentines.Value.ObservedDate);
        Assert.False(valentines.Value.ObservesWeekendAdjustment);

        var presidents = UsHolidays.GetHolidayByName(UsHolidays.Names.PresidentsDay, 2026);
        Assert.NotNull(presidents);
        Assert.Equal(UsHolidays.Names.PresidentsDay, presidents.Value.Name);
        Assert.Equal(new DateOnly(2026, 2, 16), presidents.Value.Date);
        Assert.Equal(new DateOnly(2026, 2, 16), presidents.Value.ObservedDate);
        Assert.False(presidents.Value.ObservesWeekendAdjustment);

        var memorial = UsHolidays.GetHolidayByName(UsHolidays.Names.MemorialDay, 2026);
        Assert.NotNull(memorial);
        Assert.Equal(UsHolidays.Names.MemorialDay, memorial.Value.Name);
        Assert.Equal(new DateOnly(2026, 5, 25), memorial.Value.Date);
        Assert.Equal(new DateOnly(2026, 5, 25), memorial.Value.ObservedDate);
        Assert.False(memorial.Value.ObservesWeekendAdjustment);

        var juneteenth = UsHolidays.GetHolidayByName(UsHolidays.Names.Juneteenth, 2026);
        Assert.NotNull(juneteenth);
        Assert.Equal(UsHolidays.Names.Juneteenth, juneteenth.Value.Name);
        Assert.Equal(new DateOnly(2026, 6, 19), juneteenth.Value.Date);
        Assert.Equal(new DateOnly(2026, 6, 19), juneteenth.Value.ObservedDate);
        Assert.True(juneteenth.Value.ObservesWeekendAdjustment);

        var independence = UsHolidays.GetHolidayByName(UsHolidays.Names.IndependenceDay, 2026);
        Assert.NotNull(independence);
        Assert.Equal(UsHolidays.Names.IndependenceDay, independence.Value.Name);
        Assert.Equal(new DateOnly(2026, 7, 4), independence.Value.Date);
        Assert.Equal(new DateOnly(2026, 7, 3), independence.Value.ObservedDate); // Adjusted for Sat
        Assert.True(independence.Value.ObservesWeekendAdjustment);

        var labor = UsHolidays.GetHolidayByName(UsHolidays.Names.LaborDay, 2026);
        Assert.NotNull(labor);
        Assert.Equal(UsHolidays.Names.LaborDay, labor.Value.Name);
        Assert.Equal(new DateOnly(2026, 9, 7), labor.Value.Date);
        Assert.Equal(new DateOnly(2026, 9, 7), labor.Value.ObservedDate);
        Assert.False(labor.Value.ObservesWeekendAdjustment);

        var columbus = UsHolidays.GetHolidayByName(UsHolidays.Names.ColumbusDay, 2026);
        Assert.NotNull(columbus);
        Assert.Equal(UsHolidays.Names.ColumbusDay, columbus.Value.Name);
        Assert.Equal(new DateOnly(2026, 10, 12), columbus.Value.Date);
        Assert.Equal(new DateOnly(2026, 10, 12), columbus.Value.ObservedDate);
        Assert.False(columbus.Value.ObservesWeekendAdjustment);

        var veterans = UsHolidays.GetHolidayByName(UsHolidays.Names.VeteransDay, 2026);
        Assert.NotNull(veterans);
        Assert.Equal(UsHolidays.Names.VeteransDay, veterans.Value.Name);
        Assert.Equal(new DateOnly(2026, 11, 11), veterans.Value.Date);
        Assert.Equal(new DateOnly(2026, 11, 11), veterans.Value.ObservedDate);
        Assert.True(veterans.Value.ObservesWeekendAdjustment);

        var thanksgiving = UsHolidays.GetHolidayByName(UsHolidays.Names.Thanksgiving, 2026);
        Assert.NotNull(thanksgiving);
        Assert.Equal(UsHolidays.Names.Thanksgiving, thanksgiving.Value.Name);
        Assert.Equal(new DateOnly(2026, 11, 26), thanksgiving.Value.Date);
        Assert.Equal(new DateOnly(2026, 11, 26), thanksgiving.Value.ObservedDate);
        Assert.False(thanksgiving.Value.ObservesWeekendAdjustment);

        var christmas = UsHolidays.GetHolidayByName(UsHolidays.Names.Christmas, 2026);
        Assert.NotNull(christmas);
        Assert.Equal(UsHolidays.Names.Christmas, christmas.Value.Name);
        Assert.Equal(new DateOnly(2026, 12, 25), christmas.Value.Date);
        Assert.Equal(new DateOnly(2026, 12, 25), christmas.Value.ObservedDate);
        Assert.True(christmas.Value.ObservesWeekendAdjustment);

        var newYearsEve = UsHolidays.GetHolidayByName(UsHolidays.Names.NewYearsEve, 2026);
        Assert.NotNull(newYearsEve);
        Assert.Equal(UsHolidays.Names.NewYearsEve, newYearsEve.Value.Name);
        Assert.Equal(new DateOnly(2026, 12, 31), newYearsEve.Value.Date);
        Assert.Equal(new DateOnly(2026, 12, 31), newYearsEve.Value.ObservedDate);
        Assert.False(newYearsEve.Value.ObservesWeekendAdjustment);
    }

    [Theory]
    [InlineData("2026-12-25", true)]
    [InlineData("2026-12-26", false)]
    public void GetNameForHoliday_NotNullWhenFound(string date, bool isHoliday)
    {
        Assert.True(DateOnly.TryParse(date, out var dt));
        if (isHoliday)
            Assert.NotNull(UsHolidays.GetNameForHoliday(dt));
        else
            Assert.Null(UsHolidays.GetNameForHoliday(dt));
    }

    [Fact]
    public void GetWeekDaysExcludingHolidays_GetsDates()
    {
        var christmas = UsHolidays.GetHolidayDateByName(UsHolidays.Names.Christmas, 2025);
        Assert.True(christmas.HasValue);
        var newYears = UsHolidays.GetHolidayDateByName(UsHolidays.Names.NewYears, 2026);
        Assert.True(newYears.HasValue);
        var actual = UsHolidays.GetWeekDaysExcludingHolidays(christmas.Value, newYears.Value).ToImmutableArray();
        int expected = 3; // Christmas, New Year's Eve, and New Years all excluded.
        Assert.Equal(expected, actual.Length);
        Assert.DoesNotContain(christmas.GetValueOrDefault(), actual);
        Assert.DoesNotContain(newYears.GetValueOrDefault().AddDays(-1), actual);
        Assert.DoesNotContain(newYears.GetValueOrDefault(), actual);
    }

    [Fact]
    public void CountWeekDaysExcludingHolidays_CountsInclusive()
    {
        var christmas = UsHolidays.GetHolidayDateByName(UsHolidays.Names.Christmas, 2025);
        Assert.True(christmas.HasValue);
        var newYears = UsHolidays.GetHolidayDateByName(UsHolidays.Names.NewYears, 2026);
        Assert.True(newYears.HasValue);
        var actual = UsHolidays.CountWeekDaysExcludingHolidays(christmas.Value, newYears.Value);
        int expected = 3; // Christmas, New Year's Eve, and New Years all excluded.
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetInclusiveHolidaysBetweenDates_FindsHolidays()
    {
        var christmas = UsHolidays.GetHolidayDateByName(UsHolidays.Names.Christmas, 2025);
        Assert.True(christmas.HasValue);
        var newYears = UsHolidays.GetHolidayDateByName(UsHolidays.Names.NewYears, 2026);
        Assert.True(newYears.HasValue);
        var actual = UsHolidays.GetInclusiveHolidayDatesBetweenDates(christmas.Value, newYears.Value).ToImmutableArray();
        int expected = 3; // Christmas, New Year's Eve, and New Years all included.
        Assert.Equal(expected, actual.Length);
        Assert.Contains(christmas.GetValueOrDefault(), actual);
        Assert.Contains(newYears.GetValueOrDefault().AddDays(-1), actual);
        Assert.Contains(newYears.GetValueOrDefault(), actual);
    }

    [Theory]
    [InlineData(2026, "2026-01-01", "2026-01-01")]
    [InlineData(2027, "2027-01-01", "2027-01-01")]
    public void NewYearsDay_ReturnsExpectedHoliday(int year, string nominalStr, string observedStr)
    {
        var result = UsHolidays.NewYearsDay(year);
        var expectedNominal = DateOnly.Parse(nominalStr);
        var expectedObserved = DateOnly.Parse(observedStr);
        Assert.NotNull(result);
        Assert.Equal(UsHolidays.Names.NewYears, result.Value.Name);
        Assert.Equal(expectedNominal, result.Value.Date);
        Assert.Equal(expectedObserved, result.Value.ObservedDate);
    }

    [Theory]
    [InlineData(1985, null, null)]
    [InlineData(2026, "2026-01-19", "2026-01-19")]
    public void MartinLutherKingJrBirthday_ReturnsExpectedHoliday(int year, string? nominalStr, string? observedStr)
    {
        var result = UsHolidays.MartinLutherKingJrBirthday(year);
        if (nominalStr is null)
        {
            Assert.Null(result);
            return;
        }
        var expectedNominal = DateOnly.Parse(nominalStr);
        var expectedObserved = DateOnly.Parse(observedStr ?? "");
        Assert.NotNull(result);
        Assert.Equal(UsHolidays.Names.MartinLutherKingJr, result.Value.Name);
        Assert.Equal(expectedNominal, result.Value.Date);
        Assert.Equal(expectedObserved, result.Value.ObservedDate);
    }

    [Theory]
    [InlineData(1789, "1789-04-30", "1789-04-30")]
    [InlineData(1790, null, null)]
    [InlineData(2025, "2025-01-20", "2025-01-20")]
    [InlineData(2026, null, null)]
    public void InaugurationDay_ReturnsExpectedHoliday(int year, string? nominalStr, string? observedStr)
    {
        var result = UsHolidays.InaugurationDay(year);
        if (nominalStr is null)
        {
            Assert.Null(result);
            return;
        }
        var expectedNominal = DateOnly.Parse(nominalStr);
        var expectedObserved = DateOnly.Parse(observedStr ?? "");
        Assert.NotNull(result);
        Assert.Equal(UsHolidays.Names.InaugurationDay, result.Value.Name);
        Assert.Equal(expectedNominal, result.Value.Date);
        Assert.Equal(expectedObserved, result.Value.ObservedDate);
    }

    [Theory]
    [InlineData(1884, null, null)]
    [InlineData(2026, "2026-02-16", "2026-02-16")]
    public void PresidentsDay_ReturnsExpectedHoliday(int year, string? nominalStr, string? observedStr)
    {
        var result = UsHolidays.PresidentsDay(year);
        if (nominalStr is null)
        {
            Assert.Null(result);
            return;
        }
        var expectedNominal = DateOnly.Parse(nominalStr);
        var expectedObserved = DateOnly.Parse(observedStr ?? "");
        Assert.NotNull(result);
        Assert.Equal(UsHolidays.Names.PresidentsDay, result.Value.Name);
        Assert.Equal(expectedNominal, result.Value.Date);
        Assert.Equal(expectedObserved, result.Value.ObservedDate);
    }

    [Theory]
    [InlineData(2026, "2026-02-14", "2026-02-14")]
    public void ValentinesDay_ReturnsExpectedHoliday(int year, string nominalStr, string observedStr)
    {
        var result = UsHolidays.ValentinesDay(year);
        var expectedNominal = DateOnly.Parse(nominalStr);
        var expectedObserved = DateOnly.Parse(observedStr);
        Assert.NotNull(result);
        Assert.Equal(UsHolidays.Names.Valentines, result.Value.Name);
        Assert.Equal(expectedNominal, result.Value.Date);
        Assert.Equal(expectedObserved, result.Value.ObservedDate);
    }

    [Theory]
    [InlineData(2025, "2025-04-20", "2025-04-20")]
    [InlineData(2026, "2026-04-05", "2026-04-05")]
    public void EasterSunday_ReturnsExpectedHoliday(int year, string nominalStr, string observedStr)
    {
        var result = UsHolidays.EasterSunday(year);
        var expectedNominal = DateOnly.Parse(nominalStr);
        var expectedObserved = DateOnly.Parse(observedStr);
        Assert.NotNull(result);
        Assert.Equal(UsHolidays.Names.Easter, result.Value.Name);
        Assert.Equal(expectedNominal, result.Value.Date);
        Assert.Equal(expectedObserved, result.Value.ObservedDate);
    }

    [Theory]
    [InlineData(1867, null, null)]
    [InlineData(2025, "2025-05-26", "2025-05-26")]
    [InlineData(2026, "2026-05-25", "2026-05-25")]
    public void MemorialDay_ReturnsExpectedHoliday(int year, string? nominalStr, string? observedStr)
    {
        var result = UsHolidays.MemorialDay(year);
        if (nominalStr is null)
        {
            Assert.Null(result);
            return;
        }
        var expectedNominal = DateOnly.Parse(nominalStr);
        var expectedObserved = DateOnly.Parse(observedStr ?? "");
        Assert.NotNull(result);
        Assert.Equal(UsHolidays.Names.MemorialDay, result.Value.Name);
        Assert.Equal(expectedNominal, result.Value.Date);
        Assert.Equal(expectedObserved, result.Value.ObservedDate);
    }

    [Theory]
    [InlineData(2020, null, null)]
    [InlineData(2026, "2026-06-19", "2026-06-19")]
    [InlineData(2027, "2027-06-19", "2027-06-18")]
    public void Juneteenth_ReturnsExpectedHoliday(int year, string? nominalStr, string? observedStr)
    {
        var result = UsHolidays.Juneteenth(year);
        if (nominalStr is null)
        {
            Assert.Null(result);
            return;
        }
        var expectedNominal = DateOnly.Parse(nominalStr);
        var expectedObserved = DateOnly.Parse(observedStr ?? "");
        Assert.NotNull(result);
        Assert.Equal(UsHolidays.Names.Juneteenth, result.Value.Name);
        Assert.Equal(expectedNominal, result.Value.Date);
        Assert.Equal(expectedObserved, result.Value.ObservedDate);
    }

    [Theory]
    [InlineData(1869, null, null)]
    [InlineData(2026, "2026-07-04", "2026-07-03")]
    [InlineData(2027, "2027-07-04", "2027-07-05")]
    public void IndependenceDay_ReturnsExpectedHoliday(int year, string? nominalStr, string? observedStr)
    {
        var result = UsHolidays.IndependenceDay(year);
        if (nominalStr is null)
        {
            Assert.Null(result);
            return;
        }
        var expectedNominal = DateOnly.Parse(nominalStr);
        var expectedObserved = DateOnly.Parse(observedStr ?? "");
        Assert.NotNull(result);
        Assert.Equal(UsHolidays.Names.IndependenceDay, result.Value.Name);
        Assert.Equal(expectedNominal, result.Value.Date);
        Assert.Equal(expectedObserved, result.Value.ObservedDate);
    }

    [Theory]
    [InlineData(1886, null, null)]
    [InlineData(2026, "2026-09-07", "2026-09-07")]
    public void LaborDay_ReturnsExpectedHoliday(int year, string? nominalStr, string? observedStr)
    {
        var result = UsHolidays.LaborDay(year);
        if (nominalStr is null)
        {
            Assert.Null(result);
            return;
        }
        var expectedNominal = DateOnly.Parse(nominalStr);
        var expectedObserved = DateOnly.Parse(observedStr ?? "");
        Assert.NotNull(result);
        Assert.Equal(UsHolidays.Names.LaborDay, result.Value.Name);
        Assert.Equal(expectedNominal, result.Value.Date);
        Assert.Equal(expectedObserved, result.Value.ObservedDate);
    }

    [Theory]
    [InlineData(1936, null, null)]
    [InlineData(2026, "2026-10-12", "2026-10-12")]
    public void ColumbusDay_ReturnsExpectedHoliday(int year, string? nominalStr, string? observedStr)
    {
        var result = UsHolidays.ColumbusDay(year);
        if (nominalStr is null)
        {
            Assert.Null(result);
            return;
        }
        var expectedNominal = DateOnly.Parse(nominalStr);
        var expectedObserved = DateOnly.Parse(observedStr ?? "");
        Assert.NotNull(result);
        Assert.Equal(UsHolidays.Names.ColumbusDay, result.Value.Name);
        Assert.Equal(expectedNominal, result.Value.Date);
        Assert.Equal(expectedObserved, result.Value.ObservedDate);
    }

    [Theory]
    [InlineData(1937, null, null)]
    [InlineData(2026, "2026-11-11", "2026-11-11")]
    [InlineData(2028, "2028-11-11", "2028-11-10")]
    public void VeteransDay_ReturnsExpectedHoliday(int year, string? nominalStr, string? observedStr)
    {
        var result = UsHolidays.VeteransDay(year);
        if (nominalStr is null)
        {
            Assert.Null(result);
            return;
        }
        var expectedNominal = DateOnly.Parse(nominalStr);
        var expectedObserved = DateOnly.Parse(observedStr ?? "");
        Assert.NotNull(result);
        Assert.Equal(UsHolidays.Names.VeteransDay, result.Value.Name);
        Assert.Equal(expectedNominal, result.Value.Date);
        Assert.Equal(expectedObserved, result.Value.ObservedDate);
    }

    [Theory]
    [InlineData(1940, null, null)]
    [InlineData(2026, "2026-11-26", "2026-11-26")]
    public void ThanksgivingDay_ReturnsExpectedHoliday(int year, string? nominalStr, string? observedStr)
    {
        var result = UsHolidays.ThanksgivingDay(year);
        if (nominalStr is null)
        {
            Assert.Null(result);
            return;
        }
        var expectedNominal = DateOnly.Parse(nominalStr);
        var expectedObserved = DateOnly.Parse(observedStr ?? "");
        Assert.NotNull(result);
        Assert.Equal(UsHolidays.Names.Thanksgiving, result.Value.Name);
        Assert.Equal(expectedNominal, result.Value.Date);
        Assert.Equal(expectedObserved, result.Value.ObservedDate);
    }

    [Theory]
    [InlineData(1869, null, null)]
    [InlineData(2026, "2026-12-24", "2026-12-24")]
    public void ChristmasEveDay_ReturnsExpectedHoliday(int year, string? nominalStr, string? observedStr)
    {
        var result = UsHolidays.ChristmasEveDay(year);
        if (nominalStr is null)
        {
            Assert.Null(result);
            return;
        }
        var expectedNominal = DateOnly.Parse(nominalStr);
        var expectedObserved = DateOnly.Parse(observedStr ?? "");
        Assert.NotNull(result);
        Assert.Equal(UsHolidays.Names.ChristmasEve, result.Value.Name);
        Assert.Equal(expectedNominal, result.Value.Date);
        Assert.Equal(expectedObserved, result.Value.ObservedDate);
    }

    [Theory]
    [InlineData(1869, null, null)]
    [InlineData(2026, "2026-12-25", "2026-12-25")]
    [InlineData(2027, "2027-12-25", "2027-12-24")]
    public void ChristmasDay_ReturnsExpectedHoliday(int year, string? nominalStr, string? observedStr)
    {
        var result = UsHolidays.ChristmasDay(year);
        if (nominalStr is null)
        {
            Assert.Null(result);
            return;
        }
        var expectedNominal = DateOnly.Parse(nominalStr);
        var expectedObserved = DateOnly.Parse(observedStr ?? "");
        Assert.NotNull(result);
        Assert.Equal(UsHolidays.Names.Christmas, result.Value.Name);
        Assert.Equal(expectedNominal, result.Value.Date);
        Assert.Equal(expectedObserved, result.Value.ObservedDate);
    }

    [Theory]
    [InlineData(1869, null, null)]
    [InlineData(2026, "2026-12-31", "2026-12-31")]
    public void NewYearsEveDay_ReturnsExpectedHoliday(int year, string? nominalStr, string? observedStr)
    {
        var result = UsHolidays.NewYearsEveDay(year);
        if (nominalStr is null)
        {
            Assert.Null(result);
            return;
        }
        var expectedNominal = DateOnly.Parse(nominalStr);
        var expectedObserved = DateOnly.Parse(observedStr ?? "");
        Assert.NotNull(result);
        Assert.Equal(UsHolidays.Names.NewYearsEve, result.Value.Name);
        Assert.Equal(expectedNominal, result.Value.Date);
        Assert.Equal(expectedObserved, result.Value.ObservedDate);
    }
}
