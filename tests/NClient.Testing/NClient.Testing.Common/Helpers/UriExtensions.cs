using System;

namespace NClient.Testing.Common.Helpers
{
    public static class UriExtensions
    {
        public static Uri ToUri(this string uri)
        {
            return new Uri(uri);
        }
    }
}
