using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NovaCore.Common.Extensions;
using RestSharp;

namespace NovaCore.Web.Extensions
{
    public static class RestRequestExtensions
    {
        public static void AddParameters(this RestRequest request, params WebParameter[] parameters)
        {
            foreach (WebParameter parameter in parameters)
            {
                request.AddParameter(parameter.Key, parameter.Value);
            }
        }

        public static string Formatted(this RestRequest request, RestClient restClient)
        {
            StringBuilder builder = new();

            builder.AppendLine($"RestRequest | Uri: {restClient.BuildUri(request)}");
            
            builder.AppendLine($"    Resource: {request.Resource}");
            builder.AppendLine($"    Method: {request.Method}");
            
            int maxNameSize = request.Parameters.Select(h => h.Name).Max(n => n?.Length ?? 0);
            int maxTypeSize = request.Parameters.Select(h => h.Type.ToString()).Max(t => t.Length);
            
            builder.AppendLine("    Parameters:");
            foreach (Parameter parameter in request.Parameters)
            {
                builder.Append($"        {parameter.Name?.PadRight(maxNameSize)}  ");
                string type = $"({parameter.Type.ToString()})";
                builder.AppendLine($"{type.PadRight(maxTypeSize + 2)} : {parameter.Value}");
            }

            return builder.ToString();
        }
        
        public static string Formatted(this RestResponse response)
        {
            StringBuilder builder = new();
            
            builder.AppendLine($"RestResponse | Uri: {response.ResponseUri}");
            
            builder.AppendLine($"    Status Code: {(int)response.StatusCode} ({response.StatusDescription})");
            builder.AppendLine($"    Content: {response.Content}");

            if (response.Headers is null || response.Headers.Count < 1)
            {
                return builder.ToString();
            }
            
            int maxNameSize = response.Headers.Select(h => h.Name).Max(n => n?.Length ?? 0);
            int maxTypeSize = response.Headers.Select(h => h.Type.ToString()).Max(t => t.Length);
            
            builder.AppendLine("    Headers:");
            foreach (HeaderParameter header in response.Headers ?? Array.Empty<HeaderParameter>())
            {
                builder.Append($"        {header.Name?.PadRight(maxNameSize)}  ");
                string type = $"({header.Type.ToString()})";
                builder.AppendLine($"{type.PadRight(maxTypeSize + 2)} : {header.Value}");
            }

            return builder.ToString();
        }
        
        public static RestRequest AddHeader(this RestRequest request, WebHeader header)
        {
            return request.AddHeader(header.Key, header.Value);
        }
        
        public static RestRequest AddHeader(this RestRequest request, KeyValuePair<string, string> header)
        {
            return request.AddHeader(header.Key, header.Value);
        }
        
        public static RestRequest AddHeaders(this RestRequest request, params KeyValuePair<string, string>[] headers)
        {
            foreach (KeyValuePair<string, string> header in headers) 
            {
                request.AddHeader(header.Key, header.Value);
            }

            return request;
        }
        
        public static RestRequest AddHeaders(this RestRequest request, params WebHeader[] headers)
        {
            foreach (WebHeader header in headers) 
            {
                request.AddHeader(header.Key, header.Value);
            }

            return request;
        }

        private const string _COOKIES_KEY = "Cookies";

        public static bool HasHeader(this RestRequest request, WebHeader header)
        {
            return request.Parameters.Exists(new HeaderParameter(header.Key, header.Value));
        }
        
        public static bool HasCookies(this RestRequest request)
        {
            return request.Parameters.Exists(new HeaderParameter(_COOKIES_KEY, null));
        }
        
        public static bool HasCookie(this RestRequest request, KeyValuePair<string, string> cookie)
        {
            return request.Parameters.Exists(new HeaderParameter(cookie.Key, cookie.Value));
        }

        public static WebHeader GetHeader(this RestRequest request, string headerName)
        {
            var parameter = request.Parameters.TryFind(headerName);
            return parameter is null ? null : new WebHeader(parameter.Name, parameter.Value as string);
        }

        public static RestRequest AddCookie(this RestRequest request, KeyValuePair<string, string> cookie)
        {
            if (!request.HasCookies())
            {
                return request.AddHeader(cookie);
            }

            WebHeader cookies = request.GetHeader(_COOKIES_KEY);
            return request.AddOrUpdateHeader(_COOKIES_KEY, $"{cookies.Value};{cookie.Key}={cookie.Value}");
        }
        
        public static RestRequest AddCookies(this RestRequest request, params KeyValuePair<string, string>[] cookiesToAdd)
        {
            string cookiesToAddString = cookiesToAdd.Select(c => $"{c.Key}={c.Value}").Merge(";");
            
            if (!request.HasCookies())
            {
                return request.AddHeader(_COOKIES_KEY, cookiesToAddString);
            }

            WebHeader cookies = request.GetHeader(_COOKIES_KEY);
            return request.AddOrUpdateHeader(_COOKIES_KEY, $"{cookies.Value};{cookiesToAddString}");
        }
    }
}