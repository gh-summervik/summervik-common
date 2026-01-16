using System.Text;
using System.Text.RegularExpressions;

namespace Summervik.Common.Transformations;

/// <summary>
/// Utility class for converting numbers to English text.
/// </summary>
public static partial class NumbersToWords
{
    private static readonly string[] _zeroToNineteen =
    [
        "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine",
        "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen",
        "seventeen", "eighteen", "nineteen"
    ];

    private static readonly string[] _tens =
    [
        "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety"
    ];

    private static readonly string[] _denominations =
    [
        "", "thousand", "million", "billion", "trillion", "quadrillion",
        "quintillion", "sextillion", "septillion", "octillion", "nonillion",
        "decillion", "undecillion", "duodecillion", "tredecillion",
        "quattuordecillion", "quindecillion", "sexdecillion", "septendecillion",
        "octodecillion", "novemdecillion", "vigintillion"
    ];

    private static readonly Regex _numberRegex = NumberRegex();

    /// <summary>
    /// Convert a string of numbers to English words (supports integers and decimals).
    /// </summary>
    public static string ConvertToWords(string val)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(val, nameof(val));

        val = val.Trim();

        if (!_numberRegex.IsMatch(val))
            throw new ArgumentException("Value provided is not a number.");

        bool isNegative = val[0] == '-';
        if (isNegative)
            val = val[1..];

        // Split into integer and fractional parts
        string[] parts = val.Split('.');
        string integerPart = parts[0];
        string fractionalPart = parts.Length > 1 ? parts[1] : string.Empty;

        // Trim trailing zeros in fractional part (but keep at least one if present)
        fractionalPart = fractionalPart.TrimEnd('0');
        bool hasFraction = fractionalPart.Length > 0;

        string integerWords = ConvertIntegerToWords(integerPart);

        if (!hasFraction)
            return isNegative ? $"negative {integerWords}" : integerWords;

        // Fractional: "point" + each digit spoken individually
        StringBuilder fractionalWords = new();
        foreach (char digitChar in fractionalPart)
        {
            int digit = digitChar - '0';
            fractionalWords.Append(' ').Append(_zeroToNineteen[digit]);
        }

        string result = $"{integerWords} point{fractionalWords}";

        return isNegative ? $"negative {result}" : result;
    }

    private static string ConvertIntegerToWords(string integerStr)
    {
        if (integerStr == "0")
            return "zero";

        if (integerStr.All(c => c == '0'))
            return "zero";

        StringBuilder words = new();

        int groupCount = (int)Math.Ceiling(integerStr.Length / 3.0);
        if (groupCount > _denominations.Length)
            throw new ArgumentException("Number is too large to convert.");

        int denomIndex = 0;
        while (integerStr.Length > 0)
        {
            string groupStr = integerStr.Length > 3 ? integerStr[^3..] : integerStr;
            integerStr = integerStr.Length > 3 ? integerStr[..^3] : string.Empty;

            if (int.TryParse(groupStr, out int groupValue) && groupValue > 0)
            {
                string groupWords = ConvertThreeDigits(groupValue);
                words.Insert(0, $"{groupWords} {_denominations[denomIndex]} ");
            }

            denomIndex++;
        }

        return words.ToString().Trim();
    }

    private static string ConvertThreeDigits(int value)
    {
        if (value == 0)
            return string.Empty;

        if (value < 20)
            return _zeroToNineteen[value];

        if (value < 100)
        {
            int ten = value / 10;
            int one = value % 10;
            return one == 0 ? _tens[ten - 2] : $"{_tens[ten - 2]}-{_zeroToNineteen[one]}";
        }

        int hundred = value / 100;
        int remainder = value % 100;
        string remainderWords = ConvertThreeDigits(remainder);
        return remainder == 0
            ? $"{_zeroToNineteen[hundred]} hundred"
            : $"{_zeroToNineteen[hundred]} hundred {remainderWords}";
    }

    public static string ConvertToWords(int val) => ConvertToWords(val.ToString());
    public static string ConvertToWords(long val) => ConvertToWords(val.ToString());
    public static string ConvertToWords(uint val) => ConvertToWords(val.ToString());
    public static string ConvertToWords(ulong val) => ConvertToWords(val.ToString());
    public static string ConvertToWords(double val) => ConvertToWords(val.ToString("G15"));
    public static string ConvertToWords(decimal val) => ConvertToWords(val.ToString("G29"));
    
    [GeneratedRegex(@"^-?\d+(\.\d+)?$", RegexOptions.Compiled)]
    private static partial Regex NumberRegex();
}