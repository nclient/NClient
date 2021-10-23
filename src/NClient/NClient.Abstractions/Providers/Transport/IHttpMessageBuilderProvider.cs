using NClient.Abstractions.Providers.Serialization;

namespace NClient.Abstractions.Providers.Transport
{
    public interface IHttpMessageBuilderProvider<TRequest, TResponse>
    {
        IHttpMessageBuilder<TRequest, TResponse> Create(ISerializer serializer);
    }
}
