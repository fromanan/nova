using System;
using System.Threading.Tasks;

namespace NovaCore.Web.Server.Interfaces;

public interface IHttpListener : IDisposable
{
    Task<IClient> GetClient();
}