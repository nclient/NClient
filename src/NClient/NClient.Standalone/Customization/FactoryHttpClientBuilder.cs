using NClient.Abstractions.Builders;
using NClient.Abstractions.HttpClients;
using NClient.Common.Helpers;
using NClient.Customization.Context;

namespace NClient.Customization
{
    internal class HttpClientBuilder<TClient> : INClientHttpClientBuilder<TClient> where TClient : class
    {
        private readonly string _host;
        
        public HttpClientBuilder(string host)
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
            return new SerializerBuilder<TClient, TRequest, TResponse>(context);
        }
    }
    
    public class FactoryHttpClientBuilder : INClientFactoryHttpClientBuilder
    {
        private readonly string _factoryName;
        
        public FactoryHttpClientBuilder(string factoryName)
        {
            _factoryName = factoryName;
        }
        
        public INClientFactorySerializerBuilder<TRequest, TResponse> UsingCustomHttpClient<TRequest, TResponse>(
            IHttpClientProvider<TRequest, TResponse> httpClientProvider, 
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider,
            IHttpClientExceptionFactory<TRequest, TResponse> httpClientExceptionFactory)
        {
            Ensure.IsNotNull(httpClientProvider, nameof(httpClientProvider));
            Ensure.IsNotNull(httpMessageBuilderProvider, nameof(httpMessageBuilderProvider));
            Ensure.IsNotNull(httpClientExceptionFactory, nameof(httpClientExceptionFactory));

            var context = new CustomizerContext<TRequest, TResponse>();
            
            context.SetHttpClientProvider(httpClientProvider, httpMessageBuilderProvider, httpClientExceptionFactory);
            return new FactorySerializerBuilder<TRequest, TResponse>(_factoryName, context);
        }
    }
}
