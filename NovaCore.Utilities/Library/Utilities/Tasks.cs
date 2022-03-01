using System;
using System.Threading.Tasks;

namespace NovaCore.Utilities
{
    public static class Tasks
    {
        public static async Task Sleep() => await Task.Run(() => { });
        
        public static async Task Sleep(int durationInMilliseconds) => await Task.Delay(durationInMilliseconds);
        
        public static async Task WaitUntil<T>(Predicate<T> predicate, T arg, int tickInMilliseconds = 100)
        {
            while (!predicate.Invoke(arg))
            {
                await Sleep(tickInMilliseconds);
            }
        }
        
        public static async Task WaitUntil(Func<bool> predicate, int tickInMilliseconds = 100)
        {
            while (!predicate.Invoke())
            {
                await Sleep(tickInMilliseconds);
            }
        }
    }
}