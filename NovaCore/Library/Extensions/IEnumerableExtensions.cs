using System.Collections.Generic;

namespace NovaCore.Library.Extensions
{
    public static class IEnumerableExtensions
    {
        public static string Merge<T>(this IEnumerable<T> enumerable, string separator = "")
        {
            return string.Join(separator, enumerable);
        }
        
        public static string MergeWrap<T>(this IEnumerable<T> enumerable, string start, string end, string separator = ", ")
        {
            return $"{start}{enumerable.Merge($"{end}{separator}{start}")}{end}";
        }

        public static string MergeQuotes<T>(this IEnumerable<T> enumerable, string separator = ", ")
        {
            return $"\"{enumerable.Merge($"\"{separator}\"")}\"";
        }
    }
}