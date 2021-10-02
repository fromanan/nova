using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using Query = System.Collections.Generic.KeyValuePair<string, string>;

namespace NovaCore.Library.Web
{
    public class WebUtils
    {
        public enum WebExtensions
        {
            HTML, 
            PHP, 
            JSON
        }
        
        public struct Url : IFormattable
        {
            public string Protocol;
            
            public string Domain;
            
            public string Path;
            
            public List<Query> Queries;

            public Url(string protocol, string domain, string path, params Query[] queries)
            {
                Protocol = protocol;
                Domain = domain;
                Path = path;
                Queries = queries.ToList();
            }

            public static Query Query(string key, string value)
            {
                return new Query(key, value);
            }

            public string QueryString => 
                string.Join("&", Queries.Select(query => $"{query.Key}={query.Value}"));
            
            public override string ToString() => $"{Domain}/{Path}?{QueryString}";
            public string ToString(string format, IFormatProvider formatProvider) => ToString();
        }
        
        public static string Encode(string input) => HttpUtility.HtmlEncode(input);

        public static string Decode(string input) => HttpUtility.HtmlDecode(input);
        
        public static Uri Validate(string address)
        {
            bool success = Uri.TryCreate(address, UriKind.Absolute, out Uri uriResult)
                           && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            return success ? uriResult : null;
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
    }
}