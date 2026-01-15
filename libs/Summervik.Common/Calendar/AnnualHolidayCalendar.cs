namespace Summervik.Common.Calendar;

public class AnnualHolidayCalendar
{
    private readonly DateOnly _start;
    private readonly DateOnly _end;
    private readonly HashSet<Holiday> _holidays = new(20);

    public AnnualHolidayCalendar(int year)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(year, 1);
        _start = new DateOnly(year, 1, 1);
        _end = new DateOnly(year, 12, 31);
    }

    public AnnualHolidayCalendar(DateOnly start)
    {
        if (start.Year < 1)
            throw new ArgumentOutOfRangeException(nameof(start));

        _start = start;
        _end = _start.AddYears(1).AddDays(-1);
    }

    public DateOnly Start => _start;
    public DateOnly End => _end;
    public int FiscalYear => _end.Year;

    public IReadOnlyCollection<Holiday> Holidays => [.. _holidays.OrderBy(k => k.Date)];

    public Holiday? GetHolidayByName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));

        var match = _holidays.FirstOrDefault(k => k.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        return match == default ? null : match;
    }

    public IEnumerable<Holiday> GetHolidaysByDate(DateOnly date)
    {
        return _holidays.Where(k => k.Date.Equals(date) || k.ObservedDate.Equals(date));
    }

    public bool IsHoliday(DateOnly date) => GetHolidaysByDate(date).Any();

    public AnnualHolidayCalendar WithFederalUsHolidays()
    {
        var holidays = UsHolidays.GetInclusiveHolidaysBetweenDates(_start, _end);

        var fedHolidays = UsHolidays.Names.GetFederalHolidays();

        foreach (var h in holidays.Where(h => fedHolidays.Contains(h.Name, StringComparer.OrdinalIgnoreCase)))
            _holidays.Add(h);

        return this;
    }

    public AnnualHolidayCalendar WithAllUsHolidays()
    {
        foreach (var h in UsHolidays.GetInclusiveHolidaysBetweenDates(_start, _end))
            _holidays.Add(h);

        return this;
    }

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

    public AnnualHolidayCalendar WithHoliday(Holiday holiday)
    {
        return WithHoliday(holiday.Date, holiday.Name, holiday.ObservesWeekendAdjustment);
    }

    public AnnualHolidayCalendar RemoveHoliday(Holiday holiday)
    {
        _holidays.Remove(holiday);
        return this;
    }

    public AnnualHolidayCalendar RemoveHoliday(DateOnly date)
    {
        _holidays.RemoveWhere(k => k.Date.Equals(date) || k.ObservedDate.Equals(date));
        return this;
    }
}
