using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server.Handlers;

public class HttpRouter : IHttpRequestHandler
{
    private readonly IDictionary<string, IHttpRequestHandler> _handlers =
        new Dictionary<string, IHttpRequestHandler>(StringComparer.InvariantCultureIgnoreCase);

    public HttpRouter With(string function, IHttpRequestHandler handler)
    {
        _handlers.Add(function, handler);
        return this;
    }

    public Task Handle(IHttpContext context, Func<Task> nextHandler)
    {
        string function = string.Empty;

        if (context.Request.RequestParameters.Length > 0)
        {
            function = context.Request.RequestParameters[0];
        }

        // If route not found, Call next.
        return _handlers.TryGetValue(function, out IHttpRequestHandler value)
            ? value.Handle(context, nextHandler)
            : nextHandler();
    }
}