using NClient.Providers.Api;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface INClientFactoryAdvancedApiBuilder
    {
        // TODO: doc
        INClientFactoryAdvancedTransportBuilder UsingCustomApi(IRequestBuilderProvider requestBuilderProvider);
    }
}
