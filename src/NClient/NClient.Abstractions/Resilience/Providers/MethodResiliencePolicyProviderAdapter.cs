using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NClient.Abstractions.Helpers;
using NClient.Abstractions.HttpClients;

namespace NClient.Abstractions.Resilience.Providers
{
    public class MethodResiliencePolicyProviderAdapter<TRequest, TResponse> : IMethodResiliencePolicyProvider<TRequest, TResponse>
    {
        private readonly IResiliencePolicyProvider<TRequest, TResponse>? _defaultResiliencePolicyProvider;
        private readonly IReadOnlyDictionary<MethodInfo, IResiliencePolicyProvider<TRequest, TResponse>> _resiliencePolicyProviders;

        public MethodResiliencePolicyProviderAdapter(
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

        public IResiliencePolicy<TRequest, TResponse> Create(MethodInfo methodInfo, HttpRequest httpRequest)
        {
            _resiliencePolicyProviders.TryGetValue(methodInfo, out var provider);
            return provider?.Create() ?? _defaultResiliencePolicyProvider!.Create();
        }
    }
}
