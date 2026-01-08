# summervik-common

Useful business-oriented C# extensions and utilities.

## Calendar

The `DateUtilities` class has a few methods for counting weekdays and adjusting weekend days to their nearest weekday.

The `UsHolidays` class will calculate U.S. holidays (both Federal and cultural) for a given year and also find holidays in a date range.
Methods in this class will return `null` when the provided year is before the holiday was officially accepted.
Exceptions to this rule include the ones you might expect, such as New Years, Easter, and Valentine's Day (which has an unknown origin??).

## Validators

The `Validators` library contains validators of U.S. phone numbers and Social Security Numbers.
The SSN validator works with the 2026 rules.

## Transformations

The `WagesByFrequency` class will convert dollar amounts between frequencies - useful in all sorts of government assistance applications and accounting utilities.