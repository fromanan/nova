using System;
using System.Threading.Tasks;
using NovaCore.Web.Server.Interfaces;
using NovaCore.Web.Server.Responses;

namespace NovaCore.Web.Server.Controllers;

public static class Response
{
    public static Task<IControllerResponse> Create(IControllerResponse response)
    {
        return Task.FromResult(response);
    }

    public static Task<IControllerResponse> Custom(IHttpResponse httpResponse)
    {
        return Create(new CustomResponse(httpResponse));
    }

    public static Task<IControllerResponse> Render(HttpResponseCode code, object state)
    {
        return Create(new RenderResponse(code, state));
    }

    public static Task<IControllerResponse> Render(HttpResponseCode code)
    {
        return Create(new RenderResponse(code, null));
    }

    public static Task<IControllerResponse> Redirect(Uri newLocation)
    {
        return Create(new RedirectResponse(newLocation));
    }
}