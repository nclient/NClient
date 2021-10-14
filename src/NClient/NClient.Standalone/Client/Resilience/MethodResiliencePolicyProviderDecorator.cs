using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Core.Helpers;

// ReSharper disable once CheckNamespace
namespace NClient.Resilience
{
    public class MethodResiliencePolicyProviderDecorator<TRequest, TResponse> : IMethodResiliencePolicyProvider<TRequest, TResponse>
    {
        private readonly IMethodResiliencePolicyProvider<TRequest, TResponse>? _defaultMethodResiliencePolicyProvider;
        private readonly IReadOnlyDictionary<MethodInfo, IResiliencePolicyProvider<TRequest, TResponse>> _resiliencePolicyProviders;

        public MethodResiliencePolicyProviderDecorator(
            IMethodResiliencePolicyProvider<TRequest, TResponse> defaultMethodResiliencePolicyProvider,
            IReadOnlyDictionary<MethodInfo, IResiliencePolicyProvider<TRequest, TResponse>>? specificResiliencePolicyProviders = null)
        {
            _defaultMethodResiliencePolicyProvider = defaultMethodResiliencePolicyProvider;
            _resiliencePolicyProviders = specificResiliencePolicyProviders is null
                ? new Dictionary<MethodInfo, IResiliencePolicyProvider<TRequest, TResponse>>(
                    new MethodInfoEqualityComparer())
                : new Dictionary<MethodInfo, IResiliencePolicyProvider<TRequest, TResponse>>(
                    specificResiliencePolicyProviders.ToDictionary(x => x.Key, x => x.Value),
                    new MethodInfoEqualityComparer());
        }

        public IResiliencePolicy<TRequest, TResponse> Create(MethodInfo methodInfo, IHttpRequest httpRequest)
        {
            _resiliencePolicyProviders.TryGetValue(methodInfo, out var provider);
            return provider?.Create() ?? _defaultMethodResiliencePolicyProvider!.Create(methodInfo, httpRequest);
        }
    }
}
