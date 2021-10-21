using NClient.Abstractions.Providers.HttpClient;

namespace NClient.Abstractions.Building
{
    public interface INClientHttpClientBuilder<TClient> where TClient : class
    {
        /// <summary>
        /// Sets custom <see cref="IHttpClientProvider{TRequest,TResponse}"/> used to create instances of <see cref="IHttpClient"/>.
        /// </summary>
        /// <param name="httpClientProvider">The provider that can create instances of <see cref="IHttpClient"/>.</param>
        /// <param name="httpMessageBuilderProvider">The provider that can create instances of <see cref="IHttpMessageBuilder"/>.</param>
        INClientSerializerBuilder<TClient, TRequest, TResponse> UsingCustomHttpClient<TRequest, TResponse>(
            IHttpClientProvider<TRequest, TResponse> httpClientProvider,
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider);
    }
}
