using System;
using System.Reflection;
using NClient.Abstractions.Resilience;
using NClient.Providers.Resilience.Polly;

namespace NClient.Resilience
{
    internal class AnyMethodResiliencePolicyProvider : MethodResiliencePolicyProviderBase
    {
        public AnyMethodResiliencePolicyProvider(
            int retryCount = 2,
            Func<int, TimeSpan>? sleepDurationProvider = null,
            Func<ResponseContext, bool>? resultPredicate = null)
            : base(retryCount, sleepDurationProvider, resultPredicate)
        {
        }

        public override IResiliencePolicy Create(MethodInfo methodInfo)
        {
            return new PollyResiliencePolicy(Policy);
        }
    }
}
