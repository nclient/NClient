using System;
using System.Net.Http;
using System.Reflection;
using NClient.Abstractions.Resilience;
using NClient.Annotations.Methods;
using NClient.Providers.Resilience.Polly;

namespace NClient.Resilience
{
    internal class IdempotentMethodResiliencePolicyProvider : MethodResiliencePolicyProviderBase
    {
        public IdempotentMethodResiliencePolicyProvider(
            int retryCount = 2,
            Func<int, TimeSpan>? sleepDurationProvider = null,
            Func<ResponseContext<HttpResponseMessage>, bool>? resultPredicate = null)
            : base(retryCount, sleepDurationProvider, resultPredicate)
        {
        }

        public override IResiliencePolicy<HttpResponseMessage> Create(MethodInfo methodInfo)
        {
            var isIdempotentMethod = GetMethodAttributeFor(methodInfo) switch
            {
                PostMethodAttribute => false,
                _ => true
            };

            return isIdempotentMethod
                ? new PollyResiliencePolicy<HttpResponseMessage>(Policy)
                : new DefaultResiliencePolicy();
        }
    }
}
