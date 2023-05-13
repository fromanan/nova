using System.Threading.Tasks;
using NovaCore.Common.Extensions;
using NovaCore.Web.Extensions;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server.RequestProviders;

public record HttpRequestProviderMethodOverrideDecorator(IHttpRequestProvider Child) : IHttpRequestProvider
{
    public async Task<IHttpRequest> Provide(IStreamReader streamReader)
    {
        if (await Child.Provide(streamReader).ContextIndependent() is not { } childValue)
            return null;
        
        if (!childValue.HasHeader("X-HTTP-Method-Override"))
            return childValue;

        string methodName = childValue.Headers.GetByName("X-HTTP-Method-Override");
        return new HttpRequestMethodDecorator(childValue, HttpMethodProvider.Default.Provide(methodName));
    }
}