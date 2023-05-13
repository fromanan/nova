#nullable enable
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NovaCore.Common.Extensions;

public static class StringExtensions
{
    #region Substring Retrieval Expressions
    // Return everything after a substring
    public static string After(this string str, string substr)
    {
        return str[(str.IndexOf(substr, StringComparison.Ordinal) + substr.Length)..];
    }
    
    public static string After(this string str, char substr)
    {
        return str[(str.IndexOf(substr, StringComparison.Ordinal) + substr)..];
    }

    public static string Before(this string str, string substr)
    {
        int loc = str.IndexOf(substr, StringComparison.Ordinal);
        return loc > 0 ? str[..loc] : string.Empty;
    }
    
    public static string Before(this string str, char substr)
    {
        int loc = str.IndexOf(substr, StringComparison.Ordinal);
        return loc > 0 ? str[..loc] : string.Empty;
    }

    public static string Between(this string str, string frontSubstr, string backSubstr)
    {
        int front = str.IndexOf(frontSubstr, StringComparison.Ordinal) + frontSubstr.Length;
        int back = str.LastIndexOf(backSubstr, StringComparison.Ordinal);
        return str.Substring(front, back - front);
    }

    public static string Remove(this string str, string substr)
    {
        return str.Replace(substr, string.Empty);
    }
        
    public static string RemoveAll(this string str, params string[] substrings)
    {
        return substrings.Aggregate(str, (result, substr) => result.Remove(substr));
    }
    #endregion

    #region Boolean Expressions
    public static bool IsEnclosedBy(this string str, string frontSubstr, string backSubstr)
    {
        return str.Length >= frontSubstr.Length + backSubstr.Length &&
               str[..frontSubstr.Length] == frontSubstr &&
               str.Substring(str.Length - backSubstr.Length, backSubstr.Length) == backSubstr;
    }

    public static bool StartsWith(this string str, string substr)
    {
        return str.Length >= substr.Length && str[..substr.Length] == substr;
    }

    public static bool EndsWith(this string str, string substr)
    {
        return str.Length >= substr.Length &&
               str.Substring(str.Length - substr.Length, substr.Length) == substr;
    }
    #endregion
        
    public static string Enclose(this string str, string front, string back)
    {
        return str.Insert(0, front).Insert(str.Length + 1, back);
    }

    public static void Save(this StringBuilder stringBuilder, string filename)
    {
        File.WriteAllText(filename, stringBuilder.ToString());
    }
        
    public static string Truncate(this string value, int maxChars)
    {
        return value.Length <= maxChars ? value : value[..maxChars] + "...";
    }

    // https://stackoverflow.com/questions/541954/how-would-you-count-occurrences-of-a-string-actually-a-char-within-a-string
    public static int CountOccurrences(this string str, string substr)
    {
        return str.Select((c, i) => str[i..]).Count(sub => sub.StartsWith(substr));
    }
        
    public static int CountOccurrences(this string str, char c)
    {
        return str.Count(ch => ch == c);
    }
        
    // https://stackoverflow.com/questions/5417070/c-sharp-version-of-sql-like/5419544
    // TODO: Doesn't work...
    public static bool Like(this string toSearch, string toFind)
    {
        return new Regex(@"\A" + new Regex(@"\.|\$|\^|\{|\[|\(|\||\)|\*|\+|\?|\\")
            .Replace(toFind, ch => @"\" + ch)
            .Replace('_', '.')
            .Replace("%", ".*") + @"\z", RegexOptions.Singleline).IsMatch(toSearch);
    }
        
    // https://stackoverflow.com/questions/444798/case-insensitive-containsstring
    public static bool Contains(this string str, string substring, StringComparison stringComparison)
    {
        return str?.IndexOf(substring, stringComparison) >= 0;
    }
        
    public static bool HasValue(this string str)
    {
        return !string.IsNullOrEmpty(str);
    }

    public static bool IsNullOrEmpty(this string? str)
    {
        return string.IsNullOrEmpty(str);
    }
    
    public static bool IsNullOrWhiteSpace(this string? str)
    {
        return string.IsNullOrWhiteSpace(str);
    }

    public static string ValueOrDefault(this string? str, string defaultValue = "")
    {
        return (str.IsNullOrEmpty() ? defaultValue : str)!;
    }

    public static string WrapQuotes(this string str)
    {
        return $"\"{str}\"";
    }

    public static (string Front, string Back) SplitAt(this string str, string value,
        StringComparison comparison = StringComparison.Ordinal)
    {
        int index = str.IndexOf(value, comparison);
        string front = str[(index + 1)..];
        string back = str[..index];
        return (front, back);
    }
    
    public static (string Front, string Back) SplitAt(this string str, char value,
        StringComparison comparison = StringComparison.Ordinal)
    {
        int index = str.IndexOf(value, comparison);
        string front = str[(index + 1)..];
        string back = str[..index];
        return (front, back);
    }
}