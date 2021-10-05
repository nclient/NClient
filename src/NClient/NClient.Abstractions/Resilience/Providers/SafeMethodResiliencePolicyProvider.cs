﻿using System.Net.Http;
using System.Reflection;
using NClient.Abstractions.HttpClients;

namespace NClient.Abstractions.Resilience.Providers
{
    public class SafeMethodResiliencePolicyProvider<TRequest, TResponse> : IMethodResiliencePolicyProvider<TRequest, TResponse>
    {
        private readonly IResiliencePolicyProvider<TRequest, TResponse> _safeMethodProvider;
        private readonly IResiliencePolicyProvider<TRequest, TResponse> _otherMethodProvider;

        public SafeMethodResiliencePolicyProvider(
            IResiliencePolicyProvider<TRequest, TResponse> safeMethodProvider,
            IResiliencePolicyProvider<TRequest, TResponse> otherMethodProvider)
        {
            _safeMethodProvider = safeMethodProvider;
            _otherMethodProvider = otherMethodProvider;
        }

        public IResiliencePolicy<TRequest, TResponse> Create(MethodInfo methodInfo, HttpRequest httpRequest)
        {
            return httpRequest.Method switch
            {
                { } x when x == HttpMethod.Get => _safeMethodProvider.Create(),
                { } x when x == HttpMethod.Head => _safeMethodProvider.Create(),
                { } x when x == HttpMethod.Options => _safeMethodProvider.Create(),
                _ => _otherMethodProvider.Create()
            };
        }
    }
}