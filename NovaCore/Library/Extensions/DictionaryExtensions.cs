using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NovaCore.Library.Extensions
{
    /*public static class DictionaryExtensions
    {
        public static T Flatten<S, T, U>(this IDictionary<S, T> dictionary) where T : class, IEnumerable<U>, ICollection
        {
            return dictionary.Values.SelectMany(elem => elem) as T;
        }

        // // https://social.msdn.microsoft.com/Forums/en-US/95c96221-48c5-461f-b68e-fb23e11169a7/merging-dictionaries-in-linq?forum=csharpgeneral
        public static IDictionary<S, T> Merge<S, T, U>(this IDictionary<S, T> a, IDictionary<S, T> b) where T : class, IEnumerable<U>, ICollection
        {
            return a
                .ToLookup(z => z.Key)
                .Concat(b.ToLookup(z => z.Key))
                .GroupBy(z => z.Key, z => 
                    z.Select(y => y.Value))
                .ToDictionary(z => z.Key, z => 
                    z.SelectMany(y => y.SelectMany(u => u)).Distinct()) as IDictionary<S, T>;
        }
    }*/
}