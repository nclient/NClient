using Microsoft.Extensions.Logging;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;

namespace NClient.Abstractions
{
    public interface IInterfaceBasedClientBuilder<TInterface> where TInterface : class
    {
        IInterfaceBasedClientBuilder<TInterface> WithCustomHttpClient(IHttpClientProvider httpClientProvider);
        IInterfaceBasedClientBuilder<TInterface> WithCustomSerializer(ISerializerProvider serializerProvider);
        IInterfaceBasedClientBuilder<TInterface> WithResiliencePolicy(IResiliencePolicyProvider resiliencePolicyProvider);
        IInterfaceBasedClientBuilder<TInterface> WithLogging(ILogger<TInterface> logger);
        TInterface Build();
    }
}