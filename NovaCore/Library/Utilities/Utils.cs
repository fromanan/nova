using System;
using System.Collections.Generic;
using System.Linq;
using NovaCore.Library.Extensions;

namespace NovaCore.Library.Utilities
{
    public static class Utils
    {
        private static readonly Random Random = new Random();
        
        public static bool ToBool(object value) => Convert.ToBoolean(Convert.ToInt32(value));

        public static int ToInt(object value) => Convert.ToInt32(value);

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
        }
        
        public static string ByteString(IEnumerable<byte> bytes)
        {
            return bytes.Select(b => b.ToString("X2")).Merge();
        }
    }
}