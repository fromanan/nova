using System.Collections.Generic;
using System.Threading.Tasks;

namespace NovaCore.Web.Server.Interfaces;

public interface IErrorContainer
{
    void Log(string description);

    IEnumerable<string> Errors { get; }

    bool Any { get; }

    Task<IControllerResponse> GetResponse();
}