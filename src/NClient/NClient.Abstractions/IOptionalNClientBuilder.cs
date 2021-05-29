using Microsoft.Extensions.Logging;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;

namespace NClient.Abstractions
{
    public interface IOptionalNClientBuilder<TInterface> where TInterface : class
    {
        IOptionalNClientBuilder<TInterface> WithCustomHttpClient(IHttpClientProvider httpClientProvider);
        IOptionalNClientBuilder<TInterface> WithCustomSerializer(ISerializerProvider serializerProvider);
        IOptionalNClientBuilder<TInterface> WithResiliencePolicy(IResiliencePolicyProvider resiliencePolicyProvider);
        IOptionalNClientBuilder<TInterface> WithLogging(ILogger<TInterface> logger);
        TInterface Build();
    }
}