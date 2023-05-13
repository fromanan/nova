using System;
using System.Linq;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Extensions;

public static class HttpRequestExtensions
{
    public static bool HasParameter(this IHttpRequest request, string parameter)
    {
        return request.RequestParameters.Contains(parameter);
    }
        
    public static bool HasHeader(this IHttpRequest request, string header)
    {
        return request.Headers.HasValue(header);
    }

    public static bool HasHeaderWithValue(this IHttpRequest request, string headerName, string value,
        StringComparison stringComparison = StringComparison.InvariantCultureIgnoreCase)
    {
        return request.Headers.HasValue(headerName, value, stringComparison);
    }

    public static bool UriContains(this IHttpRequest request, string value,
        StringComparison stringComparison = StringComparison.InvariantCultureIgnoreCase)
    {
        return request.Uri is not null && request.Uri.ToString().Contains(value, stringComparison);
    }

    public static bool KeepAlive(this IHttpRequest request)
    {
        return request.Headers.KeepAliveConnection();
    }
}