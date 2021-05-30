using Microsoft.Extensions.Logging;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Serialization;

namespace NClient.Abstractions
{
    public interface IOptionalBuilderBase<TBuilder, TResult>
        where TBuilder : IOptionalBuilderBase<TBuilder, TResult>
    {
        TBuilder WithCustomHttpClient(IHttpClientProvider httpClientProvider);
        TBuilder WithCustomSerializer(ISerializerProvider serializerProvider);
        TBuilder WithResiliencePolicy(IResiliencePolicyProvider resiliencePolicyProvider);
        TBuilder WithLogging(ILoggerFactory loggerFactory);
        TResult Build();
    }
}