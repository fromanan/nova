using System;
using NovaCore.Common.Interfaces;
using NovaCore.Common.Logging;

namespace NovaCore.Common;

public static class Global
{
    public static readonly Logger Logger = new();
        
    private static event Action InitEvent = delegate {  };
        
    private static event Action ValidateEvent = delegate {  };
        
    private static event Action<ExitCode> ExitEvent = delegate {  };
        
    private static event Action ShutdownEvent = delegate {  };
        
    private static event Action SaveEvent = delegate {  };
        
    private static event Action LoadEvent = delegate {  };
        
    private static event Action RestartEvent = delegate {  };
        
    private static event Action CancelEvent = delegate {  };

    public static void Init()
    {
        AppDomain.CurrentDomain.ProcessExit += (_, _) => Exit();
        Console.CancelKeyPress += (_, _) => Cancel();
        InitEvent.Invoke();
    }

    public static void Validate()
    {
        ValidateEvent.Invoke();
    }

    public static void Exit(ExitCode exitCode = ExitCode.Success)
    {
        ExitEvent.Invoke(exitCode);
    }

    public static void Shutdown()
    {
        ShutdownEvent.Invoke();
    }

    public static void Save()
    {
        SaveEvent.Invoke();
    }

    public static void Load()
    {
        LoadEvent.Invoke();
    }

    public static void Restart()
    {
        RestartEvent.Invoke();
    }

    public static void Cancel()
    {
        CancelEvent.Invoke();
    }
        
    public static void ExitImmediate(string message = null)
    {
        Environment.FailFast(message);
    }

    public static void Crash(string message = null, Exception exception = null)
    {
        Environment.FailFast(message, exception);
    }

    public static void SubscribeAll(INovaApp novaApp)
    {
        InitEvent       +=  novaApp.OnInit;
        ValidateEvent   +=  novaApp.OnValidate;
        ExitEvent       +=  novaApp.OnExit;
        ShutdownEvent   +=  novaApp.OnShutdown;
        SaveEvent       +=  novaApp.OnSave;
        LoadEvent       +=  novaApp.OnLoad;
        RestartEvent    +=  novaApp.OnRestart;
        CancelEvent     +=  novaApp.OnCancel;
    }

    public static void Subscribe(INova nova)
    {
        if (nova is INovaInit novaInit)
        {
            InitEvent += novaInit.OnInit;
        }
            
        if (nova is INovaValidate novaValidate)
        {
            ValidateEvent += novaValidate.OnValidate;
        }
            
        if (nova is INovaExit novaExit)
        {
            ExitEvent += novaExit.OnExit;
        }
            
        if (nova is INovaShutdown novaShutdown)
        {
            ShutdownEvent += novaShutdown.OnShutdown;
        }
            
        if (nova is INovaSave novaSave)
        {
            SaveEvent += novaSave.OnSave;
        }
            
        if (nova is INovaLoad novaLoad)
        {
            LoadEvent += novaLoad.OnLoad;
        }
            
        if (nova is INovaRestart novaRestart)
        {
            RestartEvent += novaRestart.OnRestart;
        }
            
        if (nova is INovaCancel novaCancel)
        {
            CancelEvent += novaCancel.OnCancel;
        }
    }
        
    public static string ShutdownMessage => $"{AppInfo.ProductName} Closed ({DateTime.Now:G})";
        
    public static string ProgramExitMessage(ExitCode exitCode) => $"Program Terminating | Exit Code: {(int)exitCode} ({exitCode.ToString()})";
}