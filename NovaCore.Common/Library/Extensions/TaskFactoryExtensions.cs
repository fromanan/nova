using System.Threading.Tasks;

namespace NovaCore.Common.Extensions;

public static class TaskFactoryExtensions
{
    public static Task GetCompleted(this TaskFactory factory)
    {
        return Task.FromResult<object>(null);
    }
}