using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using NovaCore.Files;
using NovaCore.Web.Extensions;
using RestSharp;
using Debug = NovaCore.Logging.Debug;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace NovaCore.Web
{
    public static class WebDriver
    {
        // Used to load a scripted/dynamic webpage
        public static HtmlDocument OpenBrowser(string address)
        {
            return LoadWeb(OpenWeb(), address);
        }

        public static bool WaitForPageLoaded(object browser)
        {
            Application.DoEvents();
            return ((WebBrowser) browser).ReadyState == WebBrowserReadyState.Complete;
        }

        public static HtmlWeb OpenWeb() => new HtmlWeb();

        public static HtmlDocument LoadWeb(HtmlWeb web, string address)
        {
            return web.LoadFromBrowser(address, WaitForPageLoaded);
        }

        // Used for loading basic (static) webpages or making queries
        public static string Request(string address)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(address);
            try
            {
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                //Debug.LogInfo("Request successful");
                return RequestToString(response);
            }
            catch (WebException exception)
            {
                HandleWebException(exception);
                return null;
            }
        }

        // TODO: Return response?
        public static void HandleWebException(WebException exception)
        {
            HttpWebResponse response = (HttpWebResponse) exception.Response;
            Debug.LogError(GetStatusCode(response.StatusCode));
        }

        public static string GetStatusCode(HttpStatusCode statusCode)
        {
            switch (statusCode)
            {
                case HttpStatusCode.BadRequest:
                    return "Request could not be understood by the server (400)";
                case HttpStatusCode.Unauthorized:
                    return "Authentication required for request (401)";
                case HttpStatusCode.Forbidden:
                    return "Server request refused (403)";
                case HttpStatusCode.NotFound:
                    return "Page not found (404)";
                case HttpStatusCode.RequestTimeout:
                    return "Request timed out (408)";
                case HttpStatusCode.Gone:
                    return "Requested webpage is no longer available (410)";
                case HttpStatusCode.Moved:
                    return "Page has been moved on server (301)";
                case HttpStatusCode.Redirect:
                    return "Client request redirected (302)";
                case HttpStatusCode.InternalServerError:
                    return "Client failed to connect due to an internal server error (500)";
                default:
                    return $"Uncategorized WebException ({statusCode})";
            }
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
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(body);
            return htmlDoc;
        }

        public static string RequestToString(HttpWebResponse response)
        {
            using (Stream receiveStream = response.GetResponseStream())
            {
                if (receiveStream == null)
                {
                    Debug.LogException("System failed to receive response stream from request");
                    return null;
                }
                using (StreamReader reader = OpenStreamReader(receiveStream, response.CharacterSet))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        
        public static WebException GenerateException(IRestResponse response)
        {
            string message = $"API returned non-success response | {GetStatusCode(response.StatusCode)}";
            WebException exception = new WebException(message);
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
        
        public static RestRequest BuildSharpRequest(string url, Method method, string contentType, string authorization, 
            params WebParameter[] parameters)
        {
            RestRequest request = new RestRequest(url, method);

            // Headers
            request.AddHeader("Content-Type", contentType);
            request.AddHeader("Authorization", authorization);

            // Parameters
            request.AddParameters(parameters);
            
            return request;
        }
        
        public static async Task<string> ExecuteRequest(RestRequest request, string baseUrl)
        {
            Debug.LogCustom("REQUEST", $"{baseUrl}{request.Resource}");
            
            RestClient client = new RestClient(baseUrl);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            IRestResponse response = await client.ExecuteAsync(request, cancellationTokenSource.Token);

            //await client.PostAsync<RestRequest>(request, cancellationTokenSource.Token);

            if (response.IsSuccessful)
            {
                return response.Content;
            }

            if (cancellationTokenSource.IsCancellationRequested)
            {
                Debug.LogError("Execution failed (cancellation requested)");
                return null;
            }

            //Debug.Log($"Error {(int) response.StatusCode} ({response.StatusCode}) : {response.StatusDescription}");

            if (!string.IsNullOrEmpty(response.ErrorMessage))
            {
                Debug.LogError(response.ErrorMessage);
            }

            Debug.LogException(GenerateException(response));

            //throw GenerateException(response);
            return null;
        }

        public static readonly string DefaultContentType = "application/x-www-form-urlencoded";

        #region Deprecated
        
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
                using (WebBrowser browser = CreateBrowser())
                {
                    // Navigate to page
                    browser.Navigate(address);
                    
                    // Wait for page to load
                    while (browser.ReadyState != WebBrowserReadyState.Complete)
                        Application.DoEvents();

                    // Get response
                    response = browser.Document?.DomDocument.ToString();

                    Debug.Log(response);
                }
            });

            return response;
        }
        
        #endregion
    }
}