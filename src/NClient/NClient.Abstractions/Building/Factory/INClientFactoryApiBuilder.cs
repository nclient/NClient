using NClient.Providers.Api;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface INClientFactoryApiBuilder
    {
        INClientFactoryTransportBuilder UsingCustomApi(IRequestBuilderProvider requestBuilderProvider);
    }
}
