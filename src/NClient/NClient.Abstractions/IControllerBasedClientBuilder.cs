using Microsoft.Extensions.Logging;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;

namespace NClient.Abstractions
{
    public interface IControllerBasedClientBuilder<TInterface, TController>
        where TInterface : class
        where TController : TInterface
    {
        IControllerBasedClientBuilder<TInterface, TController> WithCustomHttpClient(IHttpClientProvider httpClientProvider);
        IControllerBasedClientBuilder<TInterface, TController> WithCustomSerializer(ISerializerProvider serializerProvider);
        IControllerBasedClientBuilder<TInterface, TController> WithResiliencePolicy(IResiliencePolicyProvider resiliencePolicyProvider);
        IControllerBasedClientBuilder<TInterface, TController> WithLogging(ILogger<TInterface> logger);
        TInterface Build();
    }
}