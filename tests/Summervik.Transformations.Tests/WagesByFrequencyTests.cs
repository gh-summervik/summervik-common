using static Summervik.Transformations.WagesByFrequency;

namespace Summervik.Transformations.Tests;

public class WagesByFrequencyTests
{
    private static void AssertApproximately(decimal expected, decimal actual, decimal relativeTolerance = 0.000001M) 
    {
        if (expected == 0M)
        {
            Assert.Equal(0M, actual);
            return;
        }

        decimal high = expected * (1M + relativeTolerance);
        decimal low = expected * (1M - relativeTolerance);
        Assert.True(actual >= low && actual <= high,
            $"{actual} is not within {relativeTolerance:p8} of expected {expected} (range: {low} to {high}).");
    }

    [Fact]
    public void ConvertWages_SameFrequency_ReturnsOriginalAmount()
    {
        decimal amount = 1000.00M;

        var result = WagesByFrequency.ConvertWages(
            PayFrequency.Daily, PayFrequency.Daily, amount);

        Assert.Equal(amount, result);
    }

    [Fact]
    public void ConvertWages_ZeroAmount_ReturnsZero()
    {
        decimal result = WagesByFrequency.ConvertWages(
            PayFrequency.Hourly, PayFrequency.Annually, 0M);

        Assert.Equal(0M, result);
    }

    [Fact]
    public void ConvertWages_FromHourly_NonPrecise()
    {
        decimal hourlyAmount = 10.00M;

        decimal expectedDaily = hourlyAmount * 8M;
        decimal expectedWeekly = hourlyAmount * 40M;
        decimal expectedBiWeekly = expectedWeekly * 2M;
        decimal expectedSemiMonthly = (hourlyAmount * 40M * 52M) / 24M;
        decimal expectedMonthly = (hourlyAmount * 40M * 52M) / 12M;
        decimal expectedQuarterly = expectedMonthly * 3M;
        decimal expectedSemiAnnually = expectedMonthly * 6M;
        decimal expectedAnnually = hourlyAmount * 40M * 52M;

        AssertApproximately(expectedDaily,
            WagesByFrequency.ConvertWages(PayFrequency.Hourly, PayFrequency.Daily, hourlyAmount));

        AssertApproximately(expectedWeekly,
            WagesByFrequency.ConvertWages(PayFrequency.Hourly, PayFrequency.Weekly, hourlyAmount));

        AssertApproximately(expectedBiWeekly,
            WagesByFrequency.ConvertWages(PayFrequency.Hourly, PayFrequency.BiWeekly, hourlyAmount));

        AssertApproximately(expectedSemiMonthly,
            WagesByFrequency.ConvertWages(PayFrequency.Hourly, PayFrequency.SemiMonthly, hourlyAmount));

        AssertApproximately(expectedMonthly,
            WagesByFrequency.ConvertWages(PayFrequency.Hourly, PayFrequency.Monthly, hourlyAmount));

        AssertApproximately(expectedQuarterly,
            WagesByFrequency.ConvertWages(PayFrequency.Hourly, PayFrequency.Quarterly, hourlyAmount));

        AssertApproximately(expectedSemiAnnually,
            WagesByFrequency.ConvertWages(PayFrequency.Hourly, PayFrequency.SemiAnnually, hourlyAmount));

        AssertApproximately(expectedAnnually,
            WagesByFrequency.ConvertWages(PayFrequency.Hourly, PayFrequency.Annually, hourlyAmount));
    }

