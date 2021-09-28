using System;
using System.Collections.Generic;
using System.Linq;

namespace NovaCore.Library.Utilities
{
    public static class Misc
    {
        private static readonly Random RNG = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = RNG.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    
        // Returns a shuffled version of the list
        public static IList<T> Shuffled<T>(this IList<T> list)
        {
            IList<T> newList = list.ToList();
            int n = newList.Count;
            while (n > 1)
            {
                n--;
                int k = RNG.Next(n + 1);
                (newList[k], newList[n]) = (newList[n], newList[k]);
            }

            return newList;
        }

        public static int WrappedClamp(int n, int max, int min = 0)
        {
            return n < min ? max - 1 : n % max;
        }
    
        // Used for cycling through a list continuously
        public static int CycleList(int index, int delta, int length)
        {
            return WrappedClamp(index + delta, length);
        }

        public static IEnumerable<T> Repeat<T>(Func<T> generator, int n)
        {
            return Enumerable.Range(0, n).Select(x => generator.Invoke());
        }
    }
}