using System.Collections.Generic;
using System.IO;

namespace NovaCore.Logging
{
    public static partial class Debug
    {
        private static int streamCount;
        
        public static readonly Dictionary<int, TextWriter> Streams = new Dictionary<int, TextWriter>();

        public static T GetStream<T>(int streamNumber) where T : TextWriter
        {
            return !Streams.ContainsKey(streamNumber) ? null : Streams[streamNumber] as T;
        }

        public static T OpenStream<T>() where T : TextWriter, new()
        {
            T stream = new T();
            Streams[streamCount] = stream;
            streamCount++;
            return stream;
        }
        
        public static StreamWriter OpenStream(string filepath)
        {
            StreamWriter stream = new StreamWriter(filepath);
            Streams[streamCount] = stream;
            streamCount++;
            return stream;
        }

        public static void CloseStream(int streamNumber)
        {
            if (!Streams.ContainsKey(streamNumber)) return;
            Streams[streamNumber].Close();
            Streams.Remove(streamNumber);
        }
    }
}