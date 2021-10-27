using NClient.Providers.Api;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface INClientFactoryApiBuilder<TRequest, TResponse>
    {
        // TODO: doc
        INClientFactorySerializerBuilder<TRequest, TResponse> UsingCustomApi(IRequestBuilderProvider requestBuilderProvider);
    }
}
