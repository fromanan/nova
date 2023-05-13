using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server.Headers;

internal record HttpHeadersDebuggerProxy(IHttpHeaders Real)
{
    [DebuggerDisplay("{Value,nq}", Name = "{Key,nq}")]
    internal class HttpHeader
    {
        private readonly StringPair _header;
        public HttpHeader(StringPair header)
        {
            _header = header;
        }

        public string Value => _header.Value;

        public string Key => _header.Key;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public IEnumerable<HttpHeader> Headers => Real.Select(kvp => new HttpHeader(kvp)).ToArray();
}