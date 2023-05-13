using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server.Headers;

[DebuggerDisplay("{Count} Query String Headers")]
[DebuggerTypeProxy(typeof(HttpHeadersDebuggerProxy))]
internal class QueryStringHttpHeaders : IHttpHeaders
{
    private readonly HttpHeaders _child;

    internal int Count { get; }

    public QueryStringHttpHeaders(string query)
    {
        StringDict values = Webtools.DeserializeDictionary(query);
        Count = values.Count;
        _child = new HttpHeaders(values);
    }

    public bool HasValue(string name)
    {
        return _child.HasValue(name);
    }

    public bool HasValue(string name, string value, StringComparison comparison)
    {
        return _child.HasValue(name, value, comparison);
    }

    public string GetByName(string name)
    {
        return _child.GetByName(name);
    }
        
    public bool TryGetByName(string name, out string value)
    {
        return _child.TryGetByName(name, out value);
    }
    
    public IEnumerator<StringPair> GetEnumerator()
    {
        return _child.GetEnumerator();
    }
        
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}