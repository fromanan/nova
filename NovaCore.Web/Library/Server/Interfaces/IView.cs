using System.Threading.Tasks;

namespace NovaCore.Web.Server.Interfaces;

public interface IView
{
    Task<IViewResponse> Render(IHttpContext context, object state);
}