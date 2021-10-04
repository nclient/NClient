using System;
using System.Net.Http;
using NClient.Abstractions.Customization.Resilience;
using NClient.Abstractions.Resilience;
using NClient.Providers.HttpClient.System;
using NClient.Providers.Resilience.Polly;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class ResilienceSetterExtensions
    {
        // TODO: doc
        public static INClientResilienceMethodSelector<TClient, HttpRequestMessage, HttpResponseMessage> Use<TClient>(
            this INClientResilienceSetter<TClient, HttpRequestMessage, HttpResponseMessage> clientResilienceSetter, 
            int? maxRetries = null, Func<int, TimeSpan>? getDelay = null, Func<ResponseContext<HttpRequestMessage, HttpResponseMessage>, bool>? shouldRetry = null)
        {
            return clientResilienceSetter.UsePolly(
                new DefaultSystemResiliencePolicySettings(maxRetries, getDelay, shouldRetry));
        }
        
        public static INClientFactoryResilienceMethodSelector<HttpRequestMessage, HttpResponseMessage> Use(
            this INClientFactoryResilienceSetter<HttpRequestMessage, HttpResponseMessage> factoryResilienceSetter, 
            int? maxRetries = null, Func<int, TimeSpan>? getDelay = null, Func<ResponseContext<HttpRequestMessage, HttpResponseMessage>, bool>? shouldRetry = null)
        {
            return factoryResilienceSetter.UsePolly(
                new DefaultSystemResiliencePolicySettings(maxRetries, getDelay, shouldRetry));
        }
    }
}
