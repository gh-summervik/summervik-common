namespace Summervik.Common.Extensions;

public static class DateTimeExtensions
{

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

        var timespan = dateTime.TimeOfDay;

        var dt = DateOnly.FromDateTime(dateTime);
        var dt2 = dt.AddWeekdays(numberToIncrement);
        return new DateTime(dt2, TimeOnly.FromTimeSpan(timespan), dateTime.Kind);
    }

    /// <summary>
    /// Count weekdays between two dates.
    /// </summary>
    /// <param name="startDate">The start date.</param>
    /// <param name="endDate">The end date.</param>
    /// <param name="includeFirstDay">An indicator of whether to count the first day.</param>
    /// <returns>A count of weekdays between the two dates, inclusive of the first day
    /// if the <paramref name="includeFirstDay"/> argument is true.</returns>
    public static int CountWeekdays(this DateTime startDate, DateTime endDate, bool includeFirstDay = false)
    {
        var dt = DateOnly.FromDateTime(startDate);
        var dt2 = DateOnly.FromDateTime(endDate);
        return dt.CountWeekdays(dt2, includeFirstDay);
    }

    /// <summary>
    /// An convenience function to check if a day is a weekday.
    /// </summary>
    public static bool IsWeekday(this DateTime date) =>
        date.DayOfWeek is not DayOfWeek.Saturday and not DayOfWeek.Sunday;
}
