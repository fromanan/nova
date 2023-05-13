using System;
using System.Collections.Generic;
using System.Linq;
using NovaCore.Common.Extensions;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server.Headers;

public class CookiesStorage : ICookiesStorage
{
    private readonly IStringDict _values;

    public bool Touched { get; private set; }

    private const StringSplitOptions _SPLIT_OPTIONS =
        StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries;

    public CookiesStorage(string cookie)
    {
        _values = cookie
            .Split("; ", _SPLIT_OPTIONS)
            .ToDictionary(s => s.Before('='), s => s.After('='), StringComparer.InvariantCultureIgnoreCase);
    }

    public void Upsert(string key, string value)
    {
        _values[key] = value;
        Touched = true;
    }

    public void Remove(string key)
    {
        if (_values.Remove(key))
            Touched = true;
    }

    public IEnumerator<StringPair> GetEnumerator()
    {
        return _values.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
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
    
    public string ToCookieData()
    {
        return _values.Select(pair => Webtools.FormatHeader("Set-Cookie", pair)).Merge(Environment.NewLine);
    }
}