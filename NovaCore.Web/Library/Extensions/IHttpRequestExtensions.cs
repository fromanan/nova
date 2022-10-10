using System.Linq;
using uhttpsharp;

namespace NovaCore.Web.Extensions
{
    public static class IHttpRequestExtensions
    {
        public static bool HasParameter(this IHttpRequest request, string parameter)
        {
            return request.RequestParameters.Contains(parameter);
        }
        
        public static bool HasHeader(IHttpRequest request, string header)
        {
            return request.Headers.Any(h => h.Key == header);
        }

        public static bool HasHeaderWithValue(this IHttpRequest request, string headerName, string value)
        {
            return request.Headers.TryGetByName(headerName, out string header) && header.Contains(value);
        }

        public static bool UriContains(this IHttpRequest request, string value)
        {
            return request.Uri?.ToString().Contains(value) ?? false;
        }
    }
}