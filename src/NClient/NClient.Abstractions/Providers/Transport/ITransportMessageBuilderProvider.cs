using NClient.Providers.Serialization;

namespace NClient.Providers.Transport
{
    public interface ITransportMessageBuilderProvider<TRequest, TResponse>
    {
        ITransportMessageBuilder<TRequest, TResponse> Create(ISerializer serializer);
    }
}
