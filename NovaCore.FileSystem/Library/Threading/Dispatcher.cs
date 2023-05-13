using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NovaCore.Common;
using Logger = NovaCore.Common.Logging.Logger;

namespace NovaCore.Threading;

public class Dispatcher
{
    private readonly Queue<Task> Tasks = new();
        
    public bool Executing { get; private set; }
        
    public bool Paused { get; private set; }

    private Task MainTask;

    private Task CurrentTask;
        
    private CancellationTokenSource tokenSource = new();
    private CancellationToken token;
        
    private CancellationTokenSource currentTaskTokenSource = new();
    private CancellationToken currentTaskToken;

    private readonly AutoResetEvent StopWaitHandle = new(false);

    private const int MaximumConcurrentThreads = 10;

    public static readonly Logger Logger = new();

    public void Init()
    {
            
    }
        
    // Thread Pool

    public void Start()
    {
        token = tokenSource.Token;
        MainTask = Task.Run(Update, token);
        AppDomain.CurrentDomain.ProcessExit += (sender, args) =>
        {
            tokenSource.Cancel();
        };
        Logger.LogInfo("Dispatcher Started");
    }

    public void Update()
    {
        while (true)
        {
            StopWaitHandle.WaitOne();
            Logger.LogInfo("Executing Tasks");
            try
            {
                Run();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }

    public void Run()
    {
        token.ThrowIfCancellationRequested();
            
        Executing = true;

        while (Tasks.Count > 0)
        {
            Task task = Tasks.Dequeue();

            task.Start();

            currentTaskToken = currentTaskTokenSource.Token;

            task.Wait(currentTaskToken);
                
            if (currentTaskToken.IsCancellationRequested)
            {
                currentTaskToken.ThrowIfCancellationRequested();
                break;
            }
                
            if (token.IsCancellationRequested)
            {
                token.ThrowIfCancellationRequested();
                break;
            }
        }

        Executing = false;

        StopWaitHandle.Reset();
            
        Logger.Log("Halted Dispatcher");
    }

    private void StopDispatching()
    {
        StopWaitHandle.Reset();
    }

    private void Test()
    {
        Thread thread = null;

        Task t = Task.Run(() => 
        {
            //Capture the executing thread
            thread = Thread.CurrentThread;

            //Simulate work (usually from 3rd party code)
            Thread.Sleep(1000);

            //If you comment out thread.Abort(), then this will be displayed
            Console.WriteLine("Task finished!");
        });

        //This is needed in the example to avoid thread being still NULL
        Thread.Sleep(10);

        //Cancel the task by aborting the thread
        /*#pragma warning disable CS0618
        thread.Abort();
        #pragma warning restore CS0618*/
    }

    private void BeginDispatching()
    {
        Logger.LogInfo("Starting");
        StopWaitHandle.Set();
    }

    public void Schedule(Task task, int priority = -1)
    {
        // Add prioritizing
        Tasks.Enqueue(task);
            
        if (!Executing)
        {
            BeginDispatching();
        }
    }

    public void CancelCurrent()
    {
        if (!Executing || CurrentTask is null) return;
        currentTaskTokenSource.Cancel();
    }

    public void CancelAll()
    {
            
    }
}