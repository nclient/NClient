using NClient.Providers.Transport;

// ReSharper disable once CheckNamespace
namespace NClient
{
    /// <summary>Setter for a custom providers needed to implement the transport layer.</summary>
    public interface INClientFactoryTransportBuilder
    {
        /// <summary>Sets a custom providers needed to implement the transport layer.</summary>
        /// <param name="transportProvider">The provider for a component that can create transport.</param>
        /// <param name="transportRequestBuilderProvider">The provider that can create builder for transforming a NClient request to transport request.</param>
        /// <param name="responseBuilderProvider">The type of response that is used in the transport implementation.</param>
        /// <typeparam name="TRequest">The type of request that is used in the transport implementation.</typeparam>
        /// <typeparam name="TResponse">The type of response that is used in the transport implementation.</typeparam>
        INClientFactorySerializationBuilder<TRequest, TResponse> UsingCustomTransport<TRequest, TResponse>(
            ITransportProvider<TRequest, TResponse> transportProvider,
            ITransportRequestBuilderProvider<TRequest, TResponse> transportRequestBuilderProvider,
            IResponseBuilderProvider<TRequest, TResponse> responseBuilderProvider);
    }
}
