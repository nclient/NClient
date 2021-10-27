using NClient.Providers.Api;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface INClientApiBuilder<TClient, TRequest, TResponse> where TClient : class
    {
        // TODO: doc
        INClientSerializerBuilder<TClient, TRequest, TResponse> UsingCustomApi(IRequestBuilderProvider requestBuilderProvider);
    }
}
