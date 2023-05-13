using System.Text.RegularExpressions;

namespace NovaCore.Common.Extensions;

public static class RegexExtensions
{
    public static string Remove(this Regex regex, string input)
    {
        return regex.Replace(input, "");
    }
}