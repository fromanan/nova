using System;
using System.Windows.Forms;
using HtmlAgilityPack;
using NovaCore.Common.Logging;
using NovaCore.Files;
using NovaCore.Web;
using NovaCore.Web.Extensions;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace NovaCore.Forms;

public static class WebDriverForms
{
    public static readonly Logger Logger = new();
        
    // Used to load a scripted/dynamic webpage
    [Obsolete("Html Agility Pack removed support for LoadFromBrowser method, this method is no longer functional")]
    public static HtmlDocument OpenBrowser(string address)
    {
        return LoadWeb(OpenWeb(), address);
    }

    public static bool WaitForPageLoaded(object browser)
    {
        Application.DoEvents();
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
        return WebDriver.CreateDocument(WebDriver.Request(address)).Strip().GetBody().OuterHtml;
    }
        
    // Old method for single-threaded, default browser functionality
    [Obsolete("Outdated function for cleaning full Reddit pages.", false)]
    public static WebBrowser CreateBrowser()
    {
        return new WebBrowser {ScrollBarsEnabled = false, ScriptErrorsSuppressed = true};
    }

    // Vanilla CSharp Browser
    [Obsolete("No longer supported. Use \"OpenBrowser\" method instead.", false)]
    public static string? QueryBrowser(string address)
    {
        string? response = null;

        void RunBrowser()
        {
            using WebBrowser browser = CreateBrowser();
                
            // Navigate to page
            browser.Navigate(address);
                    
            // Wait for page to load
            while (browser.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }

            // Get response
            response = browser.Document?.DomDocument.ToString();

            Logger.Log(response);
        }
            
        // Standard browser requires a single-threaded application run environment
        FileSystem.RunSTA(RunBrowser);

        return response;
    }
}