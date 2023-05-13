using System;
using System.Threading.Tasks;
using NovaCore.Web.Server.Interfaces;
using NovaCore.Web.Server.Responses;

namespace NovaCore.Web.Server.Controllers;

public record RedirectResponse(Uri NewLocation) : IControllerResponse
{
    public Task<IHttpResponse> Respond(IHttpContext context, IView view)
    {
        StringPair[] headers =
        {
            new("Location", NewLocation.ToString())
        };
        
        return Task.FromResult<IHttpResponse>(
            new HttpResponse(HttpResponseCode.Found, string.Empty, headers, false));
    }
}