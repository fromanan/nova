using System;
using System.Dynamic;
using System.Net;
using NovaCore.Web.Server.Headers;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server;

internal record HttpContext(IHttpRequest Request, EndPoint RemoteEndPoint) : IHttpContext
{
    private readonly ExpandoObject _state = new();

    public IHttpResponse Response { get; set; }

    public ICookiesStorage Cookies { get; }
        = new CookiesStorage(Request.Headers.GetByNameOrDefault("cookie", string.Empty));

    public dynamic State => _state;

    public string[] SupportedEncodings => Request.Headers
        .GetByNameOrDefault<string>("Accept-Encoding", null)?
        .Split(',', StringSplitOptions.RemoveEmptyEntries);
}