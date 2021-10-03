using NClient.Abstractions.Builders;
using NClient.Abstractions.HttpClients;
using NClient.Builders.Context;
using NClient.Common.Helpers;

namespace NClient.Builders
{
    internal class NClientHttpClientBuilder<TClient> : INClientHttpClientBuilder<TClient> where TClient : class
    {
        private readonly string _host;
        
        public NClientHttpClientBuilder(string host)
        {
            _host = host;
        }
        
        public INClientSerializerBuilder<TClient, TRequest, TResponse> UsingCustomHttpClient<TRequest, TResponse>(
            IHttpClientProvider<TRequest, TResponse> httpClientProvider, 
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider,
            IHttpClientExceptionFactory<TRequest, TResponse> httpClientExceptionFactory)
        {
            Ensure.IsNotNull(httpClientProvider, nameof(httpClientProvider));
            Ensure.IsNotNull(httpMessageBuilderProvider, nameof(httpMessageBuilderProvider));
            Ensure.IsNotNull(httpClientExceptionFactory, nameof(httpClientExceptionFactory));

            var context = new CustomizerContext<TRequest, TResponse>();

            context.SetHost(_host);
            context.SetHttpClientProvider(httpClientProvider, httpMessageBuilderProvider, httpClientExceptionFactory);
            return new NClientSerializerBuilder<TClient, TRequest, TResponse>(context);
        }
    }
}
