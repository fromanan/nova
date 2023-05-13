using System.IO;
using System.Threading.Tasks;

namespace NovaCore.Web.Server.Streams;

internal class NotFlushingStream : Stream
{
    private readonly Stream _child;
    
    public override bool CanRead => _child.CanRead;
    
    public override bool CanSeek => _child.CanSeek;

    public override bool CanWrite => _child.CanWrite;
    
    public override long Length => _child.Length;

    public override long Position
    {
        get => _child.Position;
        set => _child.Position = value;
    }

    public override int ReadTimeout
    {
        get => _child.ReadTimeout;
        set => _child.ReadTimeout = value;
    }

    public override int WriteTimeout
    {
        get => _child.WriteTimeout;
        set => _child.WriteTimeout = value;
    }

    public NotFlushingStream(Stream child)
    {
        _child = child;
    }

    public void ExplicitFlush()
    {
        _child.Flush();
    }

    public Task ExplicitFlushAsync()
    {
        return _child.FlushAsync();
    }

    public override void Flush()
    {
        // _child.Flush();
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        return _child.Seek(offset, origin);
    }

    public override void SetLength(long value)
    {
        _child.SetLength(value);
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        return _child.Read(buffer, offset, count);
    }

    public override int ReadByte()
    {
        return _child.ReadByte();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        _child.Write(buffer, offset, count);
    }

    public override void WriteByte(byte value)
    {
        _child.WriteByte(value);
    }
}