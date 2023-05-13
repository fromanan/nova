using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NovaCore.Files;
using NovaCore.Web.Extensions;
using RestSharp;
using Logger = NovaCore.Common.Logging.Logger;

namespace NovaCore.Web;

public static class WebDriver
{
    public static readonly Logger Logger = new();
        
    public const string CONTENT_TYPE_FORM = "application/x-www-form-urlencoded";
    
    public const string CONTENT_TYPE_JSON = "application/json";
    
    public const string DEFAULT_CONTENT_TYPE = CONTENT_TYPE_FORM;

    // Used for loading basic (static) webpages or making queries
    public static string Request(string address)
    {
        HttpWebRequest request = (HttpWebRequest) WebRequest.Create(address);
        try
        {
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //Logger.LogInfo("Request successful");
            return RequestToString(response);
        }
        catch (WebException exception)
        {
            HandleWebException(exception);
            return null;
        }
    }

    public static void HandleWebException(WebException exception)
    {
        if (exception.Response is not HttpWebResponse response)
        {
            Logger.LogWarning("Could not parse exception as HttpWebResponse");
            return;
        }
        Logger.LogError(GetStatusCode(response.StatusCode));
    }

    public static string GetStatusCode(HttpStatusCode statusCode)
    {
        return statusCode switch
        {
            HttpStatusCode.BadRequest           => "Request could not be understood by the server (400)",
            HttpStatusCode.Unauthorized         => "Authentication required for request (401)",
            HttpStatusCode.Forbidden            => "Server request refused (403)",
            HttpStatusCode.NotFound             => "Resource not found (404)",
            HttpStatusCode.RequestTimeout       => "Request timed out (408)",
            HttpStatusCode.Gone                 => "Requested webpage is no longer available (410)",
            HttpStatusCode.Moved                => "Page has been moved on server (301)",
            HttpStatusCode.Redirect             => "Client request redirected (302)",
            HttpStatusCode.InternalServerError  => "Client failed to connect due to an internal server error (500)",
            _                                   => $"Uncategorized WebException ({statusCode})"
        };
    }

    public static StreamReader OpenStreamReader(Stream responseStream, string characterSet = null)
    {
        return string.IsNullOrWhiteSpace(characterSet)
            ? new StreamReader(responseStream)
            : new StreamReader(responseStream, Encoding.GetEncoding(characterSet));
    }
        
    public static HtmlDocument CreateDocumentFromFile(string filename)
    {
        return CreateDocument(File.ReadAllText(filename));
    }

    public static HtmlDocument CreateDocument(string body)
    {
        HtmlDocument htmlDoc = new();
        htmlDoc.LoadHtml(body);
        return htmlDoc;
    }

    public static string RequestToString(HttpWebResponse response)
    {
        using Stream receiveStream = response.GetResponseStream();
        using StreamReader reader = OpenStreamReader(receiveStream, response.CharacterSet);
        return reader.ReadToEnd();
    }
        
    public static WebException GenerateException(RestResponse response)
    {
        string message = $"API returned non-success response | {GetStatusCode(response.StatusCode)}";
        WebException exception = new(message);
        exception.Data.Add("StatusCode", response.StatusCode);
        exception.Data.Add("StatusDescription", response.StatusDescription);
        exception.Data.Add("ErrorMessage", response.ErrorMessage);
        exception.Data.Add("Content", response.Content);
        return exception;
    }

    public static HttpWebRequest BuildRequest(string url, string method, string contentType, string authorization, 
        string userAgent, params WebHeader[] extraHeaders)
    {
        // Create the Request
        HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
        request.Method = method;
        request.ContentType = contentType;
        request.Headers.Add("Authorization", authorization);
        request.UserAgent = userAgent;
        request.AddHeaders(extraHeaders);
        return request;
    }
        
    public static RestRequest BuildSharpRequest(string url, Method method, 
        string contentType = DEFAULT_CONTENT_TYPE, string authorization = "", params WebParameter[] parameters)
    {
        RestRequest request = new (url, method);

        // Headers
        request.AddHeader("Content-Type", contentType);
        request.AddHeader("Authorization", authorization);

        // Parameters
        request.AddParameters(parameters);
            
        return request;
    }

    public static async Task<string> ExecuteRequestAsync(RestRequest request, string baseUrl, 
        CancellationTokenSource cancellationTokenSource = null)
    {
        //Logger.LogCustom("REQUEST", $"{baseUrl}{request.Resource}");
        RestClient client = new(baseUrl);
        Logger.LogCustom("REQUEST", request.Formatted(client));
        return await RetrieveResponseAsync(client, request);
    }
        
    public static async Task<string> ExecuteRequestAsync(RestClient client, RestRequest request, 
        CancellationTokenSource cancellationTokenSource = null)
    {
        Logger.LogCustom("REQUEST", $"{client.BuildUri(request)}");
        return await RetrieveResponseAsync(client, request);
    }

    private static async Task<string> RetrieveResponseAsync(RestClient client, RestRequest request, 
        CancellationTokenSource cancellationTokenSource = null)
    {
        cancellationTokenSource ??= new CancellationTokenSource();
        RestResponse response = await client.ExecuteAsync(request, cancellationTokenSource.Token);

        Logger.LogCustom("RESPONSE", response.Formatted());
            
        //Logger.Log(response.Formatted());

        //await client.PostAsync<RestRequest>(request, cancellationTokenSource.Token);
            
        if (response.IsSuccessful)
        {
            return response.Content;
        }

        if (cancellationTokenSource.IsCancellationRequested)
        {
            Logger.LogError("Execution failed (cancellation requested)");
            return null;
        }

        //Logger.Log($"Error {(int) response.StatusCode} ({response.StatusCode}) : {response.StatusDescription}");

        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            Logger.LogError(response.ErrorMessage);
        }

        Logger.LogException(GenerateException(response));

        //throw GenerateException(response);
        return response.Content;
    }

    /// <summary>
    /// Retrieves an API response and deserializes it as a JSON
    /// </summary>
    /// <param name="client"></param>
    /// <param name="request"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private static async Task<T> RetrieveResponseAsync<T>(RestClient client, RestRequest request)
    {
        return Deserialize<T>(await RetrieveResponseAsync(client, request));
    }
        
    /// <summary>
    /// Deserializes a JSON formatted file into an object of class T, wrapper for FileSystem/JsonDeserialize
    /// </summary>
    /// <param name="body"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T Deserialize<T>(string body)
    {
        return FileSystem.Deserialize<T>(body);
    }
}