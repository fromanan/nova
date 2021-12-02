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
using Header = System.Collections.Generic.KeyValuePair<string, string>;
using Parameter = System.Collections.Generic.KeyValuePair<string, string>;

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
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    Debug.LogError("Request could not be understood by the server (400)");
                    break;
                case HttpStatusCode.Unauthorized:
                    Debug.LogError("Authentication required for request (401)");
                    break;
                case HttpStatusCode.Forbidden:
                    Debug.LogError("Server request refused (403)");
                    break;
                case HttpStatusCode.NotFound:
                    Debug.LogError("Page not found (404)");
                    break;
                case HttpStatusCode.RequestTimeout:
                    Debug.LogError("Request timed out (408)");
                    break;
                case HttpStatusCode.Gone:
                    Debug.LogError("Requested webpage is no longer available (410)");
                    break;
                case HttpStatusCode.Moved:
                    Debug.LogError("Page has been moved on server (301)");
                    break;
                case HttpStatusCode.Redirect:
                    Debug.LogError("Client request redirected (302)");
                    break;
                case HttpStatusCode.InternalServerError:
                    Debug.LogError("Client failed to connect due to an internal server error (500)");
                    break;
                default:
                    Debug.LogError($"Uncategorized WebException ({response.StatusCode})");
                    break;
            }
        }

        public static StreamReader OpenStreamReader(Stream responseStream, string characterSet = null)
        {
            return String.IsNullOrWhiteSpace(characterSet)
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
            WebException exception = new WebException("API returned non-success response.");
            exception.Data.Add("StatusCode", response.StatusCode);
            exception.Data.Add("StatusDescription", response.StatusDescription);
            exception.Data.Add("ErrorMessage", response.ErrorMessage);
            exception.Data.Add("Content", response.Content);
            return exception;
        }

        public static HttpWebRequest BuildRequest(string url, string method, string contentType, string authorization, 
            string userAgent, params Header[] extraHeaders)
        {
            // Create the Request
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            request.Method = method;
            request.ContentType = contentType;
            request.Headers.Add("Authorization", authorization);
            request.UserAgent = userAgent;
            
            foreach (Header header in extraHeaders)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            return request;
        }
        
        public static RestRequest BuildSharpRequest(string url, Method method, string contentType, string authorization, 
            params Parameter[] parameters)
        {
            RestRequest request = new RestRequest(url, method);

            // Headers
            request.AddHeader("Content-Type", contentType);
            request.AddHeader("Authorization", authorization);

            // Parameters
            foreach (Parameter parameter in parameters)
            {
                request.AddParameter(parameter.Key, parameter.Value);
            }
            
            return request;
        }
        
        public static async Task<string> ExecuteRequest(RestRequest request, string baseUrl)
        {
            RestClient client = new RestClient(baseUrl);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            IRestResponse response = await client.ExecuteAsync(request, cancellationTokenSource.Token);

            //await client.PostAsync<RestRequest>(restRequest);
            
            if (cancellationTokenSource.IsCancellationRequested || response.IsSuccessful)
                return response.Content;
            
            Debug.Log($"{response.StatusCode} : {response.StatusDescription}");
            Debug.LogError(response.ErrorMessage);
            
            Debug.LogException(GenerateException(response));

            //throw WebDriver.GenerateException(response);
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