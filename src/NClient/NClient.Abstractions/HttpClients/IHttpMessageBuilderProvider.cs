using NClient.Abstractions.Serialization;

namespace NClient.Abstractions.HttpClients
{
    public interface IHttpMessageBuilderProvider<TRequest, TResponse>
    {
        IHttpMessageBuilder<TRequest, TResponse> Create(ISerializer serializer);
    }
}
