using System;
using System.Collections.Concurrent;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server;

public record HttpMethodProviderCache : IHttpMethodProvider
{
    private readonly ConcurrentDictionary<string, HttpMethods> _cache = new();

    private readonly Func<string, HttpMethods> _childProvide;
    
    public HttpMethodProviderCache(IHttpMethodProvider child)
    {
        _childProvide = child.Provide;
    }
    
    public HttpMethods Provide(string name)
    {
        return _cache.GetOrAdd(name, _childProvide);
    }
}