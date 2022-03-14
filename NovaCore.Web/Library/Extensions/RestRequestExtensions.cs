using System;
using System.Linq;
using System.Text;
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
    }
}