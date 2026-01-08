using System.Collections.Immutable;

namespace Summervik.Calendar.Tests;

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
        var nyd = UsHolidays.NewYearsDay(2026);
        var expected = new DateOnly(2026, 1, 1);
        Assert.Equal(expected, nyd);
    }

    [Fact]
    public void NewYearsDay_2028_CelebratedDec31()
    {
        var nyd = UsHolidays.NewYearsDay(2028).GetValueOrDefault().CelebratedOn();
        var expected = new DateOnly(2027, 12, 31);
        Assert.Equal(expected, nyd);
    }

    [Theory]
    [InlineData(1985, null)]
    [InlineData(2026, "2026-01-19")]
    public void MlkJr_2026_Jan17(int year, string? expected)
    {
        DateOnly? exp = expected is null ? null : DateOnly.Parse(expected);
        Assert.Equal(UsHolidays.MartinLutherKingJrBirthday(year), exp);
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
        Assert.Equal(UsHolidays.InaugurationDay(year), exp);
    }

    [Theory]
    [InlineData(1884, null)]
    [InlineData(2026, "2026-02-16")]
    public void PresidentsDay(int year, string? expected)
    {
        DateOnly? exp = expected is null ? null : DateOnly.Parse(expected);
        Assert.Equal(UsHolidays.PresidentsDay(year), exp);
    }

    [Theory]
    [InlineData(2026, "2026-02-14")]
    public void ValentinesDay(int year, string? expected)
    {
        DateOnly? exp = expected is null ? null : DateOnly.Parse(expected);
        Assert.Equal(UsHolidays.ValentinesDay(year), exp);
    }

    [Theory]
    [InlineData(2025, "2025-04-20")]
    [InlineData(2026, "2026-04-05")]
    public void Easter(int year, string? expected)
    {
        DateOnly? exp = expected is null ? null : DateOnly.Parse(expected);
        Assert.Equal(UsHolidays.EasterSunday(year), exp);
    }

    [Theory]
    [InlineData(2025, "2025-05-26")]
    [InlineData(2026, "2026-05-25")]
    [InlineData(1867, null)]
    public void MemorialDay(int year, string? expected)
    {
        DateOnly? exp = expected is null ? null : DateOnly.Parse(expected);
        Assert.Equal(UsHolidays.MemorialDay(year), exp);
    }

    [Theory]
    [InlineData(2020, null)]
    [InlineData(2026, "2026-06-19")]
    public void Juneteenth(int year, string? expected)
    {
        DateOnly? exp = expected is null ? null : DateOnly.Parse(expected);
        Assert.Equal(UsHolidays.Juneteenth(year), exp);
    }

    [Fact]
    public void Juneteenth_2027_CelebratedJune18()
    {
        DateOnly exp = new(2027, 6, 18);
        Assert.Equal(exp, UsHolidays.Juneteenth(2027).GetValueOrDefault().CelebratedOn());
    }

    [Theory]
    [InlineData(1869, null)]
    [InlineData(2026, "2026-07-04")]
    public void IndependenceDay(int year, string? expected)
    {
        DateOnly? exp = expected is null ? null : DateOnly.Parse(expected);
        Assert.Equal(UsHolidays.IndependenceDay(year), exp);
    }

    [Fact]
    public void IndependenceDay_2027_CelebratedJuly5()
    {
        DateOnly exp = new(2027, 7, 5);
        Assert.Equal(exp, UsHolidays.IndependenceDay(2027).GetValueOrDefault().CelebratedOn());
    }

    [Theory]
    [InlineData(1886, null)]
    [InlineData(2026, "2026-09-07")]
    public void LaborDay(int year, string? expected)
    {
        DateOnly? exp = expected is null ? null : DateOnly.Parse(expected);
        Assert.Equal(UsHolidays.LaborDay(year), exp);
    }

    [Theory]
    [InlineData(1936, null)]
    [InlineData(2026, "2026-10-12")]
    public void ColumbusDay(int year, string? expected)
    {
        DateOnly? exp = expected is null ? null : DateOnly.Parse(expected);
        Assert.Equal(UsHolidays.ColumbusDay(year), exp);
    }

    [Theory]
    [InlineData(1937, null)]
    [InlineData(2026, "2026-11-11")]
    public void VeteransDay(int year, string? expected)
    {
        DateOnly? exp = expected is null ? null : DateOnly.Parse(expected);
        Assert.Equal(UsHolidays.VeteransDay(year), exp);
    }

    [Theory]
    [InlineData(1940, null)]
    [InlineData(2026, "2026-11-26")]
    public void ThanksgivingDay(int year, string? expected)
    {
        DateOnly? exp = expected is null ? null : DateOnly.Parse(expected);
        Assert.Equal(UsHolidays.ThanksgivingDay(year), exp);
    }

    [Theory]
    [InlineData(1869, null)]
    [InlineData(2026, "2026-12-24")]
    public void ChristmasEveDay(int year, string? expected)
    {
        DateOnly? exp = expected is null ? null : DateOnly.Parse(expected);
        Assert.Equal(UsHolidays.ChristmasEveDay(year), exp);
    }

    [Theory]
    [InlineData(1869, null)]
    [InlineData(2026, "2026-12-25")]
    public void ChristmasDay(int year, string? expected)
    {
        DateOnly? exp = expected is null ? null : DateOnly.Parse(expected);
        Assert.Equal(UsHolidays.ChristmasDay(year), exp);
    }

    [Theory]
    [InlineData(2026, "2026-12-31")]
    public void NewYearsEveDay(int year, string? expected)
    {
        DateOnly? exp = expected is null ? null : DateOnly.Parse(expected);
        Assert.Equal(UsHolidays.NewYearsEveDay(year), exp);
    }

    [Fact]
    public void ChristmasDay_2027_CelebratedDec24()
    {
        DateOnly exp = new(2027, 12, 24);
        Assert.Equal(exp, UsHolidays.ChristmasDay(2027).GetValueOrDefault().CelebratedOn());
    }

    [Fact]
    public void GetHoldaysForYear_2026()
    {
        var holidays = UsHolidays.GetHolidaysForYear(2026);

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

        var coll = new DateOnly[] {
            nyd,mlk,vald,pd,md,jt,id,ld,cold,vd,td,cd,nye
        };

        foreach (var d in coll)
            Assert.Contains(d, holidays.Values);
    }

    [Fact]
    public void GetHolidayByName_2026()
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

        Assert.Equal(nyd, UsHolidays.GetHolidayByName(UsHolidays.Names.NewYears, 2026).GetValueOrDefault());
        Assert.Equal(mlk, UsHolidays.GetHolidayByName(UsHolidays.Names.MartinLutherKingJr, 2026).GetValueOrDefault());
        Assert.Equal(vald, UsHolidays.GetHolidayByName(UsHolidays.Names.Valentines, 2026).GetValueOrDefault());
        Assert.Equal(pd, UsHolidays.GetHolidayByName(UsHolidays.Names.PresidentsDay, 2026).GetValueOrDefault());
        Assert.Equal(md, UsHolidays.GetHolidayByName(UsHolidays.Names.MemorialDay, 2026).GetValueOrDefault());
        Assert.Equal(jt, UsHolidays.GetHolidayByName(UsHolidays.Names.Juneteenth, 2026).GetValueOrDefault());
        Assert.Equal(id, UsHolidays.GetHolidayByName(UsHolidays.Names.IndependenceDay, 2026).GetValueOrDefault());
        Assert.Equal(ld, UsHolidays.GetHolidayByName(UsHolidays.Names.LaborDay, 2026).GetValueOrDefault());
        Assert.Equal(cold, UsHolidays.GetHolidayByName(UsHolidays.Names.ColumbusDay, 2026).GetValueOrDefault());
        Assert.Equal(vd, UsHolidays.GetHolidayByName(UsHolidays.Names.VeteransDay, 2026).GetValueOrDefault());
        Assert.Equal(td, UsHolidays.GetHolidayByName(UsHolidays.Names.Thanksgiving, 2026).GetValueOrDefault());
        Assert.Equal(cd, UsHolidays.GetHolidayByName(UsHolidays.Names.Christmas, 2026).GetValueOrDefault());
        Assert.Equal(nye, UsHolidays.GetHolidayByName(UsHolidays.Names.NewYearsEve, 2026).GetValueOrDefault());
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
        var christmas = UsHolidays.GetHolidayByName(UsHolidays.Names.Christmas, 2025);
        Assert.True(christmas.HasValue);
        var newYears = UsHolidays.GetHolidayByName(UsHolidays.Names.NewYears, 2026);
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
        var christmas = UsHolidays.GetHolidayByName(UsHolidays.Names.Christmas, 2025);
        Assert.True(christmas.HasValue);
        var newYears = UsHolidays.GetHolidayByName(UsHolidays.Names.NewYears, 2026);
        Assert.True(newYears.HasValue);
        var actual = UsHolidays.CountWeekDaysExcludingHolidays(christmas.Value, newYears.Value);
        int expected = 3; // Christmas, New Year's Eve, and New Years all excluded.
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetInclusiveHolidaysBetweenDates_FindsHolidays()
    {
        var christmas = UsHolidays.GetHolidayByName(UsHolidays.Names.Christmas, 2025);
        Assert.True(christmas.HasValue);
        var newYears = UsHolidays.GetHolidayByName(UsHolidays.Names.NewYears, 2026);
        Assert.True(newYears.HasValue);
        var actual = UsHolidays.GetInclusiveHolidaysBetweenDates(christmas.Value, newYears.Value).ToImmutableArray();
        int expected = 3; // Christmas, New Year's Eve, and New Years all included.
        Assert.Equal(expected, actual.Length);
        Assert.Contains(christmas.GetValueOrDefault(), actual);
        Assert.Contains(newYears.GetValueOrDefault().AddDays(-1), actual);
        Assert.Contains(newYears.GetValueOrDefault(), actual);
    }
}
