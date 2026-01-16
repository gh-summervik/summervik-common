namespace Summervik.Common.Calendar;

/// <summary>
/// Represents a holiday. The holiday consists of a name, a date, and an indicator of whether
/// the holiday observes weekend adjustments (i.e., if the holiday lands on a weekend, it moves to 
/// its nearest weekday).
/// </summary>
public readonly record struct Holiday(string Name, DateOnly Date, bool ObservesWeekendAdjustment = false)
{
    /// <summary>
    /// The date the holiday is observed.
    /// </summary>
    public DateOnly ObservedDate => ObservesWeekendAdjustment
        ? UsHolidays.GetObservedDate(Date)
        : Date;
}