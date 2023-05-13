using System.Net.Sockets;
using System.Threading.Tasks;
using NovaCore.Common.Extensions;
using NovaCore.Web.Server.Clients;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server.Listeners;

public readonly struct TcpListenerAdapter : IHttpListener
{
    private readonly TcpListener _listener;

    public TcpListenerAdapter(TcpListener listener)
    {
        _listener = listener;
        _listener.Start();
    }
    
    public async Task<IClient> GetClient()
    {
        return new TcpClientAdapter(await _listener.AcceptTcpClientAsync().ContextIndependent());
    }

    public void Dispose()
    {
        _listener.Stop();
    }
}