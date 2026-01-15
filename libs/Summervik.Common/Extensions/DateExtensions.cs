namespace Summervik.Common.Extensions;

public static class DateExtensions
{
    /// <summary>
    /// Gets the date a holiday would be celebrated on (adjusted for weekends).
    /// </summary>
    public static DateOnly CelebratedOn(this DateOnly date) => Calendar.DateUtilities.AdjustToObservedWeekday(date);

    /// <summary>
    /// Converts a <see cref="DateTime"/> to the start of its day, preserving the <see cref="DateTime.Kind"/>
    /// </summary>
    public static DateTime ToStartOfDay(this DateTime dateTime) =>
        DateOnly.FromDateTime(dateTime).ToDateTime(TimeOnly.MinValue, dateTime.Kind);

    /// <summary>
    /// Converts a <see cref="DateTime"/> to the end of its day, preserving the <see cref="DateTime.Kind"/>.
    /// </summary>
    public static DateTime ToEndOfDay(this DateTime dateTime) =>
        DateOnly.FromDateTime(dateTime).ToDateTime(TimeOnly.MaxValue, dateTime.Kind);

    /// <summary>
    /// Add or subtract weekdays from the specified date.
    /// </summary>
    public static DateTime AddWeekdays(this DateTime dateTime, int numberToIncrement)
    {
        if (numberToIncrement == 0)
            return dateTime;

        DateTime dt = dateTime;
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
}
