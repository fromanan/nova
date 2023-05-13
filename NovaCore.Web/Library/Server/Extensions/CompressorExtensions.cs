using System;
using System.Collections.Generic;
using System.Linq;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server.Extensions;

public static class CompressorExtensions
{
    public static ICompressor GetSupportedCompressor(this IEnumerable<ICompressor> compressors, string[] encodings)
    {
        return compressors.FirstOrDefault(c => encodings.Contains(c.Name, StringComparer.InvariantCultureIgnoreCase));
    }
}