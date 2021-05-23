using Microsoft.Extensions.Logging;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;

namespace NClient.Abstractions
{
    public interface INClientFactoryBuilder
    {
        INClientFactoryBuilder WithCustomHttpClient(IHttpClientProvider httpClientProvider);
        INClientFactoryBuilder WithCustomSerializer(ISerializerProvider serializerProvider);
        INClientFactoryBuilder WithResiliencePolicy(IResiliencePolicyProvider resiliencePolicyProvider);
        INClientFactoryBuilder WithLogging(ILoggerFactory loggerFactory);
        INClientFactory Build();
    }
}