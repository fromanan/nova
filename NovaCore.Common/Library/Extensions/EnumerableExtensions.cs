using System;
using System.Collections.Generic;
using System.Linq;

namespace NovaCore.Common.Extensions;

public static class EnumerableExtensions
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
    
    public static void ForEach<T>( this IEnumerable<T> enumerable, Action<T, int> action )
    {
        int i = 0;
        foreach( T item in enumerable )
        {
            action( item, i++ );
        }
    }

    public static void ForEach<T>( this IEnumerable<T> enumerable, Action<T> action )
    {
        foreach( T item in enumerable )
        {
            action( item );
        }
    }

    public static void ForEach<T, S>( this IEnumerable<T> enumerable, Func<T, S> action )
    {
        foreach( T item in enumerable )
        {
            action( item );
        }
    }

    // TODO: Extend into different generator types
    public static IEnumerable<T> Repeat<T>(Func<T> generator, int n)
    {
        return Enumerable.Range(0, n).Select(x => generator.Invoke());
    }
    
    public static string MergeNewline<T>(this IEnumerable<T> enumerable)
    {
        return string.Join("\n", enumerable);
    }
    
    public static string MergeReturn<T>(this IEnumerable<T> enumerable)
    {
        return string.Join("\r\n", enumerable);
    }

    public static IEnumerable<T> Reversed<T>(this IEnumerable<T> enumerable)
    {
        return enumerable.ToArray().Reverse();
    }

    public static bool All(this IEnumerable<bool> enumerable)
    {
        return enumerable.All(b => b);
    }
    
    public static bool Any(this IEnumerable<bool> enumerable)
    {
        return enumerable.Any(b => b);
    }

    public static bool None<T>(this IEnumerable<T> enumerable)
    {
        return !enumerable.Any();
    }

    public static bool Empty<T>(this IEnumerable<T> enumerable)
    {
        return !enumerable.Any();
    }
}