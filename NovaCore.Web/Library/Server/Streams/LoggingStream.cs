using System.IO;

namespace NovaCore.Web.Server.Streams;

internal class LoggingStream : Stream
{
    private readonly Stream _child;

    private readonly string _tempFileName = Path.GetTempFileName();
    
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
    
    public LoggingStream(Stream child)
    {
        _child = child;
        //Console.WriteLine($"Logging to {_tempFileName}");
    }

    public override void Flush()
    {
        _child.Flush();
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
        int retVal = _child.Read(buffer, offset, count);

        using FileStream stream = File.Open(_tempFileName, FileMode.Append);
        stream.Seek(0, SeekOrigin.End);
        stream.Write(buffer, offset, retVal);

        return retVal;
    }

    public override int ReadByte()
    {
        int retVal = _child.ReadByte();

        using FileStream stream = File.Open(_tempFileName, FileMode.Append);
        stream.Seek(0, SeekOrigin.End);
        stream.WriteByte((byte)retVal);

        return retVal;
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