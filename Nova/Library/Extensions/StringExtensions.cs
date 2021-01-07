using System;

namespace Nova.Library.Extensions
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
            return loc > 0 ? str.Substring(0, loc) : "";
        }

        public static string Between(this string str, string frontSubstr, string backSubstr)
        {
            int front = str.IndexOf(frontSubstr, StringComparison.Ordinal) + frontSubstr.Length;
            int back = str.LastIndexOf(backSubstr, StringComparison.Ordinal);
            return str.Substring(front, back - front);
        }

        public static string Remove(this string str, string substr)
        {
            return str.Replace(substr, "");
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
    }
}