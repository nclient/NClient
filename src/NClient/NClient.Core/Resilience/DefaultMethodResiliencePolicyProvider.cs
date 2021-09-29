using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NClient.Abstractions.Resilience;
using NClient.Core.Helpers;

namespace NClient.Core.Resilience
{
    public class DefaultMethodResiliencePolicyProvider<TRequest, TResponse> : IMethodResiliencePolicyProvider<TRequest, TResponse>
    {
        private readonly IResiliencePolicyProvider<TRequest, TResponse>? _defaultResiliencePolicyProvider;
        private readonly IMethodResiliencePolicyProvider<TRequest, TResponse>? _defaultMethodResiliencePolicyProvider;
        private readonly IReadOnlyDictionary<MethodInfo, IResiliencePolicyProvider<TRequest, TResponse>> _resiliencePolicyProviders;

        public DefaultMethodResiliencePolicyProvider(
            IMethodResiliencePolicyProvider<TRequest, TResponse> defaultMethodResiliencePolicyProvider,
            IReadOnlyDictionary<MethodInfo, IResiliencePolicyProvider<TRequest, TResponse>>? specificResiliencePolicyProviders = null)
        {
            _defaultMethodResiliencePolicyProvider = defaultMethodResiliencePolicyProvider;
            _resiliencePolicyProviders = specificResiliencePolicyProviders ?? new Dictionary<MethodInfo, IResiliencePolicyProvider<TRequest, TResponse>>();
        }

        public DefaultMethodResiliencePolicyProvider(
            IResiliencePolicyProvider<TRequest, TResponse> defaultResiliencePolicyProvider,
            IReadOnlyDictionary<MethodInfo, IResiliencePolicyProvider<TRequest, TResponse>>? specificResiliencePolicyProviders = null)
        {
            _defaultResiliencePolicyProvider = defaultResiliencePolicyProvider;
            _resiliencePolicyProviders = specificResiliencePolicyProviders is null
                ? new Dictionary<MethodInfo, IResiliencePolicyProvider<TRequest, TResponse>>(
                    new MethodInfoEqualityComparer())
                : new Dictionary<MethodInfo, IResiliencePolicyProvider<TRequest, TResponse>>(
                    specificResiliencePolicyProviders.ToDictionary(x => x.Key, x => x.Value),
                    new MethodInfoEqualityComparer());
        }

        public IResiliencePolicy<TRequest, TResponse> Create(MethodInfo methodInfo)
        {
            _resiliencePolicyProviders.TryGetValue(methodInfo, out var provider);
            return provider?.Create()
                ?? _defaultMethodResiliencePolicyProvider?.Create(methodInfo)
                ?? _defaultResiliencePolicyProvider!.Create();
        }
    }
}
