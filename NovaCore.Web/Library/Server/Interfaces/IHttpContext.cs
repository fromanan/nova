using System.Net;

namespace NovaCore.Web.Server.Interfaces;

public interface IHttpContext
{
    IHttpRequest Request { get; }

    IHttpResponse Response { get; set; }

    ICookiesStorage Cookies { get; }

    dynamic State { get; }

    EndPoint RemoteEndPoint { get; }
    
    string[] SupportedEncodings { get; }
}