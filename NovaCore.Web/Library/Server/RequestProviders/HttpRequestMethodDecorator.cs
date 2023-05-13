using System;
using NovaCore.Web.Server.Headers;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server.RequestProviders;

internal record HttpRequestMethodDecorator(IHttpRequest Child, HttpMethods Method) : IHttpRequest
{
    public IHttpHeaders Headers => Child.Headers;

    public string Protocol => Child.Protocol;

    public Uri Uri => Child.Uri;

    public string[] RequestParameters => Child.RequestParameters;

    public IHttpPost Post => Child.Post;

    public IHttpHeaders QueryString => Child.QueryString;
}