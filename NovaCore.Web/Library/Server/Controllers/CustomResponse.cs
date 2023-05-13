using System.Threading.Tasks;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server.Controllers;

public record CustomResponse(IHttpResponse HttpResponse) : IControllerResponse
{
    public Task<IHttpResponse> Respond(IHttpContext context, IView view)
    {
        return Task.FromResult(HttpResponse);
    }
}