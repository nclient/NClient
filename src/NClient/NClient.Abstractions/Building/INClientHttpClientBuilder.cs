using NClient.Providers.Transport;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public interface INClientHttpClientBuilder<TClient> where TClient : class
    {
        /// <summary>
        /// Sets custom <see cref="IHttpClientProvider{TRequest,TResponse}"/> used to create instances of <see cref="IHttpClient{TRequest,TResponse}"/>.
        /// </summary>
        /// <param name="httpClientProvider">The provider that can create instances of <see cref="IHttpClient{TRequest,TResponse}"/>.</param>
        /// <param name="httpMessageBuilderProvider">The provider that can create instances of <see cref="IHttpMessageBuilder{TRequest,TResponse}"/>.</param>
        INClientSerializerBuilder<TClient, TRequest, TResponse> UsingCustomHttpClient<TRequest, TResponse>(
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider);
    }
}
