using System;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server;

public readonly struct HttpMethodProvider : IHttpMethodProvider
{
    public static readonly IHttpMethodProvider Default = new HttpMethodProviderCache(new HttpMethodProvider());

    public HttpMethods Provide(string name)
    {
        return Enum.Parse<HttpMethods>(name[..1].ToUpper() + name[1..].ToLower());
    }
}