using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NovaCore.Common.Extensions;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server.Handlers;

public record RestHandler<T>(IRestController<T> Controller, IResponseProvider ResponseProvider) : IHttpRequestHandler
{
    private static async Task<object> GetHandler(IRestController<T> controller, IHttpRequest request)
    {
        return await controller.Get(request);
    }
    
    private static async Task<object> GetItemHandler(IRestController<T> controller, IHttpRequest request)
    {
        return await controller.GetItem(request);
    }
    
    private static async Task<object> CreateHandler(IRestController<T> controller, IHttpRequest request)
    {
        return await controller.Create(request);
    }
    
    private static async Task<object> UpsertHandler(IRestController<T> controller, IHttpRequest request)
    {
        return await controller.Upsert(request);
    }
    
    private static async Task<object> DeleteHandler(IRestController<T> controller, IHttpRequest request)
    {
        return await controller.Delete(request);
    }
    
    private static readonly Dictionary<RestCall, Func<IRestController<T>, IHttpRequest, Task<object>>> RestCallHandlers = new()
    {
        { RestCall.Create(HttpMethods.Get, false),   GetHandler },
        { RestCall.Create(HttpMethods.Get, true),    GetItemHandler },
        { RestCall.Create(HttpMethods.Post, false),  CreateHandler },
        { RestCall.Create(HttpMethods.Put, true),    UpsertHandler },
        { RestCall.Create(HttpMethods.Delete, true), DeleteHandler }
    };

    public async Task Handle(IHttpContext httpContext, Func<Task> next)
    {
        IHttpRequest httpRequest = httpContext.Request;

        RestCall call = new(httpRequest.Method, httpRequest.RequestParameters.Length > 1);

        if (RestCallHandlers.TryGetValue(call, out Func<IRestController<T>, IHttpRequest, Task<object>> handler))
        {
            object value = await handler(Controller, httpRequest).ContextIndependent();
            httpContext.Response = await ResponseProvider.Provide(value);
            return;
        }

        await next().ContextIndependent();
    }

    #region Embedded Types

    private readonly struct RestCall
    {
        private readonly HttpMethods _method;
            
        private readonly bool _entryFull;

        public RestCall(HttpMethods method, bool entryFull)
        {
            _method = method;
            _entryFull = entryFull;
        }

        public static RestCall Create(HttpMethods method, bool entryFull)
        {
            return new RestCall(method, entryFull);
        }

        private bool Equals(RestCall other)
        {
            return _method == other._method && _entryFull.Equals(other._entryFull);
        }

        public override bool Equals(object obj)
        {
            return obj is RestCall call && Equals(call);
        }

        public override int GetHashCode()
        {
            unchecked { return ((int)_method * 397) ^ _entryFull.GetHashCode(); }
        }
    }

    #endregion
}