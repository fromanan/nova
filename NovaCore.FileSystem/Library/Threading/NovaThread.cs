using System;
using System.Threading;
using System.Threading.Tasks;
using NovaCore.Common;
using Logger = NovaCore.Common.Logging.Logger;

namespace NovaCore.Threading
{
    public abstract class NovaThread : INovaShutdown
    {
        public readonly Thread Thread;
        public readonly Logger Logger;
        
        public bool Exited { get; protected set; }
        
        private class PauseException : Exception { }
        
        private class ExitException : Exception { }
        
        private readonly AutoResetEvent _pauseWaitHandle = new(false);
        
        protected NovaThread(Logger logger = null)
        {
            Thread = new Thread(Run);
            Logger = logger ?? new Logger();
            Global.Subscribe(this);
        }

        private void Run()
        {
            while (true)
            {
                try
                {
                    Update();
                }
                catch (PauseException)
                {
                    _pauseWaitHandle.WaitOne();
                }
                catch (ExitException)
                {
                    break;
                }
                catch (Exception exception)
                {
                    if (!HandleExceptions(exception))
                        break;
                }
            }

            Exited = true;
        }

        protected abstract void Update();

        protected virtual bool HandleExceptions(Exception exception)
        {
            Logger.LogException(exception);
            return false;
        }
        
        public virtual void Start()
        {
            Thread.Start();
        }

        public virtual void Close()
        {
            Exit();
        }

        public void WaitForExit()
        {
            Thread.Join();
        }

        public void Interrupt()
        {
            Thread.Interrupt();
        }
        
        public void Resume()
        {
            _pauseWaitHandle.Set();
        }

        public void Pause()
        {
            _pauseWaitHandle.Reset();
            throw new PauseException();
        }

        public void Wait(int millisecondsDelay)
        {
            Logger.LogInfo($"Waiting... ({millisecondsDelay} ms)");
            _pauseWaitHandle.Reset();
            Task.Run(async () =>
            {
                //Thread.Sleep(millisecondsDelay);
                await Task.Delay(millisecondsDelay);
                _pauseWaitHandle.Set();
            });
            throw new PauseException();
        }

        private void Exit()
        {
            if (Exited) return;
            Logger.LogInfo($"Exiting thread {GetHashCode()} ({GetType()})");
            throw new ExitException();
        }
        
        public void OnShutdown()
        {
            Close();
        }
    }
}