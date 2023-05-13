using System;
using System.Collections.Generic;
using System.Linq;

namespace NovaCore.Web;

public readonly struct Url : IFormattable
{
    public readonly string Protocol;
            
    public readonly string Domain;
            
    public readonly string Path;
            
    public readonly List<WebQuery> Queries;

    public Url(string protocol, string domain, string path, params WebQuery[] queries)
    {
        Protocol = protocol;
        Domain = domain;
        Path = path;
        Queries = queries.ToList();
    }

    public string QueryString => 
        string.Join("&", Queries.Select(query => $"{query.Key}={query.Value}"));
            
    public override string ToString() => $"{Domain}/{Path}?{QueryString}";
    public string ToString(string format, IFormatProvider formatProvider) => ToString();
}