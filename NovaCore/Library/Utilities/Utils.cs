using System;
using System.Collections.Generic;
using System.Linq;
using NovaCore.Extensions;

namespace NovaCore.Utilities
{
    public static class Utils
    {
        private static readonly Random Random = new Random();
        
        public static bool ToBool(object value) => Convert.ToBoolean(Convert.ToInt32(value));

        public static int ToInt(object value) => Convert.ToInt32(value);

        public static long ToLong(object value) => Convert.ToInt64(value);

        public static float ToFloat(object value) => Convert.ToSingle(value);

        public static double ToDouble(object value) => Convert.ToDouble(value);

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
        }
        
        public static string ByteString(IEnumerable<byte> bytes)
        {
            return bytes.Select(b => b.ToString("X2")).Merge();
        }
        
        public static int WrappedClamp(int n, int max, int min = 0)
        {
            return n < min ? max - 1 : n > max ? n % max : n;
        }
    
        // Used for cycling through a list continuously
        public static int CycleList(int index, int delta, int length)
        {
            return WrappedClamp(index + delta, length);
        }

        // TODO: Extend into different generator types
        public static IEnumerable<T> Repeat<T>(Func<T> generator, int n)
        {
            return Enumerable.Range(0, n).Select(x => generator.Invoke());
        }
    }
}