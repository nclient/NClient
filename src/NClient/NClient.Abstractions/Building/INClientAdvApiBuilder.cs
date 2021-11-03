using NClient.Providers.Api;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface INClientAdvApiBuilder<TClient> : INClientApiBuilder<TClient> 
        where TClient : class
    {
        // TODO: doc
        INClientAdvTransportBuilder<TClient> UsingCustomApi(IRequestBuilderProvider requestBuilderProvider);
    }
}
