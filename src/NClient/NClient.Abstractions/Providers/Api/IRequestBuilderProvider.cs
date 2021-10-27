using NClient.Providers.Serialization;

namespace NClient.Providers.Api
{
    public interface IRequestBuilderProvider
    {
        IRequestBuilder Create(ISerializer serializer);
    }
}
