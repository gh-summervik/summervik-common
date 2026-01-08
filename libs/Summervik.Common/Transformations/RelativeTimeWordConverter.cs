using System.Text.RegularExpressions;

namespace Summervik.Common.Transformations;

public static partial class RelativeTimeWordConverter
{
    private static readonly Regex _timeRefRegex = TimeRegex();

    /// <summary>
    /// Convert text to DateTime.
    /// </summary>
    public static DateTime ParseWords(string timeRef, DateTime? relativePosition = null)
    {
        DateTime result = relativePosition ?? DateTime.Now;

        var match = _timeRefRegex.Match(timeRef.Trim());

        if (match.Success)
        {
            string numberText = match.Groups[1].Value.Trim();
            string periodText = match.Groups[2].Value.ToLowerInvariant().Trim();
            bool isFuture = match.Groups[3].Success &&
                            (match.Groups[3].Value.Equals("from now", StringComparison.OrdinalIgnoreCase) ||
                             match.Groups[3].Value.Equals("in the future", StringComparison.OrdinalIgnoreCase));

            if (int.TryParse(numberText, out int number) && number > 0)
            {
                int amount = isFuture ? number : -number;

                result = periodText switch
                {
                    "second" or "seconds" => result.AddSeconds(amount),
                    "minute" or "minutes" => result.AddMinutes(amount),
                    "hour" or "hours" => result.AddHours(amount),
                    "day" or "days" => result.AddDays(amount),
                    "week" or "weeks" => result.AddDays(amount * 7),
                    "month" or "months" => result.AddMonths(amount),
                    "year" or "years" => result.AddYears(amount),
                    _ => result
                };
            }
        }

        return result;
    }

    /// <summary>
    /// Convert DateTime to text.
    /// </summary>
    public static string ToWords(DateTime target, DateTime? baseTime = null)
    {
        DateTime now = baseTime ?? DateTime.Now;
        TimeSpan diff = now - target;
        bool isFuture = diff.TotalSeconds < 0;
        double totalSeconds = Math.Abs(diff.TotalSeconds);

        if (totalSeconds < 60)
            return isFuture ? "in moments" : "just now";

        if (totalSeconds < 3600) // < 1 hour
            return FormatUnit((int)(totalSeconds / 60), "minute", isFuture);

        if (totalSeconds < 86400) // < 1 day
            return FormatUnit((int)(totalSeconds / 3600), "hour", isFuture);

        if (totalSeconds < 604800) // < 1 week (7 days)
            return FormatUnit((int)(totalSeconds / 86400), "day", isFuture);

        if (totalSeconds < 2592000) // < ~30 days (approx 1 month)
            return FormatUnit((int)(totalSeconds / 604800), "week", isFuture);

        if (totalSeconds < 31536000) // < 1 year (365 days)
        {
            int months = (int)Math.Round(totalSeconds / 2592000); // ~30 days per month
            return FormatUnit(months, "month", isFuture);
        }

        int years = (int)Math.Round(totalSeconds / 31536000);
        return FormatUnit(years, "year", isFuture);
    }

    private static string FormatUnit(int amount, string unit, bool isFuture)
    {
        string plural = amount == 1 ? "" : "s";

        if (isFuture)
            return $"in {amount} {unit}{plural}";

        return $"{amount} {unit}{plural} ago";
    }

    [GeneratedRegex(@"(\d+)\s+([^ ]+)\s*?(ago|from now|in the future)?", RegexOptions.IgnoreCase | RegexOptions.Singleline, "en-US")]
    private static partial Regex TimeRegex();
}