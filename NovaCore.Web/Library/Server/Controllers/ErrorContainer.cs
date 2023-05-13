using System.Collections.Generic;
using System.Threading.Tasks;
using NovaCore.Common.Extensions;
using NovaCore.Web.Server.Interfaces;
using NovaCore.Web.Server.Responses;

namespace NovaCore.Web.Server.Controllers;

public class ErrorContainer : IErrorContainer
{
    private readonly IList<string> _errors = new List<string>();
        
    public IEnumerable<string> Errors => _errors;
        
    public bool Any => _errors.Count != 0;

    public void Log(string description)
    {
        _errors.Add(description);
    }

    public Task<IControllerResponse> GetResponse()
    {
        return Task.FromResult<IControllerResponse>(new RenderResponse(HttpResponseCode.MethodNotAllowed,
            new { Message = _errors.Merge(", ") }));
    }
}