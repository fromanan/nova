using System.Collections.Generic;

namespace NovaCore.Library.Extensions
{
    public static class IEnumerableExtensions
    {
        public static string Merge<T>(this IEnumerable<T> enumerable, string separator = "")
        {
            return string.Join(separator, enumerable);
        }
    }
}