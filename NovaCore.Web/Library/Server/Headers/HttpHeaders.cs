using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server.Headers;

[DebuggerDisplay("{Count} Headers")]
[DebuggerTypeProxy(typeof(HttpHeadersDebuggerProxy))]
public class HttpHeaders : IHttpHeaders
{
    private readonly IStringDict _values;
    
    internal int Count => _values.Count;

    public HttpHeaders(IStringDict values)
    {
        _values = values;
    }

    public HttpHeaders(IEnumerable<StringPair> values)
    {
        _values = new StringDict(values);
    }

    public bool HasValue(string name)
    {
        return _values.ContainsKey(name);
    }
    
    public bool HasValue(string name, string value, StringComparison comparison)
    {
        return _values.Any(kvp => kvp.Key.Equals(name, Webtools.IGNORE_CASE) 
                                  && kvp.Value.Equals(value, comparison));
    }

    public string GetByName(string name)
    {
        return _values[name];
    }
        
    public bool TryGetByName(string name, out string value)
    {
        return _values.TryGetValue(name, out value);
    }

    public IEnumerator<StringPair> GetEnumerator()
    {
        return _values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}