using System;
using System.Collections.Generic;
using System.Linq;

namespace NovaCore.Library.Utilities
{
    public static class Misc
    {
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