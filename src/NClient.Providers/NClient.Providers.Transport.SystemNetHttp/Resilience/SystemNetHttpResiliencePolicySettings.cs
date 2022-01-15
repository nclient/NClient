using System;
using System.Net.Http;
using NClient.Providers.Resilience;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Transport.SystemNetHttp
{
    public class SystemNetHttpResiliencePolicySettings : IResiliencePolicySettings<HttpRequestMessage, HttpResponseMessage>
    {
        public int MaxRetries { get; }
        public Func<int, TimeSpan> GetDelay { get; }
        public Func<IResponseContext<HttpRequestMessage, HttpResponseMessage>, bool> ShouldRetry { get; }
        
        public SystemNetHttpResiliencePolicySettings(
            int maxRetries, 
            Func<int, TimeSpan> getDelay, 
            Func<IResponseContext<HttpRequestMessage, HttpResponseMessage>, bool> shouldRetry)
        {
            MaxRetries = maxRetries;
            GetDelay = getDelay;
            ShouldRetry = shouldRetry;
        }
    }
}
