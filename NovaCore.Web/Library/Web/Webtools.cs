using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using NovaCore.Common.Extensions;
using NovaCore.Web.Server.Headers;

namespace NovaCore.Web;

public static class Webtools
{
    private const StringSplitOptions _SPLIT_OPTIONS =
        StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries;
    
    public const StringComparison IGNORE_CASE = StringComparison.InvariantCultureIgnoreCase;
    
    public const string DEFAULT_CONTENT_TYPE = "text/html; charset=utf-8";
    
    public static string Encode(string input) => HttpUtility.HtmlEncode(input);

    public static string Decode(string input) => HttpUtility.HtmlDecode(input);
        
    // https://stackoverflow.com/questions/7578857/how-to-check-whether-a-string-is-a-valid-http-url
    public static bool Validate(string urlString, out Uri result)
    {
        return Uri.TryCreate(urlString, UriKind.Absolute, out result) && 
               (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
    }

    // https://stackoverflow.com/questions/570098/in-c-how-to-check-if-a-tcp-port-is-available/4165374
    public static bool IsPortAvailable(int port)
    {
        // Evaluate current system tcp connections. This is the same information provided
        // by the netstat command line application, just in .Net strongly-typed object
        // form.  We will look through the list, and if our port we would like to use
        // in our TcpClient is occupied, we will set isAvailable to false.
        IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
        TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();
        return tcpConnInfoArray.All(t => t.LocalEndPoint.Port != port) && 
               ipGlobalProperties.GetActiveTcpListeners().All(t => t.Port != port);
    }

    public static bool IsConnected()
    {
        return NetworkInterface.GetIsNetworkAvailable();
    }

    public static ListHttpHeaders GenerateHeaders(string contentType, bool keepAlive, long contentLength)
    {
        return new ListHttpHeaders(CreateDefaultHeaders(contentType, keepAlive, contentLength));
    }
    
    public static ListHttpHeaders GenerateHeaders(string contentType, bool keepAlive, long contentLength,
        IEnumerable<StringPair> extraHeaders)
    {
        return new ListHttpHeaders(CreateDefaultHeaders(contentType, keepAlive, contentLength)
            .Concat(extraHeaders).ToList());
    }

    private static StringPair[] CreateDefaultHeaders(string contentType, bool keepAlive, long contentLength)
    {
        return new[]
        {
            new StringPair("Date", DateTime.UtcNow.ToString("R")),
            new StringPair("Connection", keepAlive ? "Keep-Alive" : "Close"),
            new StringPair("Content-Type", contentType),
            new StringPair("Content-Length", contentLength.ToString(CultureInfo.InvariantCulture)),
        };
    }
    
    public static string FormatPair(StringPair pair)
    {
        return $"{Uri.EscapeDataString(pair.Key)}={Uri.EscapeDataString(pair.Value)}";
    }

    public static string FormatPair(string key, string value)
    {
        return $"{Uri.EscapeDataString(key)}={Uri.EscapeDataString(value)}";
    }

    public static string FormatHeader(string header, string key, string value)
    {
        return $"{header}: {Uri.EscapeDataString(key)}={Uri.EscapeDataString(value)}";
    }
    
    public static string FormatHeader(string header, StringPair pair)
    {
        return $"{header}: {Uri.EscapeDataString(pair.Key)}={Uri.EscapeDataString(pair.Value)}";
    }

    public static StringDict DeserializeDictionary(string body)
    {
        return body.Split('&', _SPLIT_OPTIONS)
            .ToDictionary(s => Uri.UnescapeDataString(s.Before('=')), 
                s => Uri.UnescapeDataString(s.After('=').Replace('+', ' ')), 
                StringComparer.InvariantCultureIgnoreCase);
    }
}