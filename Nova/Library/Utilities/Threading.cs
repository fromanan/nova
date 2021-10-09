using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Nova.Library.Utilities.Functions
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

        /*public static void ExecuteInMainContext(Action action)
        {
            Task task = new Task(action);
            if (TaskScheduler.FromCurrentSynchronizationContext() is TaskScheduler scheduler)
            {
                task.Start(scheduler);
            }
            {
                task.Start();
            }
        }*/

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