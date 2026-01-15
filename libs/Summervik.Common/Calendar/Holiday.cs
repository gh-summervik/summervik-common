namespace Summervik.Common.Calendar;

public readonly record struct Holiday(string Name, DateOnly Date, bool ObservesWeekendAdjustment = false)
{
    public DateOnly ObservedDate => ObservesWeekendAdjustment
        ? UsHolidays.GetObservedDate(Date)
        : Date;
}