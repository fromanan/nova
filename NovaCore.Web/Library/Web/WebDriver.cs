using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using NovaCore.Common;
using NovaCore.Files;
using NovaCore.Web.Extensions;
using RestSharp;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace NovaCore.Web
{
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
                HttpStatusCode.BadRequest => "Request could not be understood by the server (400)",
                HttpStatusCode.Unauthorized => "Authentication required for request (401)",
                HttpStatusCode.Forbidden => "Server request refused (403)",
                HttpStatusCode.NotFound => "Resource not found (404)",
                HttpStatusCode.RequestTimeout => "Request timed out (408)",
                HttpStatusCode.Gone => "Requested webpage is no longer available (410)",
                HttpStatusCode.Moved => "Page has been moved on server (301)",
                HttpStatusCode.Redirect => "Client request redirected (302)",
                HttpStatusCode.InternalServerError => "Client failed to connect due to an internal server error (500)",
                _ => $"Uncategorized WebException ({statusCode})"
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
            WebException exception = new (message);
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

        public static async Task<string> ExecuteRequest(RestRequest request, string baseUrl)
        {
            Logger.LogCustom("REQUEST", $"{baseUrl}{request.Resource}");
            RestClient client = new(baseUrl);
            return await RetrieveResponse(client, request);
        }
        
        public static async Task<string> ExecuteRequest(RestClient client, RestRequest request)
        {
            Logger.LogCustom("REQUEST", $"{client.BuildUri(request)}");
            return await RetrieveResponse(client, request);
        }

        private static async Task<string> RetrieveResponse(RestClient client, RestRequest request)
        {
            CancellationTokenSource cancellationTokenSource = new();
            RestResponse response = await client.ExecuteAsync(request, cancellationTokenSource.Token);

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
            return null;
        }

        #region Deprecated
        
        // Used to load a scripted/dynamic webpage
        [Obsolete("Html Agility Pack removed support for LoadFromBrowser method, this method is no longer functional")]
        public static HtmlDocument OpenBrowser(string address)
        {
            return LoadWeb(OpenWeb(), address);
        }

        public static bool WaitForPageLoaded(object browser)
        {
            System.Windows.Forms.Application.DoEvents();
            return ((WebBrowser) browser).ReadyState == WebBrowserReadyState.Complete;
        }

        public static HtmlWeb OpenWeb() => new();
        
        [Obsolete("Html Agility Pack removed support for LoadFromBrowser method, this method is no longer functional")]
        public static HtmlDocument LoadWeb(HtmlWeb web, string address)
        {
            throw new NotImplementedException();
            //return web.LoadFromBrowser(address, WaitForPageLoaded);
        }
        
        // Currently does nothing, built for reddit page stripping
        [Obsolete("Function only returns body content (does not clean page).", false)]
        public static string CreateAndParse(string address)
        {
            return CreateDocument(Request(address)).Strip().GetBody().OuterHtml;
        }
        
        // Old method for single-threaded, default browser functionality
        [Obsolete("Outdated function for cleaning full Reddit pages.", false)]
        public static WebBrowser CreateBrowser()
        {
            return new WebBrowser {ScrollBarsEnabled = false, ScriptErrorsSuppressed = true};
        }

        // Vanilla CSharp Browser
        [Obsolete("No longer supported. Use \"OpenBrowser\" method instead.", false)]
        public static string QueryBrowser(string address)
        {
            string response = null;
            
            // Standard browser requires a single-threaded application run environment
            FileSystem.RunSTA(() =>
            {
                using WebBrowser browser = CreateBrowser();
                
                // Navigate to page
                browser.Navigate(address);
                    
                // Wait for page to load
                while (browser.ReadyState != WebBrowserReadyState.Complete)
                {
                    System.Windows.Forms.Application.DoEvents();
                }

                // Get response
                response = browser.Document?.DomDocument.ToString();

                Logger.Log(response);
            });

            return response;
        }
        
        #endregion
    }
}