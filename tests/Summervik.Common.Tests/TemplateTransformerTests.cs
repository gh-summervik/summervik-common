using Summervik.Common.Transformations;

namespace Summervik.Common.Tests;

public class TemplateTransformerTests
{
    private readonly Dictionary<string, string> _basicData = new(StringComparer.OrdinalIgnoreCase)
    {
        { "Name", "Alice" },
        { "Age", "30" },
        { "City", "Wonderland" }
    };

    [Theory]
    [InlineData("{Name} is {Age} years old.", "Alice is 30 years old.")]
    [InlineData("Hello { name }!", "Hello Alice!")] // Case insensitivity
    public void TransformCurlyBraces_BasicReplacement(string template, string expected)
    {
        string result = TemplateTransformer.TransformCurlyBraces(template, _basicData);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TransformCurlyBraces_MissingKey_ThrowsByDefault()
    {
        string template = "{Missing}";
        Assert.Throws<KeyNotFoundException>(() =>
            TemplateTransformer.TransformCurlyBraces(template, _basicData));
    }

    [Theory]
    [InlineData("<data name=\"Name\" /> lives in <data name=\"City\" />.", "Alice lives in Wonderland.")]
    [InlineData("<data name=\"name\" />", "Alice")] // Case insensitivity
    public void TransformDataTags_BasicReplacement(string template, string expected)
    {
        string result = TemplateTransformer.TransformDataTags(template, _basicData);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("[Name] - [Age]", "Alice - 30")]
    [InlineData("[City]", "Wonderland")]
    public void TransformBrackets_Basic(string template, string expected)
    {
        string result = TemplateTransformer.TransformBrackets(template, _basicData);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("#Name# - #Age#", "Alice - 30")]
    [InlineData("#City#", "Wonderland")]
    public void TransformPounds_Basic(string template, string expected)
    {
        string result = TemplateTransformer.TransformPoundsSigns(template, _basicData);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Transform_MissingKey_ReplacesWithEmpty_WhenConfigured()
    {
        var data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) { { "Name", "Bob" } };
        string template = "{Name} {Missing} {Age}";

        string result = TemplateTransformer.Transform(
            template: template,
            keyValuePairs: data,
            regex: TemplateTransformer._curlyBraceRegex,
            throwOnMissingKeys: false,
            replaceMissingWithEmpty: true);

        Assert.Equal("Bob  ", result);
    }

    [Fact]
    public void Transform_MissingKey_LeavesPlaceholder_WhenConfigured()
    {
        var data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) { { "Name", "Bob" } };
        string template = "{Name} {Missing}";

        string result = TemplateTransformer.Transform(
            template: template,
            keyValuePairs: data,
            regex: TemplateTransformer._curlyBraceRegex,
            throwOnMissingKeys: false,
            replaceMissingWithEmpty: false);

        Assert.Equal("Bob {Missing}", result);
    }

    [Fact]
    public void Transform_Recursive_NestedPlaceholders()
    {
        var data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "First", "Alice" },
            { "Full", "{First} Johnson" }
        };

        string template = "Hello {Full}!";
        string result = TemplateTransformer.TransformCurlyBraces(template, data, recursive: true);

        Assert.Equal("Hello Alice Johnson!", result);
    }

    [Fact]
    public void Transform_Recursive_MaxIterations_PreventsInfiniteLoop()
    {
        var data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "A", "{B}" },
            { "B", "{A}" }
        };

        string template = "{A}";

        Assert.Throws<InvalidOperationException>(() =>
            TemplateTransformer.TransformCurlyBraces(template, data, recursive: true, maxIterations: 10));
    }

    [Theory]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("   ", "   ")]
    public void Transform_EmptyOrNullTemplate_ReturnsAsIs(string? template, string expected)
    {
        string result = TemplateTransformer.TransformCurlyBraces(template ?? string.Empty, _basicData);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Transform_InvalidKeyGroupIndex_Throws()
    {
        Assert.Throws<InvalidOperationException>(() =>
            TemplateTransformer.Transform(
                template: "{Name}",
                keyValuePairs: _basicData,
                regex: TemplateTransformer._curlyBraceRegex,
                keyGroupIndex: 99));
    }

    [Fact]
    public void Transform_MultiplePlaceholders_SameTemplate()
    {
        string template = "{Name} is {Age} and lives in {City} ({Name} again).";
        string expected = "Alice is 30 and lives in Wonderland (Alice again).";

        string result = TemplateTransformer.TransformCurlyBraces(template, _basicData);
        Assert.Equal(expected, result);
    }
}