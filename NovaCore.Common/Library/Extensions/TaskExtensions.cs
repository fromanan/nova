using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace NovaCore.Common.Extensions;

public static class TaskExtensions
{
    public static ConfiguredTaskAwaitable ContextIndependent(this Task task)
    {
        return task.ConfigureAwait(false);
    }
    
    public static ConfiguredTaskAwaitable<T> ContextIndependent<T>(this Task<T> task)
    {
        return task.ConfigureAwait(false);
    }
    
    public static ConfiguredValueTaskAwaitable ContextIndependent(this ValueTask task)
    {
        return task.ConfigureAwait(false);
    }
    
    public static ConfiguredValueTaskAwaitable<T> ContextIndependent<T>(this ValueTask<T> task)
    {
        return task.ConfigureAwait(false);
    }
}