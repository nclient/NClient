using Microsoft.Extensions.Logging;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;

namespace NClient.Core.HttpClients
{
    public interface IResilienceHttpClientProvider : IHttpClientProvider
    {
        IHttpClient Create(IResiliencePolicyProvider? resiliencePolicyProvider);
    }

    public class ResilienceHttpClientProvider : IResilienceHttpClientProvider
    {
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly IResiliencePolicyProvider _resiliencePolicyProvider;
        private readonly ILogger? _logger;

        public ResilienceHttpClientProvider(
            IHttpClientProvider httpClientProvider,
            IResiliencePolicyProvider resiliencePolicyProvider,
            ILogger? logger = null)
        {
            _httpClientProvider = httpClientProvider;
            _resiliencePolicyProvider = resiliencePolicyProvider;
            _logger = logger;
        }

        public IHttpClient Create()
        {
            return new ResilienceHttpClient(_httpClientProvider, _resiliencePolicyProvider, _logger);
        }

        public IHttpClient Create(IResiliencePolicyProvider? resiliencePolicyProvider)
        {
            return new ResilienceHttpClient(_httpClientProvider, resiliencePolicyProvider ?? _resiliencePolicyProvider, _logger);
        }
    }
}