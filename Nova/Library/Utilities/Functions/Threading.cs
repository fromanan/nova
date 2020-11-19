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
            var synchronization = SynchronizationContext.Current;
            if (synchronization != null)
            {
                //synchronization.Send(_ => action(), null);//sync
                //OR
                synchronization.Post(_ => action(), null);//async
            }
            else
            {
                Task.Factory.StartNew(action);
            }

            //OR
            /*var scheduler = TaskScheduler.FromCurrentSynchronizationContext();

            Task task = new Task(action);
            if (scheduler != null)
                task.Start(scheduler);
            else
                task.Start();*/
        }

        public static bool IsPlaying()
        {
            bool isPlaying = false;
            ExecuteInMainContext(() => isPlaying = Application.isPlaying);
            return isPlaying;
        }

        public static void RunIfPlaying(Action FunctionToCall)
        {
            if (!IsPlaying()) return;
            ExecuteInMainContext(FunctionToCall);
        }
    }
}