using System;
using System.Net.Http;
using System.Reflection;
using NClient.Abstractions.Resilience;
using NClient.Annotations.Methods;
using NClient.Providers.Resilience.Polly;

namespace NClient.Resilience
{
    internal class SafeMethodResiliencePolicyProvider : MethodResiliencePolicyProviderBase
    {
        public SafeMethodResiliencePolicyProvider(
            int retryCount = 2,
            Func<int, TimeSpan>? sleepDurationProvider = null,
            Func<ResponseContext<HttpRequestMessage, HttpResponseMessage>, bool>? resultPredicate = null)
            : base(retryCount, sleepDurationProvider, resultPredicate)
        {
        }

        public override IResiliencePolicy<HttpRequestMessage, HttpResponseMessage> Create(MethodInfo methodInfo)
        {
            var isSafeMethod = GetMethodAttributeFor(methodInfo) switch
            {
                GetMethodAttribute => true,
                HeadMethodAttribute => true,
                OptionsMethodAttribute => true,
                _ => false
            };

            return isSafeMethod
                ? new PollyResiliencePolicy<HttpRequestMessage, HttpResponseMessage>(Policy)
                : new DefaultResiliencePolicy();
        }
    }
}
