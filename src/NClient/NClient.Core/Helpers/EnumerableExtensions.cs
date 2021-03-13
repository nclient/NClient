using System;
using System.Collections.Generic;
using System.Linq;

namespace NClient.Core.Helpers
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int count)
        {
            var collection = source as ICollection<T> ?? source.ToArray();
            return collection.Skip(Math.Max(0, collection.Count - count));
        }
    }
}
