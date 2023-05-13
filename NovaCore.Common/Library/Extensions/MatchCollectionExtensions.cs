using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NovaCore.Common.Extensions;

public static class MatchCollectionExtensions
{
    public static IEnumerable<string> GetResultsFull(this MatchCollection matches)
    {
        return matches.Select(m => m.Groups[0].Value);
    }
        
    public static IEnumerable<string> GetResults(this MatchCollection matches)
    {
        return matches.Select(m => m.Groups[1].Value);
    }
}