using System.Diagnostics;
using System.Threading.Tasks;

namespace NovaCore.Files;

public class NovaProcess
{
    public Process Process { get; private set; }

    public delegate void LogHandler(string s);
        
    public event LogHandler LogCallback = delegate {  };

    public event LogHandler ErrorCallback = delegate {  };

    public bool LogMessages;
        
    public bool LogErrors;
        
    public bool Started { get; private set; }
        
    public bool Completed { get; private set; }
        
    public bool SaveResult { get; private set; }
        
    public bool SaveErrors { get; private set; }
        
    public string Result { get; private set; }
        
    public string Errors { get; private set; }
        
    private readonly TaskCompletionSource<int> CompletionSource = new();

    public NovaProcess(string filepath, string args = "")
    {
        Process = FileSystem.CreateExternalProcess(filepath, args);
    }
        
    public NovaProcess(ProcessStartInfo startInfo)
    {
        Process = FileSystem.CreateExternalProcess(startInfo);
    }
        
    public NovaProcess(Process process)
    {
        Process = process;
    }
        
    public static implicit operator Process(NovaProcess n) => n.Process;
        
    public static explicit operator NovaProcess(Process p) => new(p);
        
    private void GetResult() => Result = Process.StandardOutput.ReadToEnd();
        
    private void GetErrors() => Result = Process.StandardError.ReadToEnd();
        
    public void Run()
    {
        if (Started) return;

        Start();

        Process.WaitForExit();

        Stop();
    }

    public Task<int> RunAsync()
    {
        if (Started) return null;
            
        Process.EnableRaisingEvents = true;
            
        Process.Exited += (sender, args) =>
        {
            CompletionSource.SetResult(Process.ExitCode);
            Stop();
            Process.Dispose();
        };
            
        Start();

        return CompletionSource.Task;
    }

    private void Start()
    {
        if (LogMessages)
        {
            Process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data)) { LogCallback.Invoke(e.Data); }
            };
        }

        if (LogErrors)
        {
            Process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data)) { ErrorCallback.Invoke(e.Data); }
            };
        }

        Process.Start();

        if (LogMessages)
        {
            Process.BeginOutputReadLine();
        }

        if (LogErrors)
        {
            Process.BeginErrorReadLine();
        }

        Started = true;
    }

    private void Stop()
    {
        if (SaveResult)
        {
            GetResult();
        }
            
        if (SaveErrors)
        {
            GetErrors();
        }
            
        Completed = true;
    }
}