using System;
using System.Diagnostics;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server;

[DebuggerDisplay("{Method} {OriginalUri,nq}")]
internal record HttpRequest(IHttpHeaders Headers, HttpMethods Method, string Protocol, Uri Uri, string[] RequestParameters,
    IHttpHeaders QueryString, IHttpPost Post) : IHttpRequest
{
    internal string OriginalUri =>
        QueryString is null ? Uri.OriginalString : $"{Uri.OriginalString}?{QueryString.ToUriData()}";
}