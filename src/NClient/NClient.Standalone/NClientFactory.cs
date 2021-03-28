using Microsoft.Extensions.Logging;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;

namespace NClient.Standalone
{
    public interface INClientFactory
    {
        T Create<T>(string host) where T : class;
    }

    public class NClientFactory : INClientFactory
    {
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly IResiliencePolicyProvider _resiliencePolicyProvider;
        private readonly ILoggerFactory _loggerFactory;

        public NClientFactory(
            IHttpClientProvider httpClientProvider, 
            IResiliencePolicyProvider resiliencePolicyProvider,
            ILoggerFactory loggerFactory)
        {
            _httpClientProvider = httpClientProvider;
            _resiliencePolicyProvider = resiliencePolicyProvider;
            _loggerFactory = loggerFactory;
        }

        public T Create<T>(string host) where T : class
        {
            return new NClientBuilder()
                .Use<T>(host, _httpClientProvider)
                .WithResiliencePolicy(_resiliencePolicyProvider)
                .WithLogging(_loggerFactory.CreateLogger<T>())
                .Build();
        }
    }
}
