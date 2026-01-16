using Summervik.Common.Extensions;
using System.ComponentModel;

namespace Summervik.Common.Tests.Extensions;

public class EnumExtensionTests
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

    [Flags]
    public enum ColorFlagsNoDescription
    {
        None = 0,
        Red = 1 << 0,
        Green = 1 << 1,
        Blue = 1 << 2
    }

    [Fact]
    public void GetEnumDescription_WithDescription()
    {
        Assert.Equal("Transparent", Color.None.GetDescription());
        Assert.Equal("Bright Red", Color.Red.GetDescription());
    }

    [Fact]
    public void GetEnumDescription_NoDescription()
    {
        Assert.Equal("None", ColorNoDescription.None.GetDescription());
        Assert.Equal("Blue", ColorNoDescription.Blue.GetDescription());
    }

    [Fact]
    public void GetEnumDescription_SingleFlag()
    {
        Assert.Equal("Deep Sea", ColorFlags.Blue.GetDescription());
    }

    [Fact]
    public void GetEnumDescription_MultiFlag()
    {
        var color = ColorFlags.Blue | ColorFlags.Green;
        Assert.Equal("Lime Green, Deep Sea", color.GetDescription());
    }

    [Fact]
    public void GetEnumDescription_NoDescription_MultiFlag()
    {
        var color = ColorFlagsNoDescription.Red | ColorFlagsNoDescription.Green;
        Assert.Equal("Red, Green", color.GetDescription());
    }
}
