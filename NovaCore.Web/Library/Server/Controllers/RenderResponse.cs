using System.Threading.Tasks;
using NovaCore.Common.Extensions;
using NovaCore.Web.Server.Interfaces;
using NovaCore.Web.Server.Responses;

namespace NovaCore.Web.Server.Controllers;

public record RenderResponse(HttpResponseCode Code, object State) : IControllerResponse
{
    public async Task<IHttpResponse> Respond(IHttpContext context, IView view)
    {
        IViewResponse output = await view.Render(context, State).ContextIndependent();
        return StringHttpResponse.Create(output.Body, Code, output.ContentType);
    }
}