namespace Summervik.Common.Extensions;

public static class DateOnlyExtensions
{
    /// <summary>
    /// Gets the date a holiday would be celebrated on (adjusted for weekends).
    /// </summary>
    public static DateOnly MoveToNearestWeekday(this DateOnly date) => Calendar.DateUtilities.MoveToNearestWeekday(date);

    public static DateOnly AddWeekdays(this DateOnly dateOnly, int numberToIncrement)
    {
        DateOnly date = dateOnly;
        if (numberToIncrement == 0)
            return date;

        DateOnly dt = dateOnly;
        int c = 0;
        int m = Math.Abs(numberToIncrement);
        int i = numberToIncrement > 0 ? 1 : -1;

        while (c < m)
        {
            dt = dt.AddDays(i);
            if (dt.DayOfWeek is not DayOfWeek.Saturday and not DayOfWeek.Sunday)
                c++;
        }

        return dt;
    }

    /// <summary>
    /// Count weekdays between two dates.
    /// </summary>
    /// <param name="startDate">The start date.</param>
    /// <param name="endDate">The end date.</param>
    /// <param name="includeFirstDay">An indicator of whether to count the first day.</param>
    /// <returns>A count of weekdays between the two dates, inclusive of the first day
    /// if the <paramref name="includeFirstDay"/> argument is true.</returns>
    public static int CountWeekdays(this DateOnly startDate, DateOnly endDate, bool includeFirstDay = false)
    {
        if (startDate == endDate)
            return includeFirstDay && IsWeekday(startDate) ? 1 : 0;

        int totalDays = Math.Abs(endDate.DayNumber - startDate.DayNumber) + (includeFirstDay ? 1 : 0);
        if (totalDays == 0)
            return 0;

        DateOnly start = includeFirstDay ? startDate : startDate.AddDays(endDate > startDate ? 1 : -1);

        int weeks = totalDays / 7;
        int remainder = totalDays % 7;
        int weekdays = weeks * 5; // 5 weekdays per week

        DateOnly current = start;
        for (int i = 0; i < remainder; i++)
        {
            if (IsWeekday(current))
                weekdays += endDate >= startDate ? 1 : -1;
            current = current.AddDays(endDate >= startDate ? 1 : -1);
        }

        return weekdays;
    }

    /// <summary>
    /// An convenience function to check if a day is a weekday.
    /// </summary>
    public static bool IsWeekday(this DateOnly date) =>
        date.DayOfWeek is not DayOfWeek.Saturday and not DayOfWeek.Sunday;
}
