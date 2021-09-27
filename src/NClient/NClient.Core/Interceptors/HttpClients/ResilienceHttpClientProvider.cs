using Microsoft.Extensions.Logging;
using NClient.Abstractions.Handling;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;
using NClient.Core.Resilience;

namespace NClient.Core.Interceptors.HttpClients
{
    internal interface IResilienceHttpClientProvider<TRequest, TResponse>
    {
        IResilienceHttpClient<TRequest, TResponse> Create(IResiliencePolicyProvider<TResponse>? resiliencePolicyProvider);
    }

    internal class ResilienceHttpClientProvider<TRequest, TResponse> : IResilienceHttpClientProvider<TRequest, TResponse>
    {
        private readonly ISerializerProvider _serializerProvider;
        private readonly IClientHandler<TRequest, TResponse> _clientHandler;
        private readonly IHttpClientProvider<TRequest, TResponse> _httpClientProvider;
        private readonly IMethodResiliencePolicyProvider<TResponse> _methodResiliencePolicyProvider;
        private readonly ILogger? _logger;

        public ResilienceHttpClientProvider(
            ISerializerProvider serializerProvider,
            IClientHandler<TRequest, TResponse> clientHandler,
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IMethodResiliencePolicyProvider<TResponse> methodResiliencePolicyProvider,
            ILogger? logger = null)
        {
            _serializerProvider = serializerProvider;
            _clientHandler = clientHandler;
            _httpClientProvider = httpClientProvider;
            _methodResiliencePolicyProvider = methodResiliencePolicyProvider;
            _logger = logger;
        }

        public IResilienceHttpClient<TRequest, TResponse> Create(IResiliencePolicyProvider<TResponse>? resiliencePolicyProvider)
        {
            return new ResilienceHttpClient<TRequest, TResponse>(
                _serializerProvider,
                _clientHandler,
                _httpClientProvider,
                resiliencePolicyProvider == null
                    ? _methodResiliencePolicyProvider
                    : new DefaultMethodResiliencePolicyProvider<TResponse>(resiliencePolicyProvider),
                _logger);
        }
    }
}
