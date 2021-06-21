using System.Reflection;
using NClient.Abstractions.Resilience;
using NClient.Annotations.Methods;
using NClient.Core.Resilience;
using NClient.Providers.Resilience.Polly;

namespace NClient.Resilience
{
    internal class SafePollyMethodResiliencePolicyProvider : PollyMethodResiliencePolicyProviderBase
    {
        public override IResiliencePolicy Create(MethodInfo methodInfo)
        {
            var isSafeMethod = GetMethodAttributeFor(methodInfo) switch
            {
                GetMethodAttribute => true,
                HeadMethodAttribute => true,
                OptionsMethodAttribute => true,
                _ => false
            };

            return isSafeMethod
                ? new PollyResiliencePolicy(Policy)
                : new DefaultResiliencePolicy();
        }
    }
}