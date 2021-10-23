


using NClient.Providers.Serialization;

namespace NClient.Providers.Transport
{
    public interface IHttpMessageBuilderProvider<TRequest, TResponse>
    {
        IHttpMessageBuilder<TRequest, TResponse> Create(ISerializer serializer);
    }
}
