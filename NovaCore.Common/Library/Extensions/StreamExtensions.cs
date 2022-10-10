using System.IO;
using System.Text;

namespace NovaCore.Common.Extensions
{
    public static class StreamExtensions
    {
        public static string GetContent(this Stream stream)
        {
            StreamReader reader = stream.CreateReader();
            return reader.ReadToEnd();
        }

        public static void Reset(this Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
        }

        public static StreamReader CreateReader(this Stream stream, bool keepOpen = true)
        {
            stream.Reset();
            return new StreamReader(stream, Encoding.UTF8, true, 1024, keepOpen);
        }
    }
}