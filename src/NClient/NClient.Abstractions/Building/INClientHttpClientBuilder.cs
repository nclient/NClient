using NClient.Providers.Transport;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface INClientHttpClientBuilder<TClient> where TClient : class
    {
        /// <summary>
        /// Sets custom <see cref="ITransportProvider{TRequest,TResponse}"/> used to create instances of <see cref="ITransport{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="transportProvider">The provider that can create instances of <see cref="ITransport{TRequest,TResponse}"/>.</param>
        /// <param name="transportMessageBuilderProvider">The provider that can create instances of <see cref="ITransportMessageBuilder{TRequest,TResponse}"/>.</param>
        INClientSerializerBuilder<TClient, TRequest, TResponse> UsingCustomHttpClient<TRequest, TResponse>(
            ITransportProvider<TRequest, TResponse> transportProvider,
            ITransportMessageBuilderProvider<TRequest, TResponse> transportMessageBuilderProvider);
    }
}
