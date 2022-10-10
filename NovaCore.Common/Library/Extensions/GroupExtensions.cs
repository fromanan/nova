using System.Text.RegularExpressions;

namespace NovaCore.Common.Extensions
{
    public static class GroupExtensions
    {
        public static string GetResultFull(this Match match)
        {
            return match.Groups[0].Value;
        }
        
        public static string GetResult(this Match match)
        {
            return match.Groups[1].Value;
        }
    }
}