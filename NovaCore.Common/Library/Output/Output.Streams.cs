using System.Collections.Generic;
using System.IO;

namespace NovaCore.Common;

public static partial class Output
{
    private static int _streamCount;
        
    public static readonly Dictionary<int, TextWriter> Streams = new();

    public static T GetStream<T>(int streamNumber) where T : TextWriter
    {
        return !Streams.ContainsKey(streamNumber) ? null : Streams[streamNumber] as T;
    }

    public static T OpenStream<T>() where T : TextWriter, new()
    {
        T stream = new T();
        Streams[_streamCount] = stream;
        _streamCount++;
        return stream;
    }
        
    public static StreamWriter OpenStream(string filepath)
    {
        StreamWriter stream = new(filepath);
        Streams[_streamCount] = stream;
        _streamCount++;
        return stream;
    }

    public static void CloseStream(int streamNumber)
    {
        if (!Streams.ContainsKey(streamNumber)) return;
        Streams[streamNumber].Close();
        Streams.Remove(streamNumber);
    }
}