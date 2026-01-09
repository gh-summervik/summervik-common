using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleTo("Summervik.Common.Tests")]
namespace Summervik.Common.Transformations;

public static partial class TemplateTransformer
{
    internal static readonly Regex _dataTagRegex = DataTagRegex();
    internal static readonly Regex _curlyBraceRegex = CurlyBraceRegex();
    internal static readonly Regex _bracketRegex = BracketRegex();
    internal static readonly Regex _poundRegex = PoundSignRegex();

    public const string DataTagExpression = @"<data name=""([^\""]+)""[\s+]?\/>";
    public const string CurlyBraceExpression = @"\{[\s+]?([^\}]+)\}";
    public const string BracketExpression = @"\[[\s+]?([^\]]+)\]";
    public const string PoundExpression = @"#[\s+]?([^#]+)#";

    /// <summary>
    /// Transforms a template by replacing placeholders with values from a dictionary.
    /// </summary>
    /// <param name="template">The template string.</param>
    /// <param name="model">A class containing properties corresponding to template keys.</param>
    /// <param name="regex">The regex to match placeholders.</param>
    /// <param name="keyGroupIndex">The capture group index containing the key (default 1).</param>
    /// <param name="throwOnMissingKeys">Throw if key not found (default true).</param>
    /// <param name="replaceMissingWithEmpty">Replace missing keys with empty string if not throwing (default true).</param>
    /// <param name="recursive">Enable recursive replacement for nested placeholders (default false).</param>
    /// <param name="maxIterations">Maximum recursive iterations (default 100).</param>
    /// <returns>The transformed string.</returns>
    public static string Transform<T>(
        string template,
        T model,
        Regex regex,
        int keyGroupIndex = 1,
        bool throwOnMissingKeys = true,
        bool replaceMissingWithEmpty = true,
        bool recursive = false,
        int maxIterations = 100) where T : class
    {
        if (string.IsNullOrWhiteSpace(template))
            return template ?? string.Empty;

        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(regex);

        if (keyGroupIndex < 0)
            throw new ArgumentException("keyGroupIndex cannot be negative.");

        var dictionary = ToDictionary(model);

        string result = template;
        int iteration = 0;

        do
        {
            string previous = result;

            result = regex.Replace(result, match =>
            {
                if (keyGroupIndex >= match.Groups.Count)
                    throw new InvalidOperationException("keyGroupIndex out of range for the regex.");

                string key = match.Groups[keyGroupIndex].Value.Trim();

                if (dictionary.TryGetValue(key, out string? value))
                    return value ?? string.Empty;

                if (throwOnMissingKeys)
                    throw new KeyNotFoundException($"Key '{key}' not found in dictionary.");

                return replaceMissingWithEmpty ? string.Empty : match.Value;
            });

            if (!recursive || result == previous)
                break;

            iteration++;
            if (iteration >= maxIterations)
                throw new InvalidOperationException("Maximum recursive iterations reached - possible circular reference.");

        } while (true);

        return result;
    }

    public static string TransformDataTags<T>(string template, T model, bool recursive = false, int maxIterations = 100) where T : class =>
        Transform(template, model, _dataTagRegex, 1, recursive: recursive, maxIterations: maxIterations);

    public static string TransformCurlyBraces<T>(string template, T model, bool recursive = false, int maxIterations = 100) where T : class =>
        Transform(template, model, _curlyBraceRegex, 1, recursive: recursive, maxIterations: maxIterations);

    public static string TransformBrackets<T>(string template, T model, bool recursive = false, int maxIterations = 100) where T : class =>
        Transform(template, model, _bracketRegex, 1, recursive: recursive, maxIterations: maxIterations);

    public static string TransformPoundsSigns<T>(string template, T model, bool recursive = false, int maxIterations = 100) where T : class =>
        Transform(template, model, _poundRegex, 1, recursive: recursive, maxIterations: maxIterations);

    [GeneratedRegex(DataTagExpression, RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
    private static partial Regex DataTagRegex();
    [GeneratedRegex(CurlyBraceExpression, RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
    private static partial Regex CurlyBraceRegex();
    [GeneratedRegex(BracketExpression, RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
    private static partial Regex BracketRegex();
    [GeneratedRegex(PoundExpression, RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
    private static partial Regex PoundSignRegex();
    private static Dictionary<string, string> ToDictionary<T>(T model) where T : class
    {
        ArgumentNullException.ThrowIfNull(model);

        var dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase))
        {
            object? value = property.GetValue(model);
            dictionary[property.Name] = value?.ToString() ?? string.Empty;
        }

        return dictionary;
    }

}
