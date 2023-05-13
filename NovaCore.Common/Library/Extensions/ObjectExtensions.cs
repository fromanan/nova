using System;
using System.Collections.Generic;

namespace NovaCore.Common.Extensions;

public static class ObjectExtensions
{
    public static T As<T>(this object obj)
    {
        if (!obj.Is<T>())
            throw new InvalidCastException();

        return (T)obj;
    }

    public static bool Is<T>(this object obj)
    {
        Type objType = obj.GetType();
        return objType.IsAssignableTo(typeof(T)) ||
               objType.IsEquivalentTo(typeof(T)) || 
               objType.IsInstanceOfType(typeof(T));
    }

    public static T Cast<T>(this object obj)
    {
        return (T)obj;
    }
    
    public static IEnumerable<T> SingleItemAsEnumerable<T>(this T item)
    {
        yield return item;
    }

    public static string WrapQuotes(this object obj)
    {
        return obj.ToString()?.WrapQuotes() ?? string.Empty;
    }
}