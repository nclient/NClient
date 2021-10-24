using NClient.Providers.Transport;

namespace NClient.Core.Extensions
{
    public static class RequestTypeExtensions
    {
        public static bool IsIdempotent(this RequestType requestType)
        {
            return requestType switch
            {
                RequestType.Create => false,
                _ => true
            };
        }

        public static bool IsSafe(this RequestType requestType)
        {
            return requestType switch
            {
                RequestType.Read => true,
                RequestType.Head => true,
                RequestType.Options => true,
                _ => false
            };
        }
    }
}
