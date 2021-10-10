using System;
using System.Threading.Tasks;
using uhttpsharp;

namespace NovaCore.Web.Library.Interfaces
{
    public interface ICallback
    {
        Task Handler(IHttpContext context, Func<Task> next);
    }
}