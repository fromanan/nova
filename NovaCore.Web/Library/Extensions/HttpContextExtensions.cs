using System;
using System.Linq;
using System.Text;
using NovaCore.Common;
using NovaCore.Common.Extensions;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Extensions;

public static class HttpContextExtensions
{
    public static string FormatRequest(this IHttpContext context)
    {
        IHttpRequest request = context.Request;
            
        StringBuilder builder = new();
            
        builder.AppendLine("Request:");
        builder.AppendLine($"Uri: {request.Uri}");
            
        if (request.QueryString is not null)
        {
            builder.Append("Query String: ?");
            builder.AppendLine(request.QueryString.Select(q => $"{q.Key}={q.Value}").Merge("&"));
        }

        if (request.Post.Parsed.Any())
        {
            builder.AppendLine($"Post: {request.Post.Parsed.Select(s => $"  [S] {s.Key} = {s.Value}").MergeNewline()}");
        }
            
        builder.AppendLine(context.Cookies.Select(c => $"  [C] {c.Key} = {c.Value}").MergeNewline());

        builder.AppendLine();
            
        builder.AppendLine(request.Headers.Select(h => $"  [H] {h.Key} = {h.Value}").MergeNewline());
            
        builder.AppendLine();
            
        builder.AppendLine(request.RequestParameters.Select(p => $"  [P] {p}").MergeNewline());
            
        return builder.ToString();
    }
        
    public static string FormatResponse(this IHttpContext context)
    {
        if (context.Response is null)
            return null;
        StringBuilder builder = new();
        builder.AppendLine("Response:");
        //builder.AppendLine(context.Response.WriteBody());
        builder.AppendLine(context.Response.Headers?.Select(h => $"  {h.Key} = {h.Value}").MergeNewline());
        return builder.ToString();
    }
        
    public static void PrintRequest(this IHttpContext context, ConsoleColor color)
    {
        Output.WriteLine(context.FormatRequest(), color:color);
    }

    public static void PrintResponse(this IHttpContext context, ConsoleColor color)
    {
        Output.WriteLine(context.FormatResponse(), color:color);
    }
}