using System.Collections.Generic;
using System.Reflection;
using NClient.Abstractions.Resilience;

namespace NClient.Core.Resilience
{
    public class DefaultMethodResiliencePolicyProvider : IMethodResiliencePolicyProvider
    {
        private readonly IResiliencePolicyProvider? _defaultResiliencePolicyProvider;
        private readonly IMethodResiliencePolicyProvider? _defaultMethodResiliencePolicyProvider;
        private readonly IReadOnlyDictionary<MethodInfo, IResiliencePolicyProvider> _resiliencePolicyProviders;

        public DefaultMethodResiliencePolicyProvider(
            IMethodResiliencePolicyProvider defaultMethodResiliencePolicyProvider,
            IReadOnlyDictionary<MethodInfo, IResiliencePolicyProvider>? specificResiliencePolicyProviders = null)
        {
            _defaultMethodResiliencePolicyProvider = defaultMethodResiliencePolicyProvider;
            _resiliencePolicyProviders = specificResiliencePolicyProviders ?? new Dictionary<MethodInfo, IResiliencePolicyProvider>();
        }

        public DefaultMethodResiliencePolicyProvider(
            IResiliencePolicyProvider defaultResiliencePolicyProvider,
            IReadOnlyDictionary<MethodInfo, IResiliencePolicyProvider>? specificResiliencePolicyProviders = null)
        {
            _defaultResiliencePolicyProvider = defaultResiliencePolicyProvider;
            _resiliencePolicyProviders = specificResiliencePolicyProviders ?? new Dictionary<MethodInfo, IResiliencePolicyProvider>();
        }

        public IResiliencePolicy Create(MethodInfo methodInfo)
        {
            _resiliencePolicyProviders.TryGetValue(methodInfo, out var provider);
            return provider?.Create()
                   ?? _defaultMethodResiliencePolicyProvider?.Create(methodInfo)
                   ?? _defaultResiliencePolicyProvider!.Create();
        }
    }
}