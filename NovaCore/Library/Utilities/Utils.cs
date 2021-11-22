using System;
using System.Linq;

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
    }
}