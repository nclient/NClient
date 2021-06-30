using System;

namespace NClient.Core.Helpers
{
    public static class UriHelper
    {
        public static Uri Combine(Uri baseUri, string relativePath)
        {
            return new Uri(Combine(baseUri.ToString(), relativePath));
        }

        public static string Combine(string left, string right)
        {
            return $"{left.TrimStart('/').TrimEnd('/')}/{right.TrimStart('/').TrimEnd('/')}";
        }
    }
}