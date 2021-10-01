using System.Net.Http;
using NClient.Abstractions.Customization.Resilience;
using NClient.Abstractions.Resilience;
using NClient.Providers.Resilience.Polly;
using NClient.Resilience;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class ResiliencePolicyProviderSetterExtensions
    {
        // TODO: doc
        public static IResiliencePolicyMethodSelector<TInterface, HttpRequestMessage, HttpResponseMessage> Use<TInterface>(
            this IResiliencePolicyProviderSetter<TInterface, HttpRequestMessage, HttpResponseMessage> resiliencePolicyProviderSetter, 
            IResiliencePolicySettings<HttpRequestMessage, HttpResponseMessage>? policySettings = null)
        {
            return resiliencePolicyProviderSetter.UsePolly(policySettings ?? new DefaultResiliencePolicySettings());
        }
        
        public static IResiliencePolicyMethodSelector<HttpRequestMessage, HttpResponseMessage> Use(
            this IResiliencePolicyProviderSetter<HttpRequestMessage, HttpResponseMessage> resiliencePolicyProviderSetter, 
            IResiliencePolicySettings<HttpRequestMessage, HttpResponseMessage>? policySettings = null)
        {
            return resiliencePolicyProviderSetter.UsePolly(policySettings ?? new DefaultResiliencePolicySettings());
        }
    }
}
