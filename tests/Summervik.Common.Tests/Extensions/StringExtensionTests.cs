using Summervik.Common.Extensions;
using System.ComponentModel;

namespace Summervik.Common.Tests.Extensions;

public class StringExtensionTests
{
    public enum ColorNoDescription
    {
        None = 0,
        Red = 1,
        Green = 2,
        Blue = 3
    }

    public enum Color
    {
        [Description("Transparent")]
        None = 0,
        [Description("Bright Red")]
        Red = 1,
        [Description("Forest Green")]
        Green = 2,
        [Description("Sky Blue")]
        Blue = 3
    }

    [Flags]
    public enum ColorFlags
    {
        None = 0,
        [Description("Blood Red")]
        Red = 1 << 0,
        [Description("Lime Green")]
        Green = 1 << 1,
        [Description("Deep Sea")]
        Blue = 1 << 2
    }

    [Fact]
    public void ConvertToEnum_NoDescription()
    {
        Assert.Equal(ColorNoDescription.None, "None".ToEnum<ColorNoDescription>());
        Assert.Equal(ColorNoDescription.Red, "Red".ToEnum<ColorNoDescription>());
    }

    [Fact]
    public void ConvertToEnum_WithDescription()
    {
        Assert.Equal(Color.None, "Transparent".ToEnum<Color>());
        Assert.Equal(Color.Green, "Forest Green".ToEnum<Color>());
    }

    [Fact]
    public void ConvertToEnum_Flags()
    {
        Assert.Equal(ColorFlags.None, "".ToEnum<ColorFlags>());
        Assert.Equal(ColorFlags.Red, "Blood Red".ToEnum<ColorFlags>());
        var color = ColorFlags.None | ColorFlags.Red | ColorFlags.Green | ColorFlags.Blue;
        Assert.Equal(color, "Blood Red, Lime Green, Deep Sea".ToEnum<ColorFlags>());
    }
}
