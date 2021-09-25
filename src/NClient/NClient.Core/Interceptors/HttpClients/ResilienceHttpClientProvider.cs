using Microsoft.Extensions.Logging;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;
using NClient.Core.Interceptors.HttpResponsePopulation;
using NClient.Core.Resilience;

namespace NClient.Core.Interceptors.HttpClients
{
    internal interface IResilienceHttpClientProvider
    {
        IResilienceHttpClient Create(IResiliencePolicyProvider? resiliencePolicyProvider);
    }

    internal class ResilienceHttpClientProvider : IResilienceHttpClientProvider
    {
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly ISerializerProvider _serializerProvider;
        private readonly IHttpResponsePopulater _httpResponsePopulater;
        private readonly IMethodResiliencePolicyProvider _methodResiliencePolicyProvider;
        private readonly ILogger? _logger;

        public ResilienceHttpClientProvider(
            IHttpClientProvider httpClientProvider,
            ISerializerProvider serializerProvider,
            IHttpResponsePopulater httpResponsePopulater,
            IMethodResiliencePolicyProvider methodResiliencePolicyProvider,
            ILogger? logger = null)
        {
            _httpClientProvider = httpClientProvider;
            _serializerProvider = serializerProvider;
            _httpResponsePopulater = httpResponsePopulater;
            _methodResiliencePolicyProvider = methodResiliencePolicyProvider;
            _logger = logger;
        }

        public IResilienceHttpClient Create(IResiliencePolicyProvider? resiliencePolicyProvider)
        {
            return new ResilienceHttpClient(
                _httpClientProvider,
                _serializerProvider,
                _httpResponsePopulater,
                resiliencePolicyProvider == null
                    ? _methodResiliencePolicyProvider
                    : new DefaultMethodResiliencePolicyProvider(resiliencePolicyProvider),
                _logger);
        }
    }
}
