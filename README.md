# Summervik.Common
A collection of practical, business-oriented C# utilities and extensions for common tasks in applications like payroll, government forms, templating, and data formatting.

This library focuses on reliability, simplicity, and real-world usability. All classes are thoroughly unit-tested — check out the test project for detailed usage examples.

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download)
[![.NET](https://img.shields.io/badge/.NET-9.0-blue.svg)](https://dotnet.microsoft.com/download)
[![.NET](https://img.shields.io/badge/.NET-10.0-blue.svg)](https://dotnet.microsoft.com/download)

## Installation
Available as a NuGet package:

[![NuGet Version](https://img.shields.io/nuget/v/Summervik.Common.svg?style=flat)](https://www.nuget.org/packages/Summervik.Common/)[![NuGet Downloads](https://img.shields.io/nuget/dt/Summervik.Common.svg?style=flat)](https://www.nuget.org/packages/Summervik.Common/)

```bash
dotnet add package Summervik.Common
```

## Features

### Calendar
- **`DateUtilities`**:
  - Count inclusive days or weekdays (Monday–Friday) between dates.
    - Adjust weekend dates to nearest weekday (Saturday Friday, Sunday Monday)—standard for holiday observation.

- **`UsHolidays`**:
  - Calculates U.S. Federal and common cultural holidays for any year.
  - Returns `null` for years before official recognition (e.g., Juneteenth before 2021).
  - Always-available holidays include New Year's Day, Valentine's Day, and Easter Sunday.
  - Helpers for discovering holidays in date ranges and excluding holidays from weekday counts (business days).

- **`AnnualHolidayCalendar`**:
  - Create annual calendar with custom start date (to support fiscal years).
  - Methods to add Federal holidays or all US Holidays.
  - Add/Remove custom holidays with optional weekend adjustment.
  - Query for holidays by name or date.
  - Read-only, sorted list of holidays.
  
### Extensions
- **`DateOnlyExtensions`** and **`DateTimeExtensions`**:
  - Useful extension methods for start and end of day and adding/counting weekdays.
- **`StreamExtensions`**:
  - Utilities for writing strings directly to a string and reading streams directly into strings.

### Transformations
- **`Cryptography`**:
  - Secure hashing (SHA256 etc.) for strings and files.
  - Authenticated AES-GCM encryption/decryption (modern and secure).

- **`NumbersToWords`**:
  - Converts integers (up to vigintillion), negatives, and decimals to English words.
  - Examples: "123" becomes "one hundred twenty-three", "1.23" bcomes "one point two three" and "0.0000" becomes "zero."

- **`WordsToTime`**:
  - Bidirectional natural-language time conversion.
  - Parse "3 days ago" or "in 2 hours" to a `DateTime`.
  - Convert `DateTime` to "3 days ago" or "in 2 hours."

- **`SocialSecurityNumberFormatter`**:
  - Custom formatter for SSNs: "F" (dashes, default), "N" (digits only).
  - Usage: `string.Format(new SocialSecurityNumberFormatter(), "{0:F}", "123456789")` becomes "123-45-6789".

- **`TemplateTransformation`**:
  - Flexible template engine with multiple placeholder styles:
    - `<data name="Key" />`
    - `{Key}`
    - `[Key]`
    - `#Key#`
  - Case-insensitive keys, recursive nested replacement, and configurable missing key handling (throw or replace).

- **`UnitedStatesPhoneFormatter`**:
  - Formats U.S. phone digits with multiple styles:
    - "F" (default): (123) 456-7890
    - "N": digits only
    - "dots": 123.456.7890
    - "I": +1 (123) 456-7890 (requires exactly 10 digits)
    - "Idots": +1.123.456.7890
  - Handles extensions (≤4 digits), local (7 digits), and standard (10 digits).

- **`WagesByFrequency`**:
  - Converts wage amounts between pay frequencies (hourly, daily, weekly, biweekly, semi-monthly, monthly, etc.).
  - Supports custom work hours and precise leap-year averaging.

### Validators
- **`UnitedStatesPhoneNumber`**:
  - Structural validation by digit count (4, 7, 10, 11 supported).

- **`SocialSecurityNumber`**:
  - Format + plausible issue validation (rejects zeros and reserved areas per current SSA rules).

## Usage Examples

```csharp
// SSN Formatting
string ssn = string.Format(new SocialSecurityNumberFormatter(), "{0:F}", "123456789");
// "123-45-6789"

// Phone Formatting
string phone = string.Format(new UnitedStatesPhoneFormatter(), "{0:I}", "1234567890");
// "+1 (123) 456-7890"

// Relative Time
string words = WordsToTime.ToWords(DateTime.Now.AddDays(-3));
// "3 days ago"

DateTime parsed = WordsToTime.ParseWords("2 hours from now");
// Now + 2 hours

// Template
var data = new Dictionary<string, string> { { "Name", "Alice" } };
string result = TemplateTransformation.TransformCurlyBraces("Hello {Name}!", data);
// "Hello Alice!"

// AnnualHolidayCalendar
var calendar = new AnnualHolidayCalendar(2026)
    .WithFederalUsHolidays()
    .WithHoliday(new DateOnly(2026, 12, 26), "Company Day Off");

bool isHoliday = calendar.IsHoliday(new DateOnly(2026, 7, 3)); // True for observed Independence Day
```

## Contributing
Pull requests welcome! Focus on new utilities, performance improvements, or bug fixes.
