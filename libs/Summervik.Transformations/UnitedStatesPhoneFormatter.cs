namespace Summervik.Transformations;

/// <summary>
/// Provides formatting for U.S. phone numbers.
/// </summary>
public sealed class UnitedStatesPhoneFormatter : IFormatProvider, ICustomFormatter
{
    /// <summary>
    /// Converts the value of a specified object to an equivalent string representation 
    /// using specified format and culture-specific formatting information.
    /// </summary>
    /// <param name="format">
    /// A format string containing formatting specifications (case-insensitive).
    /// Supported formats:
    /// - "F" (default): Formatted with parentheses and dashes for 10 digits, e.g., (123) 456-7890
    /// - "N": Digits only (supports 4, 7, or 10 digits)
    /// - "dots": Formatted with dots, e.g., 123.456.7890
    /// - "I": International with country code and parentheses/dashes, e.g., +1 (123) 456-7890 (requires exactly 10 digits)
    /// - "Idots": International with dots, e.g., +1.123.456.7890 (requires exactly 10 digits)
    /// </param>
    /// <param name="arg">An object to format (expected to be a string containing digits).</param>
    /// <param name="formatProvider">An object that supplies format information about the current instance.</param>
    /// <returns>The formatted phone number string.</returns>
    /// <exception cref="FormatException">Thrown if the input has invalid digit count for the format or unsupported format specifier.</exception>
    public string Format(string? format, object? arg, IFormatProvider? formatProvider)
    {
        // Fallback to default formatting if not called via this provider or arg is null
        if (arg is null || formatProvider != this)
        {
            if (arg is IFormattable formattable)
                return formattable.ToString(format, formatProvider);
            return arg?.ToString() ?? string.Empty;
        }

        // Normalize format (case-insensitive, default "F")
        string normalizedFormat = string.IsNullOrWhiteSpace(format) ? "F" : format.ToUpperInvariant();

        // Expect string input
        if (arg is not string inputString)
            throw new FormatException("Phone number formatter expects a string input.");

        // Extract digits
        char[] digits = new char[11]; // Max possible
        int digitCount = 0;

        foreach (char c in inputString)
        {
            if (char.IsDigit(c))
            {
                if (digitCount >= 11)
                    throw new FormatException("Phone number input contains more than 11 digits.");

                digits[digitCount++] = c;
            }
        }

        if (digitCount == 0)
            throw new FormatException("Phone number input contains no digits.");

        string numericString = new(digits, 0, digitCount);

        // Apply format
        return normalizedFormat switch
        {
            "F" => FormatF(numericString),
            "N" => FormatN(numericString),
            "DOTS" => FormatDots(numericString),
            "I" => FormatI(numericString),
            "IDOTS" => FormatIDots(numericString),
            _ => throw new FormatException($"The '{format}' format specifier is invalid. Supported: F, N, dots, I, Idots.")
        };
    }

    private static string FormatF(string numericString)
    {
        return numericString.Length switch
        {
            <= 4 => numericString,
            7 => $"{numericString[..3]}-{numericString[3..]}",
            10 => $"({numericString[..3]}) {numericString[3..6]}-{numericString[6..]}",
            _ => throw new FormatException($"Format 'F' requires 4 or fewer, 7, or 10 digits (found {numericString.Length}).")
        };
    }

    private static string FormatN(string numericString)
    {
        if (numericString.Length is not (<= 4 or 7 or 10))
            throw new FormatException($"Format 'N' requires 4 or fewer, 7, or 10 digits (found {numericString.Length}).");

        return numericString;
    }

    private static string FormatDots(string numericString)
    {
        return numericString.Length switch
        {
            <= 4 => numericString,
            7 => $"{numericString[..3]}.{numericString[3..]}",
            10 => $"{numericString[..3]}.{numericString[3..6]}.{numericString[6..]}",
            _ => throw new FormatException($"Format 'dots' requires 4 or fewer, 7, or 10 digits (found {numericString.Length}).")
        };
    }

    private static string FormatI(string numericString)
    {
        if (numericString.Length != 10)
            throw new FormatException($"Format 'I' requires exactly 10 digits (found {numericString.Length}).");

        return $"+1 ({numericString[..3]}) {numericString[3..6]}-{numericString[6..]}";
    }

    private static string FormatIDots(string numericString)
    {
        if (numericString.Length != 10)
            throw new FormatException($"Format 'Idots' requires exactly 10 digits (found {numericString.Length}).");

        return $"+1.{numericString[..3]}.{numericString[3..6]}.{numericString[6..]}";
    }

    /// <summary>
    /// Returns an object that provides formatting services for the specified type.
    /// </summary>
    public object? GetFormat(Type? formatType) =>
        formatType == typeof(ICustomFormatter) ? this : null;
}
