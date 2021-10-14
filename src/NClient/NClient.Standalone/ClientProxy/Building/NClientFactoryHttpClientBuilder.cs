using NClient.Abstractions.Building;
using NClient.Abstractions.HttpClients;
using NClient.Common.Helpers;
using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building
{
    internal class NClientFactoryHttpClientBuilder : INClientFactoryHttpClientBuilder
    {
        private readonly string _factoryName;
        
        public NClientFactoryHttpClientBuilder(string factoryName)
        {
            _factoryName = factoryName;
        }
        
        public INClientFactorySerializerBuilder<TRequest, TResponse> UsingCustomHttpClient<TRequest, TResponse>(
            IHttpClientProvider<TRequest, TResponse> httpClientProvider, 
            IHttpMessageBuilderProvider<TRequest, TResponse> httpMessageBuilderProvider)
        {
            Ensure.IsNotNull(httpClientProvider, nameof(httpClientProvider));
            Ensure.IsNotNull(httpMessageBuilderProvider, nameof(httpMessageBuilderProvider));
            
            return new NClientFactorySerializerBuilder<TRequest, TResponse>(_factoryName, new BuilderContext<TRequest, TResponse>()
                .WithHttpClientProvider(httpClientProvider, httpMessageBuilderProvider));
        }
    }
}
