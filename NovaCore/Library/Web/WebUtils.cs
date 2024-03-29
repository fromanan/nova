﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;

namespace NovaCore.Web
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
            
            public List<WebQuery> Queries;

            public Url(string protocol, string domain, string path, params WebQuery[] queries)
            {
                Protocol = protocol;
                Domain = domain;
                Path = path;
                Queries = queries.ToList();
            }

            public static WebQuery Query(string key, string value)
            {
                return new WebQuery(key, value);
            }

            public string QueryString => 
                string.Join("&", Queries.Select(query => $"{query.Key}={query.Value}"));
            
            public override string ToString() => $"{Domain}/{Path}?{QueryString}";
            public string ToString(string format, IFormatProvider formatProvider) => ToString();
        }
        
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
    }
}