using Summervik.Common.Transformations;

namespace Summervik.Common.Tests.Transformations;

public class NumberToWordsTests
{
    [Theory]
    [InlineData("123", "one hundred twenty-three")]
    [InlineData("123.45", "one hundred twenty-three point four five")]
    [InlineData("0.001", "zero point zero zero one")]
    [InlineData("1000000.99", "one million point nine nine")]
    [InlineData("-5.67", "negative five point six seven")]
    [InlineData("0.0", "zero")]
    [InlineData("1.2300", "one point two three")]
    [InlineData("1.23001", "one point two three zero zero one")]
    public void ConvertToWords_Decimals(string input, string expected)
    {
        Assert.Equal(expected, NumbersToWords.ConvertToWords(input));
    }
}
