using System;
using System.Net.Http;

namespace NClient.Providers.Transport.Http.System.Helpers
{
    public static class HttpRequestMessageExtensions
    {
        public static TimeSpan GetTimeout(this HttpRequestMessage httpRequestMessage)
        {
            return TryGetTimeout(httpRequestMessage) 
                ?? throw new ArgumentException($"The properties of {nameof(HttpRequestMessage)} do not contain a timeout.");
        }
        
        public static TimeSpan? TryGetTimeout(this HttpRequestMessage httpRequestMessage)
        {
            if (httpRequestMessage.Properties.TryGetValue("Timeout", out var timeout)) 
                return (TimeSpan) timeout;
            return null;
        }
        
        public static HttpRequestMessage SetTimeout(this HttpRequestMessage httpRequestMessage, TimeSpan timeout)
        {
            httpRequestMessage.Properties["Timeout"] = timeout;
            return httpRequestMessage;
        }
    }
}
