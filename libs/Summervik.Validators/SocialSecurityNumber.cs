using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Summervik.Validators;

/// <summary>
/// Utility for validating United States Social Security Numbers.
/// </summary>
public static class SocialSecurityNumber
{
    /// <summary>
    /// Determines if the strcuture of the social security number is valid.
    /// </summary>
    /// <param name="socialSecurityNumber">The SSN to validate.</param>
    /// <returns>An indicator of whether the structure of the SSN is valid.</returns>
    public static bool IsValidStructure(string socialSecurityNumber)
    {
        if (string.IsNullOrWhiteSpace(socialSecurityNumber))
            return false; 

#pragma warning disable SYSLIB1045 // Convert to 'GeneratedRegexAttribute'.
        Regex unformattedSsnRegex = new(@"^\d{3}-?\d{2}-?\d{4}$");
#pragma warning restore SYSLIB1045 // Convert to 'GeneratedRegexAttribute'.

        return unformattedSsnRegex.IsMatch(socialSecurityNumber);
    }

    /// <summary>
    /// Determines if a Social Security Number is a valid issue.
    /// </summary>
    /// <param name="socialSecurityNumber">The SSN to validate.</param>
    /// <returns>An indicator of whether the SSN is valid.</returns>
    public static bool IsValid(string socialSecurityNumber)
    {
        bool result = false;
        if (IsValidStructure(socialSecurityNumber))
        {
            string numbersOnly = socialSecurityNumber.Replace("-", "");
            ushort area = ushort.Parse(numbersOnly[..3]);
            ushort group = ushort.Parse(numbersOnly[3..5]);
            ushort series = ushort.Parse(numbersOnly[5..]);

            result = (area > 0 && group > 0 && series > 0
                && !_unusedAreas.Any(a => a.low <= area && area <= a.high));
        }

        return result;
    }

    /// <summary>
    /// Gets a collection of unused areas.
    /// </summary>
    public static IEnumerable<int> UnusedAreas
    {
        get
        {
            foreach ((ushort low, ushort high) in _unusedAreas)
                foreach (var area in Enumerable.Range(low, high - low + 1))
                    yield return area;
        }
    }

    /// <summary>
    /// Gets the collection of used areas.
    /// </summary>
    public static IEnumerable<int> UsedAreas => Enumerable.Range(1, 999).Except(UnusedAreas);

    /// <summary>
    /// Unused Areas.
    /// </summary>
    /// <seealso cref="https://www.ssa.gov/employer/stateweb.htm"/>
    private static readonly ReadOnlyCollection<(ushort low, ushort high)> _unusedAreas =
        new([
            (237,246),
            (587,699),
            (750,999)
        ]);
}
