using System;

namespace NovaCore.Web.Server.Interfaces;

public interface IHttpRequest
{
    IHttpHeaders Headers { get; }

    HttpMethods Method { get; }

    string Protocol { get; }

    Uri Uri { get; }

    string[] RequestParameters { get; }

    IHttpPost Post { get; }

    IHttpHeaders QueryString { get; }
}