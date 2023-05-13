using System.Net;

namespace NovaCore.Web.Extensions;

public static class HttpWebRequestExtensions
{
    public static void AddHeaders(this HttpWebRequest request, params WebHeader[] extraHeaders)
    {
        foreach (WebHeader header in extraHeaders)
        {
            request.Headers.Add(header.Key, header.Value);
        }
    }
}