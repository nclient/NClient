using System.Net.Http;
using NClient.Abstractions.Customization.Resilience;
using NClient.Abstractions.Resilience;
using NClient.Providers.Resilience.Polly;
using NClient.Resilience;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class ResilienceSetterExtensions
    {
        // TODO: doc
        public static INClientResilienceMethodSelector<TClient, HttpRequestMessage, HttpResponseMessage> Use<TClient>(
            this INClientResilienceSetter<TClient, HttpRequestMessage, HttpResponseMessage> clientResilienceSetter, 
            IResiliencePolicySettings<HttpRequestMessage, HttpResponseMessage>? policySettings = null)
        {
            return clientResilienceSetter.UsePolly(policySettings ?? new DefaultResiliencePolicySettings());
        }
        
        public static INClientFactoryResilienceMethodSelector<HttpRequestMessage, HttpResponseMessage> Use(
            this INClientFactoryResilienceSetter<HttpRequestMessage, HttpResponseMessage> factoryResilienceSetter, 
            IResiliencePolicySettings<HttpRequestMessage, HttpResponseMessage>? policySettings = null)
        {
            return factoryResilienceSetter.UsePolly(policySettings ?? new DefaultResiliencePolicySettings());
        }
    }
}
