namespace Summervik.Validators;

/// <summary>
/// Utility for validating a phone number.
/// </summary>
public static class UnitedStatesPhoneNumber
{
    /// <summary>
    /// Determines if the structure of the phone number is valid.
    /// </summary>
    /// <param name="phoneNumber">The phone number to validate.</param>
    /// <param name="validCounts">An array of counts that are valid (e.g., 4, 7, 10).</param>
    /// <returns>A boolean indicator of whether the structure is valid.</returns>
    public static bool IsValidStructure(string phoneNumber, params int[] validCounts)
    {
        string numbersOnly = new([.. phoneNumber.ToCharArray().Where(char.IsDigit)]);
        int length = numbersOnly.Length;

        return validCounts.Length > 0
            ? validCounts.Contains(length)
            : (length is 10 or 11);
    }
}
