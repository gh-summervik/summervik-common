using System.Text.RegularExpressions;

namespace Summervik.Common.Validators;

/// <summary>
/// Utility for validating United States Social Security Numbers.
/// </summary>
public static partial class SocialSecurityNumber
{
    /// <summary>
    /// Determines if the structure of the social security number is valid (format only).
    /// Accepts XXX-XX-XXXX or XXXXXXXXX.
    /// </summary>
    public static bool IsValidStructure(string? socialSecurityNumber)
    {
        if (string.IsNullOrWhiteSpace(socialSecurityNumber))
            return false;

        return SsnRegex().IsMatch(socialSecurityNumber);
    }

    /// <summary>
    /// Determines if a Social Security Number is plausibly valid (format + issued area/group/serial rules).
    /// Rejects all-zero segments and known unassigned/reserved area ranges.
    /// </summary>
    public static bool IsValid(string? socialSecurityNumber)
    {
        if (!IsValidStructure(socialSecurityNumber))
            return false;

        ReadOnlySpan<char> digits = socialSecurityNumber!.Replace("-", "");

        ushort area = ushort.Parse(digits[..3]);
        ushort group = ushort.Parse(digits.Slice(3, 2));
        ushort serial = ushort.Parse(digits.Slice(5, 4));

        if (area == 0 || area == 666 || area >= 900 || group == 0 || serial == 0)
            return false;

        return true;
    }

    [GeneratedRegex(@"^\d{3}-?\d{2}-?\d{4}$")]
    private static partial Regex SsnRegex();
}