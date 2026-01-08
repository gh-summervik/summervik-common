namespace Summervik.Common.Validators;

/// <summary>
/// Utility for validating the structure of United States phone numbers.
/// </summary>
public static class UnitedStatesPhoneNumber
{
    /// <summary>
    /// Determines if the structure of the phone number is valid based on digit count.
    /// Strips all non-digit characters and checks if the resulting length matches any provided valid counts.
    /// </summary>
    /// <param name="phoneNumber">The phone number to validate (may include formatting like dashes, spaces, parentheses).</param>
    /// <param name="validCounts">
    /// Optional array of valid digit counts. Common US examples:
    /// - 4: extensions
    /// - 7: local numbers
    /// - 10: standard domestic
    /// - 11: with leading '1' country code (e.g., +1)
    /// If not provided or empty, defaults to 10 or 11 digits.
    /// </param>
    /// <returns>True if the digit length matches a valid count; otherwise false.</returns>
    public static bool IsValidStructure(string? phoneNumber, params int[] validCounts)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return false;

        ReadOnlySpan<char> span = phoneNumber.AsSpan();
        int digitCount = 0;

        foreach (char c in span)
            if (char.IsDigit(c))
                digitCount++;

        // Default to standard US full numbers if no custom counts provided
        if (validCounts is null || validCounts.Length == 0)
            return digitCount is 10 or 11;

        return Array.IndexOf(validCounts, digitCount) >= 0;
    }
}
