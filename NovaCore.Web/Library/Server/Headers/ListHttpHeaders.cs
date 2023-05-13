using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server.Headers;

[DebuggerDisplay("{Count} Headers")]
[DebuggerTypeProxy(typeof(HttpHeadersDebuggerProxy))]
public record ListHttpHeaders(IList<StringPair> Values) : IHttpHeaders
{
    internal int Count => Values.Count;

    public IEnumerable<string> Keys => Values.Select(kvp => kvp.Key);

    public bool HasValue(string name)
    {
        return this.Any(kvp => kvp.Key.Equals(name, Webtools.IGNORE_CASE));
    }
    
    public bool HasValue(string name, string value, StringComparison comparison)
    {
        return this.Any(kvp => kvp.Key.Equals(name, Webtools.IGNORE_CASE) 
                               && kvp.Value.Equals(value, comparison));
    }

    private IEnumerable<string> GetValuesByName(string name) => Values
        .Where(kvp => kvp.Key.Equals(name, Webtools.IGNORE_CASE))
        .Select(kvp => kvp.Value);

    public string GetByName(string name)
    {
        return GetValuesByName(name).First();
    }

    public bool TryGetByName(string name, out string value)
    {
        value = GetValuesByName(name).FirstOrDefault();
        return value != default;
    }

    public IEnumerator<StringPair> GetEnumerator()
    {
        return Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}