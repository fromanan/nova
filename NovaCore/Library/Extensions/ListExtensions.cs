using System;
using System.Collections.Generic;
using System.Linq;

namespace NovaCore.Extensions
{
    public static class ListExtensions
    {
        private static readonly Random Random = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Next(n + 1);
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
                int k = Random.Next(n + 1);
                (newList[k], newList[n]) = (newList[n], newList[k]);
            }

            return newList;
        }
    }
}