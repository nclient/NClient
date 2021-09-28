using System;
using System.Net.Http;
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
            Func<ResponseContext<HttpResponseMessage>, bool>? resultPredicate = null)
            : base(retryCount, sleepDurationProvider, resultPredicate)
        {
        }

        public override IResiliencePolicy<HttpResponseMessage> Create(MethodInfo methodInfo)
        {
            return new PollyResiliencePolicy<HttpResponseMessage>(Policy);
        }
    }
}
