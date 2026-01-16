using Summervik.Common.Transformations;

namespace Summervik.Common.Tests.Transformations;

public class RelativeTimeWordConverterTests
{
    [Theory]
    [InlineData("5 seconds ago", 5, "seconds")]
    [InlineData("1 minute ago", 1, "minute")]
    [InlineData("2 hours ago", 2, "hours")]
    [InlineData("3 days ago", 3, "days")]
    [InlineData("1 week ago", 1, "week")]
    [InlineData("4 months ago", 4, "months")]
    [InlineData("2 years ago", 2, "years")]
    public void ParseWords_SubtractsCorrectly(string input, int expectedAmount, string period)
    {
        DateTime baseTime = new(2026, 1, 1, 12, 0, 0);
        DateTime result = RelativeTimeWordConverter.ParseWords(input, baseTime);

        DateTime expected = period switch
        {
            "seconds" => baseTime.AddSeconds(-expectedAmount),
            "minute" => baseTime.AddMinutes(-expectedAmount),
            "hours" => baseTime.AddHours(-expectedAmount),
            "days" => baseTime.AddDays(-expectedAmount),
            "week" => baseTime.AddDays(-expectedAmount * 7),
            "months" => baseTime.AddMonths(-expectedAmount),
            "years" => baseTime.AddYears(-expectedAmount),
            _ => baseTime
        };

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("5 SECONDS AGO")]
    [InlineData("1 Minute ago")]
    [InlineData("2 HoUrS AgO")]
    public void ParseWords_CaseInsensitive(string input)
    {
        DateTime baseTime = new(2026, 1, 1);
        DateTime result = RelativeTimeWordConverter.ParseWords(input, baseTime);
        Assert.True(result < baseTime); // Ensures subtraction occurred
    }

    [Fact]
    public void ParseWords_NoMatch_ReturnsOriginal()
    {
        DateTime baseTime = DateTime.Now;
        DateTime result = RelativeTimeWordConverter.ParseWords("hello world", baseTime);
        Assert.Equal(baseTime, result);
    }

    [Fact]
    public void ParseWords_InvalidPeriod_Ignores()
    {
        DateTime baseTime = DateTime.Now;
        DateTime result = RelativeTimeWordConverter.ParseWords("5 bananas ago", baseTime);
        Assert.Equal(baseTime, result);
    }

    [Theory]
    [InlineData(-30, "just now")]                    // 30 seconds ago
    [InlineData(-90, "1 minute ago")]
    [InlineData(-3660, "1 hour ago")]
    [InlineData(-90000, "1 day ago")]                 // ~25 hours
    [InlineData(-604800, "1 week ago")]
    [InlineData(-2592000, "1 month ago")]             // ~30 days
    [InlineData(-63072000, "2 years ago")]            // ~2 years
    [InlineData(3600, "in 1 hour")]
    [InlineData(172800, "in 2 days")]
    public void ToWords_VariousDifferences(double secondsOffset, string expected)
    {
        DateTime baseTime = new(2026, 1, 8);
        DateTime target = baseTime.AddSeconds(secondsOffset);
        Assert.Equal(expected, RelativeTimeWordConverter.ToWords(target, baseTime));
    }
}
