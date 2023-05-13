using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server.Headers;

[DebuggerDisplay("Empty Headers")]
public class EmptyHttpHeaders : IHttpHeaders
{
    public static readonly IHttpHeaders Empty = new EmptyHttpHeaders();

    private static readonly IEnumerable<StringPair> EmptyKeyValuePairs = Array.Empty<StringPair>();

    private EmptyHttpHeaders() { }

    public IEnumerator<StringPair> GetEnumerator()
    {
        return EmptyKeyValuePairs.GetEnumerator();
    }
        
    IEnumerator IEnumerable.GetEnumerator()
    {
        return EmptyKeyValuePairs.GetEnumerator();
    }

    public bool HasValue(string name)
    {
        throw new ArgumentException("EmptyHttpHeaders does not contain any header");
    }
    
    public bool HasValue(string name, string value, StringComparison comparison)
    {
        throw new ArgumentException("EmptyHttpHeaders does not contain any header");
    }
        
    public string GetByName(string name)
    {
        throw new ArgumentException("EmptyHttpHeaders does not contain any header");
    }
        
    public bool TryGetByName(string name, out string value)
    {
        value = null;
        return false;
    }
}