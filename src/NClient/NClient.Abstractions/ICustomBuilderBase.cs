using Microsoft.Extensions.Logging;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;

namespace NClient.Abstractions
{
    public interface ICustomBuilderBase<TBuilder, TResult>
    {
        TBuilder WithCustomHttpClient(IHttpClientProvider httpClientProvider);
        TBuilder WithCustomSerializer(ISerializerProvider serializerProvider);
        TBuilder WithResiliencePolicy(IResiliencePolicyProvider resiliencePolicyProvider);
        TResult Build();
    }
}