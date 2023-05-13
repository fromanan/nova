using System.Threading.Tasks;
using NovaCore.Web.Server.Responses;

namespace NovaCore.Web.Server.Interfaces;

public interface IResponseProvider
{
    Task<IHttpResponse> Provide(object value, HttpResponseCode responseCode = HttpResponseCode.Ok);
}