using Summervik.Common.Extensions;

namespace Summervik.Common.Tests.Extensions;

public class StreamExtensionTests
{
    [Fact]
    public void WriteToStream_ReadFromStream_Match()
    {
        const string Msg = "this is my message";
        var stream = new MemoryStream();
        stream.WriteString(Msg);
        var str = stream.ReadString();
        Assert.Equal(Msg, str);
    }

    [Fact]
    public async Task WriteToStream_ReadFromStream_MatchAsync()
    {
        const string Msg = "this is my message";
        var stream = new MemoryStream();
        await stream.WriteStringAsync(Msg);
        var str = await stream.ReadStringAsync();
        Assert.Equal(Msg, str);
    }

    [Fact]
    public void WriteToStream_ReadFromStream_PositionUnchanged()
    {
        const string Msg = "this is my message";
        var stream = new MemoryStream();
        stream.WriteString(Msg);
        var pos = stream.Position;
        var str = stream.ReadString();
        Assert.Equal(pos, stream.Position);
    }

    [Fact]
    public async Task WriteToStream_ReadFromStream_PositionUnchangedAsync()
    {
        const string Msg = "this is my message";
        var stream = new MemoryStream();
        await stream.WriteStringAsync(Msg);
        var pos = stream.Position;
        var str = await stream.ReadStringAsync();
        Assert.Equal(pos, stream.Position);
    }

    [Fact]
    public void ReadFromStream_StreamUnreadable_ReturnsNull()
    {
        const string Msg = "this is my message";
        var stream = new MemoryStream();
        stream.WriteString(Msg);
        stream.Dispose(); // makes it unreadable.
        Assert.Null(stream.ReadString());
    }

    [Fact]
    public async Task ReadFromStreamAsync_StreamUnreadable_ReturnsNullAsync()
    {
        const string Msg = "this is my message";
        var stream = new MemoryStream();
        await stream.WriteStringAsync(Msg);
        stream.Dispose(); // makes it unreadable.
        Assert.Null(await stream.ReadStringAsync());
    }
}
