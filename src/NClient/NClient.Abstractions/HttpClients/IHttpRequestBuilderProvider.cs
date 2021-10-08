using NClient.Abstractions.Serialization;

namespace NClient.Abstractions.HttpClients
{
    public interface IHttpMessageBuilderProvider<TRequest>
    {
        IHttpMessageBuilder<TRequest> Create(ISerializer serializer);
    }
}
