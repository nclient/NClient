namespace NClient.Benchmark.Client.Helpers
{
    public class IdProvider
    {
        private static int _id;

        public static int Get() => _id++;
    }
}
