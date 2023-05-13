using System.Threading;
using System.Threading.Tasks;

namespace NovaCore.Threading;

public class NovaTask
{
    private Task Task;
    public CancellationTokenSource TokenSource { get; private set; } = new();
    public CancellationToken Token { get; private set; }

    public NovaTask()
    {
        Token = TokenSource.Token;
    }
        
    public NovaTask(Task task)
    {
        Task = task;
        Token = TokenSource.Token;
    }

    public void Run()
    {
        if (Task is not null)
        {
            Task.Start();
        }
        else
        {
            Task = Task.Run(() => 
            {
                
            }, Token);
        }
    }

    public void Cancel()
    {
        TokenSource.Cancel();
    }
}