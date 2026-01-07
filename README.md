# summervik-common

Useful business-oriented C# extensions and utilities.

## Calendar

The `DateUtilities` has a few methods for counting weekdays and adjusting weekend days to their nearest weekday.

The `UnitedStatesCalendar` class will calculate U.S. holidays for a given year and also find holidays in a date range.
Methods in this class attempts to return `null` when the provided year is before the holiday was officially accepted.
Exceptions to this rule include the ones you might expect, such as New Years, Easter, and Valentine's Day.

## Validators

The `Validators` library contains some common validation routines, including emails, phone numbers, and SSNs.

The `EmailAddress`, in particular, is often highly underestimated - there's a lot going on.
