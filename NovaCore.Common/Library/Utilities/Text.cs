using System;
using System.Collections.Generic;
using System.Linq;
using NovaCore.Common.Extensions;

namespace NovaCore.Common.Utilities
{
    public static class Text
    {
        private static readonly Random Random = new();
        
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
        }
        
        public static string ByteString(IEnumerable<byte> bytes)
        {
            return bytes.Select(b => b.ToString("X2")).Merge();
        }
        
        public static string Guid()
        {
            return $"{System.Guid.NewGuid()}";
        }
        
        // DateTime.Now:yyyyMMddHHmmssffff?
        public static string Timestamp()
        {
            return $"{DateTime.Now:yyyyMMddHHmmss}";
        }
    }
}