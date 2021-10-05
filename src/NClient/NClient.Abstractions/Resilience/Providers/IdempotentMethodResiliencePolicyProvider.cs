using System.Net.Http;
using System.Reflection;
using NClient.Abstractions.HttpClients;

namespace NClient.Abstractions.Resilience.Providers
{
    public class IdempotentMethodResiliencePolicyProvider<TRequest, TResponse> : IMethodResiliencePolicyProvider<TRequest, TResponse>
    {
        private readonly IResiliencePolicyProvider<TRequest, TResponse> _idempotentMethodProvider;
        private readonly IResiliencePolicyProvider<TRequest, TResponse> _otherMethodProvider;

        public IdempotentMethodResiliencePolicyProvider(
            IResiliencePolicyProvider<TRequest, TResponse> idempotentMethodProvider,
            IResiliencePolicyProvider<TRequest, TResponse> otherMethodProvider)
        {
            _idempotentMethodProvider = idempotentMethodProvider;
            _otherMethodProvider = otherMethodProvider;
        }

        public IResiliencePolicy<TRequest, TResponse> Create(MethodInfo methodInfo, HttpRequest httpRequest)
        {
            return httpRequest.Method switch
            {
                { } x when x == HttpMethod.Post => _otherMethodProvider.Create(),
                _ => _idempotentMethodProvider.Create()
            };
        }
    }
}
