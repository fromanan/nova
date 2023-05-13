using NovaCore.Common.Interfaces;

namespace NovaCore.Common
{
    public interface INovaApp : INovaInit, INovaValidate, INovaExit, INovaShutdown, INovaSave, INovaLoad, INovaRestart, 
        INovaCancel
    {
    }
}