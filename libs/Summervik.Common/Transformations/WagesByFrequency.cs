namespace Summervik.Common.Transformations;

/// <summary>
/// Utility class for converting dollar amounts between pay frequencies.
/// </summary>
public static class WagesByFrequency
{
    public enum PayFrequency
    {
        Hourly,
        Daily,
        Weekly,
        BiWeekly,          
        SemiMonthly,
        Monthly,
        Quarterly,
        SemiAnnually,
        Annually
    }

    private static class CalendarFactors
    {
        public const double MonthsInYear = 12D;
        public const double WeeksInYear = 52D;
        public const double WorkDaysPerWeek = 5D;

        public static double DaysInYearPrecise
        {
            get
            {
                var startDate = new DateOnly(1900, 1, 1);
                var endDate = new DateOnly(2000, 12, 31);
                // 101 years inclusive, approx. 365.2475 days/year
                return (endDate.DayNumber - startDate.DayNumber + 1) / 101D;
            }
        }

        public static double WeeksInYearPrecise => DaysInYearPrecise / 7D;
    }

    private static double GetAnnualMultiplier(PayFrequency frequency, bool usePrecision, double workHoursInDay)
    {
        double weeksPerYear = usePrecision ? CalendarFactors.WeeksInYearPrecise : CalendarFactors.WeeksInYear;

        double effectiveHoursPerWeek = workHoursInDay * CalendarFactors.WorkDaysPerWeek;

        return frequency switch
        {
            PayFrequency.Hourly => effectiveHoursPerWeek * weeksPerYear,
            PayFrequency.Daily => CalendarFactors.WorkDaysPerWeek * weeksPerYear,
            PayFrequency.Weekly => weeksPerYear,
            PayFrequency.BiWeekly => weeksPerYear / 2D,
            PayFrequency.SemiMonthly => 24D,
            PayFrequency.Monthly => 12D,
            PayFrequency.Quarterly => 4D,
            PayFrequency.SemiAnnually => 2D,
            PayFrequency.Annually => 1D,
            _ => throw new ArgumentException($"Unknown frequency: {frequency}")
        };
    }

    /// <summary>
    /// Convert a wage dollar amount from one frequency to another.
    /// </summary>
    public static decimal ConvertWages(PayFrequency sourceFrequency, PayFrequency targetFrequency, decimal amount,
        double workHoursInDay = 8D, bool usePrecision = false)
    {
        if (amount == 0 || sourceFrequency == targetFrequency)
            return amount;

        double multSource = GetAnnualMultiplier(sourceFrequency, usePrecision, workHoursInDay);
        double multTarget = GetAnnualMultiplier(targetFrequency, usePrecision, workHoursInDay);

        decimal factor = (decimal)(multSource / multTarget);
        return amount * factor;
    }

    /// <summary>
    /// Convert a wage dollar amount from one frequency to another.
    /// </summary>
    public static decimal ConvertWages(string sourceFrequency, string targetFrequency, decimal amount,
        double workHoursInDay = 8D, bool usePrecision = false)
    {
        if (!TryParseFrequency(sourceFrequency, out PayFrequency src))
            throw new ArgumentException($"Unknown source frequency: {sourceFrequency}");
        if (!TryParseFrequency(targetFrequency, out PayFrequency tgt))
            throw new ArgumentException($"Unknown target frequency: {targetFrequency}");

        return ConvertWages(src, tgt, amount, workHoursInDay, usePrecision);
    }

    private static bool TryParseFrequency(string input, out PayFrequency frequency)
    {
        frequency = default;

        if (string.IsNullOrWhiteSpace(input))
            return false;

        string normalized = input.ToLowerInvariant()
                                 .Trim()
                                 .Replace(" ", "")
                                 .Replace("-", "");

        if (Enum.TryParse<PayFrequency>(normalized, ignoreCase: true, out frequency))
            return true;

        if (normalized == "every2weeks")
        {
            frequency = PayFrequency.BiWeekly;
            return true;
        }

        return false;
    }
}
