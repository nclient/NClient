using NClient.Providers.Api;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface INClientAdvancedApiBuilder<TClient> : INClientApiBuilder<TClient> 
        where TClient : class
    {
        // TODO: doc
        INClientAdvancedTransportBuilder<TClient> UsingCustomApi(IRequestBuilderProvider requestBuilderProvider);
    }
}
