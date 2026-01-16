using System.Text;

namespace Summervik.Common.Extensions;

public static class StreamExtensions
{
    private static readonly Lock _writeLock = new();

    /// <summary>
    /// Writes a message to the stream if it's writable.
    /// </summary>
    public static void WriteString(this Stream stream, string? message)
    {
        ArgumentNullException.ThrowIfNull(stream, nameof(stream));
        if (!string.IsNullOrEmpty(message) && stream.CanWrite)
        {
            lock (_writeLock)
            {
                stream.Write(Encoding.UTF8.GetBytes(message));
            }
        }
    }

    /// <summary>
    /// Read a stream into a string if the stream is readable.
    /// Will flush the stream before reading and will reset the stream's
    /// position to what it was before the read.
    /// </summary>
    /// <returns>The stream as a string or null if unreadable.</returns>
    public static string? ReadString(this Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream, nameof(stream));
        if (stream.CanRead)
        {
            stream.Flush();
            var pos = stream.Position;
            stream.Position = 0;
            var buffer = new byte[stream.Length];
            _ = stream.Read(buffer, 0, buffer.Length);
            var str = Encoding.UTF8.GetString(buffer);
            stream.Position = pos;
            return str;
        }
        return null;
    }

    /// <summary>
    /// Writes a message asynchronously to the stream if it's writable.
    /// </summary>
    public static ValueTask WriteStringAsync(this Stream stream, string message,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(stream, nameof(stream));
        cancellationToken.ThrowIfCancellationRequested();

        return !string.IsNullOrEmpty(message) && stream.CanWrite
            ? stream.WriteAsync(Encoding.UTF8.GetBytes(message), cancellationToken)
            : default;
    }

    /// <summary>
    /// Read a stream into a string asyncronously if the stream is readable.
    /// Will flush the stream before reading and will reset the stream's
    /// position to what it was before the read.
    /// </summary>
    /// <returns>A ValueTask representing the asyncronous action.
    /// The ValueTask contains a string if the stream is readable,
    /// otherwise contains null.</returns>
    public static async ValueTask<string?> ReadStringAsync(this Stream stream,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(stream, nameof(stream));
        cancellationToken.ThrowIfCancellationRequested();

        if (stream.CanRead)
        {
            await stream.FlushAsync(cancellationToken);
            var pos = stream.Position;
            stream.Position = 0;

            var buffer = new byte[stream.Length];
            _ = await stream.ReadAsync(buffer, 0, buffer.Length);
            var str = Encoding.UTF8.GetString(buffer);
            stream.Position = pos;
            return str;
        }
        return null;
    }
}