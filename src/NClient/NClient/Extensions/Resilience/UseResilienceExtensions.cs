using System;
using System.Net.Http;
using NClient.Providers.Transport;
using NClient.Providers.Transport.SystemNetHttp;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class UseResilienceExtensions
    {
        /// <summary>Sets a resilience policy for specific method/methods.</summary>
        /// <param name="clientResilienceSetter"></param>
        /// <param name="maxRetries">The max number of retries.</param>
        /// <param name="getDelay">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <param name="shouldRetry">The predicate to filter the results this policy will handle.</param>
        public static INClientResilienceMethodSelector<TClient, HttpRequestMessage, HttpResponseMessage> Use<TClient>(
            this INClientResilienceSetter<TClient, HttpRequestMessage, HttpResponseMessage> clientResilienceSetter, 
            int? maxRetries = null, Func<int, TimeSpan>? getDelay = null, Func<IResponseContext<HttpRequestMessage, HttpResponseMessage>, bool>? shouldRetry = null)
        {
            return clientResilienceSetter.UsePolly(
                new DefaultSystemNetHttpResiliencePolicySettings(maxRetries, getDelay, shouldRetry));
        }
        
        /// <summary>Sets a resilience policy for specific method/methods.</summary>
        /// <param name="factoryResilienceSetter"></param>
        /// <param name="maxRetries">The max number of retries.</param>
        /// <param name="getDelay">The function that provides the duration to wait for for a particular retry attempt.</param>
        /// <param name="shouldRetry">The predicate to filter the results this policy will handle.</param>
        public static INClientFactoryResilienceMethodSelector<HttpRequestMessage, HttpResponseMessage> Use(
            this INClientFactoryResilienceSetter<HttpRequestMessage, HttpResponseMessage> factoryResilienceSetter, 
            int? maxRetries = null, Func<int, TimeSpan>? getDelay = null, Func<IResponseContext<HttpRequestMessage, HttpResponseMessage>, bool>? shouldRetry = null)
        {
            return factoryResilienceSetter.UsePolly(
                new DefaultSystemNetHttpResiliencePolicySettings(maxRetries, getDelay, shouldRetry));
        }
    }
}
