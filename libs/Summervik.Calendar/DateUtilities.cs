namespace Summervik.Calendar;

public static class DateUtilities
{
    /// <summary>
    /// Get all dates between the two dates, inclusively.
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
    /// Gets weekdays inclusive of the dates provided.
    /// </summary>
    public static IEnumerable<DateOnly> GetWeekDays(DateOnly date1, DateOnly date2) =>
        GetInclusiveDays(date1, date2).Where(d => d.DayOfWeek is not (DayOfWeek.Saturday or DayOfWeek.Sunday));

    /// <summary>
    /// Counts weekdays inclusive of the dates provided.
    /// </summary>
    public static int CountWeekDays(DateOnly date1, DateOnly date2) =>
        GetWeekDays(date1, date2).Count();

    public static DateOnly AdjustWeekendDayToNearestWeekday(DateOnly date) =>
        date.DayOfWeek == DayOfWeek.Saturday
            ? date.AddDays(-1)
            : date.DayOfWeek == DayOfWeek.Sunday
                ? date.AddDays(1)
                : date;

    /// <summary>
    /// Extension to move Saturday to Friday and Sunday to Monday when appropriate.
    /// </summary>
    public static DateOnly CelebratedOn(this DateOnly date) => AdjustWeekendDayToNearestWeekday(date);
}