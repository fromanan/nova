using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NovaCore.Common.Extensions;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server.RequestProviders;

internal record MyStreamReader(Stream UnderlyingStream) : IStreamReader, IDisposable
{
    private const int _BUFFER_SIZE = 8096 / 4;

    private readonly byte[] _middleBuffer = new byte[_BUFFER_SIZE];
    
    private int _index;
    
    private int _count;

    private ValueTask<int> GetBufferChunk() => UnderlyingStream.ReadAsync(_middleBuffer.AsMemory(0, _BUFFER_SIZE));

    private async Task ReadBuffer()
    {
        do
        {
            _count = await GetBufferChunk().ContextIndependent();

            // Fix for 100% CPU
            if (_count == 0)
                await Task.Delay(100).ContextIndependent();
        } while (_count == 0);

        _index = 0;
    }

    public async Task<string> ReadLine()
    {
        StringBuilder builder = new(64);

        if (_index == _count)
            await ReadBuffer().ContextIndependent();

        byte readByte = _middleBuffer[_index++];

        while (readByte != '\n' && (builder.Length == 0 || builder[^1] != '\r'))
        {
            builder.Append((char)readByte);

            if (_index == _count)
                await ReadBuffer().ContextIndependent();

            readByte = _middleBuffer[_index++];
        }

        //Debug.WriteLine("Readline : " + sw.ElapsedMilliseconds);

        return builder.ToString(0, builder.Length - 1);
    }

    public async Task<byte[]> ReadBytes(int count)
    {
        byte[] buffer = new byte[count];
        int currentByte = 0;

        // Empty the buffer
        int bytesToRead = Math.Min(_count - _index, count) + _index;
        for (int i = _index; i < bytesToRead; i++)
            buffer[currentByte++] = _middleBuffer[i];

        _index = _count;

        // Read from stream
        while (currentByte < count)
        {
            currentByte += await UnderlyingStream
                .ReadAsync(buffer.AsMemory(currentByte, count - currentByte))
                .ContextIndependent();
        }

        //Debug.WriteLine("ReadBytes(" + count + ") : " + sw.ElapsedMilliseconds);

        return buffer;
    }

    public void Dispose()
    {
        UnderlyingStream?.Dispose();
    }
}