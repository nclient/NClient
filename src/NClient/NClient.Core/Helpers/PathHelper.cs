namespace NClient.Core.Helpers
{
    internal static class PathHelper
    {
        public static string Combine(string left, string right)
        {
            return $"{left.TrimStart('/').TrimEnd('/')}/{right.TrimStart('/').TrimEnd('/')}";
        }
    }
}
