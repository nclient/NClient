using NClient.Providers.Transport;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface INClientFactoryTransportBuilder
    {
        /// <summary>
        /// Sets custom <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instances of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="transportProvider">The provider that can create instances of <see cref="ITransport{TRequest,TResponse}"/>.</param>
        /// <param name="transportMessageBuilderProvider">The provider that can create instances of <see cref="ITransportMessageBuilder{TRequest,TResponse}"/>.</param>
        INClientFactorySerializerBuilder<TRequest, TResponse> UsingCustomTransport<TRequest, TResponse>(
            ITransportProvider<TRequest, TResponse> transportProvider,
            ITransportMessageBuilderProvider<TRequest, TResponse> transportMessageBuilderProvider);
    }
}
