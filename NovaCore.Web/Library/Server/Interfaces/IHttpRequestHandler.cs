using System;
using System.Threading.Tasks;

namespace NovaCore.Web.Server.Interfaces;

public interface IHttpRequestHandler
{
    Task Handle(IHttpContext context, Func<Task> next);
}