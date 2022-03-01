using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace NovaCore.Web
{
    public static class Forms
    {
        public static string FormPost(string url, NameValueCollection postData)
        {
            using WebClient webClient = new();
            return Encoding.ASCII.GetString(webClient.UploadValues(url, postData));
        }
    }
}