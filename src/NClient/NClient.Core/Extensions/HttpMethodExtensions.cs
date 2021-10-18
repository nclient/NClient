using System.Net.Http;

namespace NClient.Core.Extensions
{
    public static class HttpMethodExtensions
    {
        public static bool IsIdempotentMethod(this HttpMethod httpMethod)
        {
            return httpMethod switch
            {
                { } x when x == HttpMethod.Post => false,
                _ => true
            };
        }
        
        public static bool IsSafeMethod(this HttpMethod httpMethod)
        {
            return httpMethod switch
            {
                { } x when x == HttpMethod.Get => true,
                { } x when x == HttpMethod.Head => true,
                { } x when x == HttpMethod.Options => true,
                _ => false
            };
        }
    }
}
