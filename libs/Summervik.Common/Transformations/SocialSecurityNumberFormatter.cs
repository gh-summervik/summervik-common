namespace Summervik.Common.Transformations;

/// <summary>
/// Provides formatting for U.S. Social Security Numbers.
/// </summary>
public sealed class SocialSecurityNumberFormatter : IFormatProvider, ICustomFormatter
{
    /// <summary>
    /// Converts the value of a specified object to an equivalent string representation 
    /// using specified format and culture-specific formatting information.
    /// </summary>
    /// <param name="format">
    /// A format string containing formatting specifications.
    /// Supported formats (case-insensitive):
    /// - "F" (default): XXX-XX-XXXX (dashes)
    /// - "N": XXXXXXXXX (numbers only)
    /// </param>
    /// <param name="arg">An object to format (expected to be a string containing digits).</param>
    /// <param name="formatProvider">An object that supplies format information about the current instance.</param>
    /// <returns>The formatted SSN string.</returns>
    /// <exception cref="FormatException">Thrown if the input does not contain exactly 9 digits or if the format is unsupported.</exception>
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
            throw new FormatException("Social Security Number formatter expects a string input.");

        // Extract exactly 9 digits
        char[] digits = new char[9];
        int digitCount = 0;

        foreach (char c in inputString)
        {
            if (char.IsDigit(c))
            {
                if (digitCount >= 9)
                    throw new FormatException("SSN input contains more than 9 digits.");

                digits[digitCount++] = c;
            }
        }

        if (digitCount != 9)
            throw new FormatException("SSN requires exactly 9 digits.");

        string numericString = new(digits);

        // Apply format
        return normalizedFormat switch
        {
            "F" => $"{numericString[..3]}-{numericString.Substring(3, 2)}-{numericString.Substring(5, 4)}",
            "N" => numericString,
            _ => throw new FormatException($"The '{format}' format specifier is invalid. Supported: F, N.")
        };
    }

    /// <summary>
    /// Returns an object that provides formatting services for the specified type.
    /// </summary>
    public object? GetFormat(Type? formatType) =>
        formatType == typeof(ICustomFormatter) ? this : null;
}