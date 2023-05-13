using System.Threading.Tasks;

namespace NovaCore.Web.Server.Interfaces;

public interface IControllerResponse
{
    Task<IHttpResponse> Respond(IHttpContext context, IView view);
}