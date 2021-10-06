using NClient.Abstractions.Builders;
using NClient.Abstractions.HttpClients;
using NClient.Common.Helpers;
using NClient.Standalone.Builders.Context;

namespace NClient.Standalone.Builders
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
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider)
        {
            Ensure.IsNotNull(httpClientProvider, nameof(httpClientProvider));
            Ensure.IsNotNull(httpMessageBuilderProvider, nameof(httpMessageBuilderProvider));

            var context = new BuilderContext<TRequest, TResponse>();

            context.SetHost(_host);
            context.SetHttpClientProvider(httpClientProvider, httpMessageBuilderProvider);
            return new NClientSerializerBuilder<TClient, TRequest, TResponse>(context);
        }
    }
}
