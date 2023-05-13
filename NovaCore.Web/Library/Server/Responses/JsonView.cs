using System.Threading.Tasks;
using Newtonsoft.Json;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server.Responses;

public class JsonView : IView
{
    public Task<IViewResponse> Render(IHttpContext context, object state)
    {
        return Task.FromResult<IViewResponse>(new JsonViewResponse(JsonConvert.SerializeObject(state)));
    }

    private class JsonViewResponse : IViewResponse
    {
        public JsonViewResponse(string body)
        {
            Body = body;
        }
            
        public string Body { get; }

        public string ContentType => "application/json; charset=utf-8";
    }
}