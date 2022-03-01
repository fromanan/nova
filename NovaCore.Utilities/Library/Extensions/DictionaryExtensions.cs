namespace NovaCore.Extensions
{
    /*public static class DictionaryExtensions
    {
        public static IEnumerable<T> Flatten<S, T>(this IDictionary<S, ICollection<T>> dictionary)
        {
            return dictionary.Values.SelectMany(elem => elem);
        }

        // // https://social.msdn.microsoft.com/Forums/en-US/95c96221-48c5-461f-b68e-fb23e11169a7/merging-dictionaries-in-linq?forum=csharpgeneral
        public static IDictionary<S, ICollection<T>> Merge<S, T>(this IDictionary<S, ICollection<T>> a, IDictionary<S, ICollection<T>> b)
        {
            return a
                .ToLookup(z => z.Key)
                .Concat(b.ToLookup(z => z.Key))
                .GroupBy(z => z.Key, z => 
                    z.Select(y => y.Value))
                .ToDictionary(z => z.Key, z => 
                    z.SelectMany(y => y.SelectMany(u => u)).Distinct() as ICollection<T>);
        }
    }*/
}