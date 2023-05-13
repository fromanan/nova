using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server.Extensions;

public static class RequestHandlersAggregateExtensions
{
    public static Func<IHttpContext, Task> Aggregate(this IList<IHttpRequestHandler> handlers)
    {
        return handlers.Aggregate(0);
    }

    private static Func<IHttpContext, Task> Aggregate(this IList<IHttpRequestHandler> handlers, int index)
    {
        if (index == handlers.Count)
        {
            return null;
        }

        IHttpRequestHandler currentHandler = handlers[index];
        Func<IHttpContext, Task> nextHandler = handlers.Aggregate(index + 1);

        return context => currentHandler.Handle(context, () => nextHandler(context));
    }
}