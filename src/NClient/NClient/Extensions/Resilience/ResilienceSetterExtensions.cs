using System;
using System.Net.Http;
using NClient.Providers.Transport;
using NClient.Providers.Transport.Http.System;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class ResilienceSetterExtensions
    {
        // TODO: doc
        public static INClientResilienceMethodSelector<TClient, HttpRequestMessage, HttpResponseMessage> Use<TClient>(
            this INClientResilienceSetter<TClient, HttpRequestMessage, HttpResponseMessage> clientResilienceSetter, 
            int? maxRetries = null, Func<int, TimeSpan>? getDelay = null, Func<IResponseContext<HttpRequestMessage, HttpResponseMessage>, bool>? shouldRetry = null)
        {
            return clientResilienceSetter.UsePolly(
                new DefaultSystemResiliencePolicySettings(maxRetries, getDelay, shouldRetry));
        }
        
        public static INClientFactoryResilienceMethodSelector<HttpRequestMessage, HttpResponseMessage> Use(
            this INClientFactoryResilienceSetter<HttpRequestMessage, HttpResponseMessage> factoryResilienceSetter, 
            int? maxRetries = null, Func<int, TimeSpan>? getDelay = null, Func<IResponseContext<HttpRequestMessage, HttpResponseMessage>, bool>? shouldRetry = null)
        {
            return factoryResilienceSetter.UsePolly(
                new DefaultSystemResiliencePolicySettings(maxRetries, getDelay, shouldRetry));
        }
    }
}
