using System;
using System.Threading.Tasks;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server.Controllers;

public static class Pipeline
{
    private class EmptyPipeline : IPipeline
    {
        public Task<IControllerResponse> Go(Func<Task<IControllerResponse>> injectedTask, IHttpContext context)
        {
            return injectedTask();
        }
    }

    public static IPipeline Empty = new EmptyPipeline();
}