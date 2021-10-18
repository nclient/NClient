﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Standalone.ClientProxy.Building.Context;

// ReSharper disable once CheckNamespace
namespace NClient.Resilience
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

        public IResiliencePolicy<TRequest, TResponse> Create(MethodInfo methodInfo, IHttpRequest httpRequest)
        {
            var resiliencePolicyPredicate = _resiliencePolicyPredicates.FirstOrDefault(x => x.Predicate(methodInfo, httpRequest));
            return resiliencePolicyPredicate?.Provider.Create() ?? _defaultResiliencePolicyProvider!.Create();
        }
    }
}
