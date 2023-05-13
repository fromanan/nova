using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using NovaCore.Common.Extensions;
using NovaCore.Web.Server.Clients;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server.Listeners;

public record ListenerSslDecorator(IHttpListener Child, X509Certificate Certificate) : IHttpListener
{
    public async Task<IClient> GetClient()
    {
        return new ClientSslDecorator(await Child.GetClient().ContextIndependent(), Certificate);
    }

    public void Dispose()
    {
        Child.Dispose();
    }
}