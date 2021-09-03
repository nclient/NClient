using System;
using System.Reflection;
using NClient.Abstractions.Resilience;
using NClient.Annotations.Methods;
using NClient.Core.Resilience;
using NClient.Providers.Resilience.Polly;

namespace NClient.Resilience
{
    internal class IdempotentMethodResiliencePolicyProvider : MethodResiliencePolicyProviderBase
    {
        public IdempotentMethodResiliencePolicyProvider(
            int retryCount = 2,
            Func<int, TimeSpan>? sleepDurationProvider = null,
            Func<ResponseContext, bool>? resultPredicate = null)
            : base(retryCount, sleepDurationProvider, resultPredicate)
        {
        }

        public override IResiliencePolicy Create(MethodInfo methodInfo)
        {
            var isIdempotentMethod = GetMethodAttributeFor(methodInfo) switch
            {
                PostMethodAttribute => false,
                _ => true
            };

            return isIdempotentMethod
                ? new PollyResiliencePolicy(Policy)
                : new DefaultResiliencePolicy();
        }
    }
}
