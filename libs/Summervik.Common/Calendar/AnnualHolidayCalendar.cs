using System.Collections.Immutable;

namespace Summervik.Common.Calendar;

/// <summary>
/// Represents a custom holiday calendar for a one-year period.
/// </summary>
public class AnnualHolidayCalendar
{
    private readonly DateOnly _start;
    private readonly DateOnly _end;
    private readonly HashSet<Holiday> _holidays = new(20);

    /// <summary>
    /// Create an instance of <see cref="AnnualHolidayCalendar"/> for a calendar year.
    /// </summary>
    public AnnualHolidayCalendar(int year)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(year, 1);
        _start = new DateOnly(year, 1, 1);
        _end = new DateOnly(year, 12, 31);
    }

    /// <summary>
    /// Create an instance of <see cref="AnnualHolidayCalendar"/> for a fiscal year
    /// starting at the date provided and ending one year later.
    /// </summary>
    /// <param name="start">The start date of the fiscal year.</param>
    public AnnualHolidayCalendar(DateOnly start)
    {
        if (start.Year < 1)
            throw new ArgumentOutOfRangeException(nameof(start));

        _start = start;
        _end = _start.AddYears(1).AddDays(-1);
    }

    public DateOnly Start => _start;
    public DateOnly End => _end;

    /// <summary>
    /// Fiscal year is defined by the year of the <see cref="End"/> date.
    /// </summary>
    public int FiscalYear => _end.Year;

    /// <summary>
    /// A read-only collection of holidays in date order.
    /// </summary>
    public IReadOnlyCollection<Holiday> Holidays => [.. _holidays.OrderBy(k => k.Date)];

    /// <summary>
    /// Gets the first <see cref="Holiday"/> matching the name provided.
    /// The match is case insensitive.
    /// </summary>
    public Holiday? GetHolidayByName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        var list = GetHolidaysByName(name).ToImmutableArray();
        return list.Length == 0 ? null : list[0];
    }

    /// <summary>
    /// Get all holidays matching the name provided.
    /// The check is case insensitive.
    /// </summary>
    public IEnumerable<Holiday> GetHolidaysByName(string name) =>
        _holidays.Where(k => k.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
        .OrderBy(k => k.Date);

    /// <summary>
    /// Gets all <see cref="Holiday"/> instances for the date provided.
    /// The match includes both the official date and the observed date.
    /// </summary>
    public IEnumerable<Holiday> GetHolidaysByDate(DateOnly date) =>
        _holidays.Where(k => k.Date.Equals(date) || k.ObservedDate.Equals(date))
        .OrderBy(k => k.Date);

    /// <summary>
    /// Gets an indicator of whether the date provided is a holiday.
    /// This method respects both the date and the observed date of the holiday.
    /// </summary>
    public bool IsHoliday(DateOnly date) => GetHolidaysByDate(date).Any();

    /// <summary>
    /// Add Federal holidays to the holiday list.
    /// </summary>
    public AnnualHolidayCalendar WithFederalUsHolidays()
    {
        var holidays = UsHolidays.GetInclusiveHolidaysBetweenDates(_start, _end);

        var fedHolidays = UsHolidays.Names.GetFederalHolidays();

        foreach (var h in holidays.Where(h => fedHolidays.Contains(h.Name, StringComparer.OrdinalIgnoreCase)))
            _holidays.Add(h);

        return this;
    }

    /// <summary>
    /// Add all US holidays to the holiday list.
    /// </summary>
    public AnnualHolidayCalendar WithAllUsHolidays()
    {
        foreach (var h in UsHolidays.GetInclusiveHolidaysBetweenDates(_start, _end))
            _holidays.Add(h);

        return this;
    }

    /// <summary>
    /// Add a specific holiday to the holiday list.
    /// The list prevents exact duplicates, but duplicate names and/or dates are allowed.
    /// </summary>
    public AnnualHolidayCalendar WithHoliday(DateOnly date, string name, bool observesWeekendAdjustment = false)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        if (date < _start || date > _end)
            throw new ArgumentOutOfRangeException(nameof(date), $"Holidays must be between {_start:yyyy-MM-dd} and {_end:yyyy-MM-dd}.");

        _holidays.Add(new Holiday()
        {
            Name = name,
            Date = date,
            ObservesWeekendAdjustment = observesWeekendAdjustment
        });
        return this;
    }

    /// <summary>
    /// Adds a holiday to the holiday list.
    /// </summary>
    public AnnualHolidayCalendar WithHoliday(Holiday holiday)
    {
        return WithHoliday(holiday.Date, holiday.Name, holiday.ObservesWeekendAdjustment);
    }

    /// <summary>
    /// Remove a holiday from the holiday list.
    /// </summary>
    public AnnualHolidayCalendar RemoveHoliday(Holiday holiday)
    {
        _holidays.Remove(holiday);
        return this;
    }

    /// <summary>
    /// Removes from the holiday list all holidays with the date or observed date.
    /// </summary>
    public AnnualHolidayCalendar RemoveHoliday(DateOnly date)
    {
        _holidays.RemoveWhere(k => k.Date.Equals(date) || k.ObservedDate.Equals(date));
        return this;
    }

    /// <summary>
    /// Removes from the holiday list all holidays with the specified name.
    /// The name check is case insensitive.
    /// </summary>
    public AnnualHolidayCalendar RemoveHoliday(string name)
    {
        _holidays.RemoveWhere(k => k.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        return this;
    }
}
