using NovaCore.Web.Server.Headers;

namespace NovaCore.Web.Server.Interfaces;

public interface IHttpPost
{
    byte[] Raw { get; }

    IHttpHeaders Parsed { get; }
}