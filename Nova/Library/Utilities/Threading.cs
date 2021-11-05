using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Nova.Library.Utilities
{
    public static class Threading
    {
        public static void ExecuteInMainContext(Action action)
        {
            if (SynchronizationContext.Current is SynchronizationContext context)
            {
                //context.Send(_ => action(), null); // sync
                context.Post(_ => action(), null); // async
            }
            else
            {
                Task.Factory.StartNew(action);
            }
        }

        public static async Task ExecuteInMainContextAsync(Action action)
        {
            Task task = new Task(action);
            
            if (TaskScheduler.FromCurrentSynchronizationContext() is TaskScheduler scheduler)
            {
                task.Start(scheduler);
            }
            else
            {
                task.Start();
            }
            
            await task;
        }

        public static bool IsPlaying()
        {
            bool isPlaying = false;
            ExecuteInMainContext(() => isPlaying = Application.isPlaying);
            return isPlaying;
        }

        public static void RunIfPlaying(Action action)
        {
            if (!IsPlaying()) return;
            ExecuteInMainContext(action);
        }
    }
}