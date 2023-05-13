using System.IO;
using System.Threading.Tasks;
using NovaCore.Common.Extensions;
using NovaCore.Web.Server.Interfaces;
using NovaCore.Web.Server.Responses;

namespace NovaCore.Web.Server.Handlers;

public class FileHandler : IHttpRequestHandler
{
    public static string DefaultMimeType { get; set; } = "text/plain";
    
    public static string HttpRootDirectory { get; set; }
    
    public static IStringDict MimeTypes { get; private set; } = new StringDict
    {
        { ".css", "text/css" },
        { ".gif", "image/gif" },
        { ".htm", "text/html" },
        { ".html", "text/html" },
        { ".jpg", "image/jpeg" },
        { ".js", "application/javascript" },
        { ".png", "image/png" },
        { ".xml", "application/xml" },
    };

    private static string GetContentType(string path)
    {
        string extension = Path.GetExtension(path) ?? string.Empty;
        return MimeTypes.TryGetValue(extension, out string type) ? type : DefaultMimeType;
    }
    public async Task Handle(IHttpContext context, System.Func<Task> next)
    {
        string requestPath = context.Request.Uri.OriginalString.TrimStart('/');

        string httpRoot = Path.GetFullPath(HttpRootDirectory ?? ".");
        string path = Path.GetFullPath(Path.Combine(httpRoot, requestPath));

        if (!File.Exists(path))
        {
            await next().ContextIndependent();
            return;
        }

        context.Response = new HttpResponse(GetContentType(path), File.OpenRead(path),
            context.Request.Headers.KeepAliveConnection());
    }
}