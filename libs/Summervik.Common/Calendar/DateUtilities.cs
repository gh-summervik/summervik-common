namespace Summervik.Common.Calendar;

/// <summary>
/// Utility class for common date calculations using <see cref="DateOnly"/>.
/// </summary>
public static class DateUtilities
{
    /// <summary>
    /// Gets all dates between the two dates, inclusively (order-independent).
    /// </summary>
    public static IEnumerable<DateOnly> GetInclusiveDays(DateOnly date1, DateOnly date2)
    {
        if (date2 < date1)
            (date1, date2) = (date2, date1);

        while (date1 <= date2)
        {
            yield return date1;
            date1 = date1.AddDays(1);
        }
    }

    /// <summary>
    /// Counts all days inclusive of the dates provided.
    /// </summary>
    public static int CountDays(DateOnly date1, DateOnly date2) =>
        Math.Abs(date2.DayNumber - date1.DayNumber) + 1;

    /// <summary>
    /// Gets weekdays (Monday-Friday) inclusive of the dates provided..
    /// </summary>
    public static IEnumerable<DateOnly> GetWeekdays(DateOnly date1, DateOnly date2) =>
        GetInclusiveDays(date1, date2)
            .Where(d => d.DayOfWeek is not DayOfWeek.Saturday and not DayOfWeek.Sunday);

    /// <summary>
    /// Counts weekdays inclusive of the dates provided..
    /// Optimized mathematical calculation - no enumeration needed.
    /// </summary>
    public static int CountWeekdays(DateOnly start, DateOnly end)
    {
        if (end < start)
            (start, end) = (end, start);

        int totalDays = CountDays(start, end);
        if (totalDays == 0)
            return 0;

        // Full weeks contribute 5 business days each
        int fullWeeks = (totalDays - 1) / 7;
        int businessDays = fullWeeks * 5;

        // Count business days in the remaining days (up to 6)
        int remainingDays = (totalDays - 1) % 7 + 1;
        DayOfWeek dow = start.DayOfWeek;
        for (int i = 0; i < remainingDays; i++)
        {
            if (dow is not DayOfWeek.Saturday and not DayOfWeek.Sunday)
                businessDays++;

            dow = dow == DayOfWeek.Saturday ? DayOfWeek.Sunday : dow + 1;
        }

        return businessDays;
    }

    /// <summary>
    /// Adjusts a weekend date to the observed weekday (Saturday to previous Friday, Sunday to next Monday).
    /// Weekdays are unchanged. This follows common holiday observation rules.
    /// </summary>
    public static DateOnly AdjustToObservedWeekday(DateOnly date) =>
        date.DayOfWeek switch
        {
            DayOfWeek.Saturday => date.AddDays(-1),
            DayOfWeek.Sunday => date.AddDays(1),
            _ => date
        };
}
