using System;
using System.Collections.Generic;
using System.Linq;

namespace NovaCore.Web
{
    public struct Url : IFormattable
    {
        public string Protocol;
            
        public string Domain;
            
        public string Path;
            
        public IEnumerable<WebQuery> Queries;

        public Url(string protocol, string domain, string path, params WebQuery[] queries)
        {
            Protocol = protocol;
            Domain = domain;
            Path = path;
            Queries = queries;
        }

        public string QueryString => 
            string.Join("&", Queries.Select(query => $"{query.Key}={query.Value}"));
            
        public override string ToString() => $"{Domain}/{Path}?{QueryString}";
        public string ToString(string format, IFormatProvider formatProvider) => ToString();
    }
}