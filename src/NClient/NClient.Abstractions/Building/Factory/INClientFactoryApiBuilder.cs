using NClient.Providers.Api;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface INClientFactoryApiBuilder
    {
        // TODO: doc
        INClientFactoryTransportBuilder UsingCustomApi(IRequestBuilderProvider requestBuilderProvider);
    }
}
