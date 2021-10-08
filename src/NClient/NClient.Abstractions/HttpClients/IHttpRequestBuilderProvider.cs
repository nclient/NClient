using NClient.Abstractions.Serialization;

namespace NClient.Abstractions.HttpClients
{
    public interface IHttpMessageBuilderProvider<TRequest>
    {
        IHttpRequestBuilder<TRequest> Create(ISerializer serializer);
    }
}
