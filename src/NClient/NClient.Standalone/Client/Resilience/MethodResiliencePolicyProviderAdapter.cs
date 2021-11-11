using System;
using System.Collections.Generic;
using System.Linq;
using NClient.Invocation;
using NClient.Providers;
using NClient.Providers.Resilience;
using NClient.Providers.Transport;
using NClient.Standalone.ClientProxy.Building.Models;

namespace NClient.Standalone.Client.Resilience
{
    internal class MethodResiliencePolicyProviderAdapter<TRequest, TResponse> : IMethodResiliencePolicyProvider<TRequest, TResponse>
    {
        private readonly IResiliencePolicyProvider<TRequest, TResponse>? _defaultResiliencePolicyProvider;
        private readonly IReadOnlyCollection<ResiliencePolicyPredicate<TRequest, TResponse>> _resiliencePolicyPredicates;

        public MethodResiliencePolicyProviderAdapter(
            IResiliencePolicyProvider<TRequest, TResponse> defaultResiliencePolicyProvider,
            IEnumerable<ResiliencePolicyPredicate<TRequest, TResponse>>? resiliencePolicyPredicates = null)
        {
            _defaultResiliencePolicyProvider = defaultResiliencePolicyProvider;
            _resiliencePolicyPredicates = resiliencePolicyPredicates?.ToArray() ?? Array.Empty<ResiliencePolicyPredicate<TRequest, TResponse>>();
        }

        public IResiliencePolicy<TRequest, TResponse> Create(IMethod method, IRequest request, IToolSet toolset)
        {
            var resiliencePolicyPredicate = _resiliencePolicyPredicates.FirstOrDefault(x => x.Predicate(method, request));
            return resiliencePolicyPredicate?.Provider.Create(toolset) ?? _defaultResiliencePolicyProvider!.Create(toolset);
        }
    }
}
