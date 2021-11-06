using System.Collections.Generic;
using System.Linq;

namespace NovaCore.Library.Extensions
{
    public static class DictionaryExtensions
    {
        public static IEnumerable<T> Flatten<S, T>(this IDictionary<S, IEnumerable<T>> dictionary)
        {
            return dictionary.Values.SelectMany(elem => elem);
        }

        // // https://social.msdn.microsoft.com/Forums/en-US/95c96221-48c5-461f-b68e-fb23e11169a7/merging-dictionaries-in-linq?forum=csharpgeneral
        public static IDictionary<S, IEnumerable<T>> Merge<S, T>(this IDictionary<S, IEnumerable<T>> a, 
            IDictionary<S, IEnumerable<T>> b)
        {
            return a
                .ToLookup(z => z.Key)
                .Concat(b.ToLookup(z => z.Key))
                .GroupBy(z => z.Key, z => 
                    z.Select(y => y.Value))
                .ToDictionary(z => z.Key, z => 
                    z.SelectMany(y => y.SelectMany(u => u)).Distinct());
        }
    }
}