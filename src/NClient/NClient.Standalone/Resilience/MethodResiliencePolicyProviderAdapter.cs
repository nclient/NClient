using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Core.Helpers;

// ReSharper disable once CheckNamespace
namespace NClient.Resilience
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
