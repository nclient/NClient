using System.Linq;

namespace NClient.Benchmark.Client.Helpers
{
    public class ArrayProvider
    {
        private static long _id;

        public static string[] Get() => Enumerable
            .Range(start: 0, count: 4)
            .Select(_ => $"id-{_id++}")
            .ToArray();
    }
}
