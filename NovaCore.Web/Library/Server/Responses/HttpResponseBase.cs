using System.IO;
using System.Threading.Tasks;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server.Responses;

public abstract class HttpResponseBase : IHttpResponse
{
    public bool CloseConnection => !Headers.HasValue("Connection", "Keep-Alive");
    
    protected HttpResponseBase(HttpResponseCode code, IHttpHeaders headers)
    {
        ResponseCode = code;
        Headers = headers;
    }

    public abstract Task WriteBody(StreamWriter writer);
    
    public HttpResponseCode ResponseCode { get; }
    
    public IHttpHeaders Headers { get; }
}