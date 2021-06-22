using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NClient.Abstractions.Resilience;
using NClient.Core.Helpers;

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
            _resiliencePolicyProviders = specificResiliencePolicyProviders is null 
                ? new Dictionary<MethodInfo, IResiliencePolicyProvider>(
                    new MethodInfoEqualityComparer())
                : new Dictionary<MethodInfo, IResiliencePolicyProvider>(
                    specificResiliencePolicyProviders.ToDictionary(x => x.Key, x => x.Value), 
                    new MethodInfoEqualityComparer());
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