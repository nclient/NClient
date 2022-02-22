using NClient.Providers.Api;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface INClientApiBuilder<TClient> where TClient : class
    {
        INClientTransportBuilder<TClient> UsingCustomApi(IRequestBuilderProvider requestBuilderProvider);
    }
}
