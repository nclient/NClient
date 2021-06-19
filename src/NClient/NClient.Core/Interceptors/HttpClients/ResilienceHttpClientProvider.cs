using Microsoft.Extensions.Logging;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;
using NClient.Core.Interceptors.HttpResponsePopulation;

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
        private readonly IResiliencePolicyProvider _resiliencePolicyProvider;
        private readonly ILogger? _logger;

        public ResilienceHttpClientProvider(
            IHttpClientProvider httpClientProvider,
            ISerializerProvider serializerProvider,
            IHttpResponsePopulater httpResponsePopulater,
            IResiliencePolicyProvider resiliencePolicyProvider,
            ILogger? logger = null)
        {
            _httpClientProvider = httpClientProvider;
            _serializerProvider = serializerProvider;
            _httpResponsePopulater = httpResponsePopulater;
            _resiliencePolicyProvider = resiliencePolicyProvider;
            _logger = logger;
        }

        public IResilienceHttpClient Create(IResiliencePolicyProvider? resiliencePolicyProvider)
        {
            return new ResilienceHttpClient(
                _httpClientProvider, 
                _serializerProvider, 
                _httpResponsePopulater,
                resiliencePolicyProvider ?? _resiliencePolicyProvider, 
                _logger);
        }
    }
}