using System.IO;
using System.Threading.Tasks;
using NovaCore.Web.Server.Responses;

namespace NovaCore.Web.Server.Interfaces;

public interface IHttpResponse
{
    Task WriteBody(StreamWriter writer);

    /// <summary>
    /// Gets the status line of this http response,
    /// The first line that will be sent to the client.
    /// </summary>
    HttpResponseCode ResponseCode { get; }

    IHttpHeaders Headers { get; }

    bool CloseConnection { get; }
}