    [Fact]
    public void ConvertWages_FromDaily_NonPrecise()
    {
        decimal dailyAmount = 80.00M;

        decimal expectedHourly = dailyAmount / 8M;
        decimal expectedWeekly = dailyAmount * 5M;
        decimal expectedBiWeekly = expectedWeekly * 2M;
        decimal expectedSemiMonthly = (dailyAmount * 5M * 52M) / 24M;
        decimal expectedMonthly = (dailyAmount * 5M * 52M) / 12M;
        decimal expectedQuarterly = expectedMonthly * 3M;
        decimal expectedSemiAnnually = expectedMonthly * 6M;
        decimal expectedAnnually = dailyAmount * 5M * 52M;

        AssertApproximately(expectedHourly,
            WagesByFrequency.ConvertWages(PayFrequency.Daily, PayFrequency.Hourly, dailyAmount));

        AssertApproximately(expectedWeekly,
            WagesByFrequency.ConvertWages(PayFrequency.Daily, PayFrequency.Weekly, dailyAmount));

        AssertApproximately(expectedBiWeekly,
            WagesByFrequency.ConvertWages(PayFrequency.Daily, PayFrequency.BiWeekly, dailyAmount));

        AssertApproximately(expectedSemiMonthly,
            WagesByFrequency.ConvertWages(PayFrequency.Daily, PayFrequency.SemiMonthly, dailyAmount));

        AssertApproximately(expectedMonthly,
            WagesByFrequency.ConvertWages(PayFrequency.Daily, PayFrequency.Monthly, dailyAmount));

        AssertApproximately(expectedQuarterly,
            WagesByFrequency.ConvertWages(PayFrequency.Daily, PayFrequency.Quarterly, dailyAmount));

        AssertApproximately(expectedSemiAnnually,
            WagesByFrequency.ConvertWages(PayFrequency.Daily, PayFrequency.SemiAnnually, dailyAmount));

        AssertApproximately(expectedAnnually,
            WagesByFrequency.ConvertWages(PayFrequency.Daily, PayFrequency.Annually, dailyAmount));
    }

    [Fact]
    public void ConvertWages_PreciseMode_ExampleConversions()
    {
        decimal hourlyAmount = 10.00M;
        double weeksPerYearPrecise = 52.17821782178218D; // Exact value from 36890 days / 101 years / 7

        decimal expectedAnnuallyPrecise = hourlyAmount * 40M * (decimal)weeksPerYearPrecise;
        decimal expectedMonthlyPrecise = expectedAnnuallyPrecise / 12M;

        var annually = WagesByFrequency.ConvertWages(
            PayFrequency.Hourly, PayFrequency.Annually, hourlyAmount, usePrecision: true);

        var monthly = WagesByFrequency.ConvertWages(
            PayFrequency.Hourly, PayFrequency.Monthly, hourlyAmount, usePrecision: true);

        AssertApproximately(expectedAnnuallyPrecise, annually);
        AssertApproximately(expectedMonthlyPrecise, monthly);

        // Round-trip: annual back to hourly should recover the original hourly rate
        var roundTrip = WagesByFrequency.ConvertWages(
            PayFrequency.Annually, PayFrequency.Hourly, annually, usePrecision: true);

        AssertApproximately(hourlyAmount, roundTrip);
    }

    [Fact]
    public void ConvertWages_StringOverload_ValidFrequencies()
    {
        decimal amount = 10.00M;

        decimal result = WagesByFrequency.ConvertWages(
            "hourly", "annually", amount);

        AssertApproximately(20800.00M, result);

        result = WagesByFrequency.ConvertWages(
            "every 2 weeks", "monthly", amount * 80M);

        AssertApproximately((800M * 26M) / 12M, result);
    }

    [Fact]
    public void ConvertWages_StringOverload_VariantsAndCaseInsensitivity()
    {
        decimal amount = 10.00M;

        WagesByFrequency.ConvertWages("Bi-Weekly", "Annually", amount);
        WagesByFrequency.ConvertWages("semi monthly", "hourly", amount);
        WagesByFrequency.ConvertWages("SemiAnnually", "Daily", amount);
    }

    [Fact]
    public void ConvertWages_StringOverload_InvalidFrequency_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            WagesByFrequency.ConvertWages("invalid", "hourly", 10M));

        Assert.Throws<ArgumentException>(() =>
            WagesByFrequency.ConvertWages("hourly", "badfreq", 10M));
    }
}