using Summervik.Common.Transformations;

namespace Summervik.Common.Tests;

public class TemplateTransformerTests
{
    private class TestData
    {
        public string Name { get; } = "Alice";
        public int Age { get; } = 30;
        public string City { get; } = "Wonderland";
    }

    [Theory]
    [InlineData("{Name} is {Age} years old.", "Alice is 30 years old.")]
    [InlineData("Hello { name }!", "Hello Alice!")] // Case insensitivity
    public void TransformCurlyBraces_BasicReplacement(string template, string expected)
    {
        string result = TemplateTransformer.TransformCurlyBraces(template, new TestData());
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TransformCurlyBraces_MissingKey_ThrowsByDefault()
    {
        string template = "{Missing}";
        Assert.Throws<KeyNotFoundException>(() =>
            TemplateTransformer.TransformCurlyBraces(template, new { Name = "Bob" }));
    }

    [Theory]
    [InlineData("<data name=\"Name\" /> lives in <data name=\"City\" />.", "Alice lives in Wonderland.")]
    [InlineData("<data name=\"name\" />", "Alice")] // Case insensitivity
    public void TransformDataTags_BasicReplacement(string template, string expected)
    {
        string result = TemplateTransformer.TransformDataTags(template, new
        {
            Name = "Alice",
            Age = 30,
            City = "Wonderland"
        });
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("[Name] - [Age]", "Alice - 30")]
    [InlineData("[City]", "Wonderland")]
    public void TransformBrackets_Basic(string template, string expected)
    {
        string result = TemplateTransformer.TransformBrackets(template, new TestData());
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("#Name# - #Age#", "Alice - 30")]
    [InlineData("#City#", "Wonderland")]
    public void TransformPounds_Basic(string template, string expected)
    {
        string result = TemplateTransformer.TransformPoundsSigns(template, new TestData());
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Transform_MissingKey_ReplacesWithEmpty_WhenConfigured()
    {
        string template = "{Name} {Missing} {Age}";

        string result = TemplateTransformer.Transform(
            template: template,
            model: new { Name = "Bob" },
            regex: TemplateTransformer._curlyBraceRegex,
            throwOnMissingKeys: false,
            replaceMissingWithEmpty: true);

        Assert.Equal("Bob  ", result);
    }

    [Fact]
    public void Transform_MissingKey_LeavesPlaceholder_WhenConfigured()
    {
        string template = "{Name} {Missing}";

        string result = TemplateTransformer.Transform(
            template: template,
            model: new { Name = "Bob" },
            regex: TemplateTransformer._curlyBraceRegex,
            throwOnMissingKeys: false,
            replaceMissingWithEmpty: false);

        Assert.Equal("Bob {Missing}", result);
    }

    [Fact]
    public void Transform_Recursive_NestedPlaceholders()
    {
        string template = "Hello {Full}!";
        string result = TemplateTransformer.TransformCurlyBraces(template, new
        {
            First = "Alice",
            Full = "{First} Johnson"
        }, recursive: true);

        Assert.Equal("Hello Alice Johnson!", result);
    }

    [Fact]
    public void Transform_Recursive_MaxIterations_PreventsInfiniteLoop()
    {
        string template = "{A}";

        Assert.Throws<InvalidOperationException>(() =>
            TemplateTransformer.TransformCurlyBraces(template, new
            {
                A = "{B}",
                B = "{A}"
            }, recursive: true, maxIterations: 10));
    }

    [Theory]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("   ", "   ")]
    public void Transform_EmptyOrNullTemplate_ReturnsAsIs(string? template, string expected)
    {
        string result = TemplateTransformer.TransformCurlyBraces(template ?? string.Empty, new TestData());
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Transform_InvalidKeyGroupIndex_Throws()
    {
        Assert.Throws<InvalidOperationException>(() =>
            TemplateTransformer.Transform(
                template: "{Name}",
                model: new TestData(),
                regex: TemplateTransformer._curlyBraceRegex,
                keyGroupIndex: 99));
    }

    [Fact]
    public void Transform_MultiplePlaceholders_SameTemplate()
    {
        string template = "{Name} is {Age} and lives in {City} ({Name} again).";
        string expected = "Alice is 30 and lives in Wonderland (Alice again).";

        string result = TemplateTransformer.TransformCurlyBraces(template, new TestData());
        Assert.Equal(expected, result);
    }
}