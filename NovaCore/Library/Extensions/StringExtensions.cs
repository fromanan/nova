using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NovaCore.Library.Extensions
{
    public static class StringExtensions
    {
        #region Substring Retrieval Expressions
        // Return everything after a substring
        public static string After(this string str, string substr)
        {
            return str.Substring(str.IndexOf(substr, StringComparison.Ordinal) + substr.Length);
        }

        public static string Before(this string str, string substr)
        {
            int loc = str.IndexOf(substr, StringComparison.Ordinal);
            return loc > 0 ? str.Substring(0, loc) : string.Empty;
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
                   str.Substring(0, frontSubstr.Length) == frontSubstr &&
                   str.Substring(str.Length - backSubstr.Length, backSubstr.Length) == backSubstr;
        }

        public static bool StartsWith(this string str, string substr)
        {
            return str.Length >= substr.Length && str.Substring(0, substr.Length) == substr;
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
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }

        // https://stackoverflow.com/questions/541954/how-would-you-count-occurrences-of-a-string-actually-a-char-within-a-string
        public static int CountOccurrences(this string str, string substr)
        {
            return str.Select((c, i) => str.Substring(i)).Count(sub => sub.StartsWith(substr));
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
    }
}