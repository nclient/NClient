using NClient.Abstractions.Serialization;

namespace NClient.Abstractions.HttpClients
{
    public interface IHttpClientProvider
    {
        IHttpClient Create(ISerializer serializer);
    }
}
