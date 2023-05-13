using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NovaCore.Common.Extensions;
using NovaCore.Web.Server.Headers;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server.RequestProviders;

public class HttpRequestProvider : IHttpRequestProvider
{
    private static readonly char[] Separators = { '/' };

    public async Task<IHttpRequest> Provide(IStreamReader reader)
    {
        // parse the http request
        if (await reader.ReadLine().ContextIndependent() is not { } request)
            return null;

        if (SplitRequest(request) is not ({ } defaultMethod, { } url, { } httpProtocol))
            return null;

        IHttpHeaders queryString = GetQueryStringData(ref url);
        
        Uri uri = new(url, UriKind.Relative);

        IHttpHeaders headers = await ReadHeaders(reader);
        
        IHttpPost post = await GetPostData(reader, headers).ContextIndependent();

        HttpMethods httpMethod = GetMethod(headers, defaultMethod);

        string[] parameters = uri.OriginalString.Split(Separators, StringSplitOptions.RemoveEmptyEntries);
        
        return new HttpRequest(headers, httpMethod, httpProtocol, uri, parameters, queryString, post);
    }

    private static IHttpHeaders GetQueryStringData(ref string url)
    {
        if (!url.Contains('?'))
            return EmptyHttpHeaders.Empty;
        (url, string queryString) = url.SplitAt('?');
        return new QueryStringHttpHeaders(queryString);
    }

    private static async Task<IHttpPost> GetPostData(IStreamReader streamReader, IHttpHeaders headers)
    {
        if (headers.TryGetByName("content-length", out int postContentLength) && postContentLength > 0)
            return await HttpPost.Create(streamReader, postContentLength).ContextIndependent();

        return EmptyHttpPost.Empty;
    }

    private static StringPair SplitHeader(string header)
    {
        (string key, string value) = header.SplitAt(": ");
        return new StringPair(key, value);
    }
    
    private static (string DefaultMethod, string Url, string HttpProtocol) SplitRequest(string request)
    {
        int firstSpace = request.IndexOf(' ');
        int lastSpace = request.LastIndexOf(' ');
        return (request[..firstSpace], request.Substring(firstSpace + 1, lastSpace - firstSpace - 1),
            request[(lastSpace + 1)..]);
    }

    private static async Task<IHttpHeaders> ReadHeaders(IStreamReader reader)
    {
        List<StringPair> headersRaw = new();

        string line = string.Empty;
        
        async Task<bool> GetLine()
        {
            line = await reader.ReadLine().ContextIndependent();
            return line.HasValue();
        }

        while (await GetLine())
            headersRaw.Add(SplitHeader(line));

        return new HttpHeaders(headersRaw);
    }

    private static HttpMethods GetMethod(IHttpHeaders headers, string defaultMethod)
    {
        return HttpMethodProvider.Default.Provide(headers.GetByNameOrDefault("_method", defaultMethod));
    }
}