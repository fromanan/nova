using System;
using System.Threading.Tasks;
using NovaCore.Web.Server;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Extensions
{
    public class AnonymousHttpRequestHandler : IHttpRequestHandler
    {
        private readonly Func<IHttpContext, Func<Task>, Task> _method;

        public AnonymousHttpRequestHandler(Func<IHttpContext, Func<Task>, Task> method)
        {
            _method = method;
        }

        public Task Handle(IHttpContext context, Func<Task> next)
        {
            return _method(context, next);
        }
    }
    
    public static class HttpServerExtensions
    {
        public static void Use(this HttpServer server, Func<IHttpContext, Func<Task>, Task> method)
        {
            server.Use(new AnonymousHttpRequestHandler(method));
        }
    }
}