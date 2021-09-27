using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NClient.Abstractions.Resilience;
using NClient.Core.Helpers;

namespace NClient.Core.Resilience
{
    public class DefaultMethodResiliencePolicyProvider<TResponse> : IMethodResiliencePolicyProvider<TResponse>
    {
        private readonly IResiliencePolicyProvider<TResponse>? _defaultResiliencePolicyProvider;
        private readonly IMethodResiliencePolicyProvider<TResponse>? _defaultMethodResiliencePolicyProvider;
        private readonly IReadOnlyDictionary<MethodInfo, IResiliencePolicyProvider<TResponse>> _resiliencePolicyProviders;

        public DefaultMethodResiliencePolicyProvider(
            IMethodResiliencePolicyProvider<TResponse> defaultMethodResiliencePolicyProvider,
            IReadOnlyDictionary<MethodInfo, IResiliencePolicyProvider<TResponse>>? specificResiliencePolicyProviders = null)
        {
            _defaultMethodResiliencePolicyProvider = defaultMethodResiliencePolicyProvider;
            _resiliencePolicyProviders = specificResiliencePolicyProviders ?? new Dictionary<MethodInfo, IResiliencePolicyProvider<TResponse>>();
        }

        public DefaultMethodResiliencePolicyProvider(
            IResiliencePolicyProvider<TResponse> defaultResiliencePolicyProvider,
            IReadOnlyDictionary<MethodInfo, IResiliencePolicyProvider<TResponse>>? specificResiliencePolicyProviders = null)
        {
            _defaultResiliencePolicyProvider = defaultResiliencePolicyProvider;
            _resiliencePolicyProviders = specificResiliencePolicyProviders is null
                ? new Dictionary<MethodInfo, IResiliencePolicyProvider<TResponse>>(
                    new MethodInfoEqualityComparer())
                : new Dictionary<MethodInfo, IResiliencePolicyProvider<TResponse>>(
                    specificResiliencePolicyProviders.ToDictionary(x => x.Key, x => x.Value),
                    new MethodInfoEqualityComparer());
        }

        public IResiliencePolicy<TResponse> Create(MethodInfo methodInfo)
        {
            _resiliencePolicyProviders.TryGetValue(methodInfo, out var provider);
            return provider?.Create()
                ?? _defaultMethodResiliencePolicyProvider?.Create(methodInfo)
                ?? _defaultResiliencePolicyProvider!.Create();
        }
    }
}
