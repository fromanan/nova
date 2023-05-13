using System;
using System.Collections.Generic;
using System.Linq;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server.Headers;

/// <summary>
/// A trivial implementation of <see cref="IHttpHeaders"/>
/// that is composed from multiple <see cref="IHttpHeaders"/>.
/// 
/// If value is found in more then one header,
/// Gets the first available value from by the order of the headers
/// given in the c'tor.
/// </summary>
public class CompositeHttpHeaders : IHttpHeaders
{
    private static readonly IEqualityComparer<StringPair> HeaderComparer =
        new KeyValueComparer<string, string, string>(k => k.Key, StringComparer.InvariantCultureIgnoreCase);

    private readonly IEnumerable<IHttpHeaders> _children;

    public CompositeHttpHeaders(IEnumerable<IHttpHeaders> children)
    {
        _children = children;
    }

    public CompositeHttpHeaders(params IHttpHeaders[] children)
    {
        _children = children;
    }

    public bool HasValue(string name)
    {
        return _children.Any(h => h.HasValue(name));
    }
    
    public bool HasValue(string name, string value, StringComparison comparison)
    {
        return _children.Any(h => h.HasValue(name, value));
    }

    private IHttpHeaders GetChildWithName(string name)
    {
        return _children.First(c => c.HasValue(name));
    }

    public string GetByName(string name)
    {
        if (!HasValue(name))
            throw new KeyNotFoundException($"Header {name} was not found in any of the children headers.");
        
        return GetChildWithName(name)[name];
    }

    public bool TryGetByName(string name, out string value)
    {
        value = null;
        
        if (!HasValue(name))
            return false;

        value = GetChildWithName(name)[name];
        return true;
    }

    public IEnumerator<StringPair> GetEnumerator()
    {
        return _children.SelectMany(c => c).Distinct(HeaderComparer).GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}