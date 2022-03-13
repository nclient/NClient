using System.Collections.Generic;
using System.Linq;

namespace NClient.Benchmark.Client.Helpers
{
    public class ListProvider
    {
        public static List<string> Get() => Enumerable
            .Range(start: 0, count: 4)
            .Select(_ => $"id-{IdProvider.Get()}")
            .ToList();
    }
}
