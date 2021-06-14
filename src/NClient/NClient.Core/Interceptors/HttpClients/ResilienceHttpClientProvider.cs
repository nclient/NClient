using Microsoft.Extensions.Logging;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;

namespace NClient.Core.Interceptors.HttpClients
{
    public interface IResilienceHttpClientProvider
    {
        IHttpClient Create(IResiliencePolicyProvider? resiliencePolicyProvider);
    }

    public class ResilienceHttpClientProvider : IResilienceHttpClientProvider
    {
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly ISerializerProvider _serializerProvider;
        private readonly IResiliencePolicyProvider _resiliencePolicyProvider;
        private readonly ILogger? _logger;

        public ResilienceHttpClientProvider(
            IHttpClientProvider httpClientProvider,
            ISerializerProvider serializerProvider,
            IResiliencePolicyProvider resiliencePolicyProvider,
            ILogger? logger = null)
        {
            _httpClientProvider = httpClientProvider;
            _serializerProvider = serializerProvider;
            _resiliencePolicyProvider = resiliencePolicyProvider;
            _logger = logger;
        }

        public IHttpClient Create(IResiliencePolicyProvider? resiliencePolicyProvider)
        {
            return new ResilienceHttpClient(_httpClientProvider, _serializerProvider, resiliencePolicyProvider ?? _resiliencePolicyProvider, _logger);
        }
    }
}