using NClient.Providers.Transport;

namespace NClient.Core.Extensions
{
    public static class HttpMethodExtensions
    {
        // TODO: может привести к ошибкам
        public static bool IsIdempotentMethod(this RequestType? transportMethod)
        {
            return transportMethod.HasValue && IsIdempotentMethod(transportMethod.Value);
        }
        
        public static bool IsIdempotentMethod(this RequestType requestType)
        {
            return requestType switch
            {
                RequestType.Post => false,
                _ => true
            };
        }
        
        // TODO: может привести к ошибкам
        public static bool IsSafeMethod(this RequestType? transportMethod)
        {
            return transportMethod.HasValue && IsSafeMethod(transportMethod.Value);
        }

        public static bool IsSafeMethod(this RequestType requestType)
        {
            return requestType switch
            {
                RequestType.Get => true,
                RequestType.Head => true,
                RequestType.Options => true,
                _ => false
            };
        }
    }
}
