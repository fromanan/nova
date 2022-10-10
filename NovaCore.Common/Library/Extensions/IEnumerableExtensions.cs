using System;
using System.Collections.Generic;
using System.Linq;

namespace NovaCore.Common.Extensions
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
        
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T element in enumerable)
            {
                action.Invoke(element);
            }
        }
        
        // TODO: Extend into different generator types
        public static IEnumerable<T> Repeat<T>(Func<T> generator, int n)
        {
            return Enumerable.Range(0, n).Select(x => generator.Invoke());
        }
    }
}