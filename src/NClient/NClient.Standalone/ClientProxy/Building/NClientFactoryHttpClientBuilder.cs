using NClient.Common.Helpers;
using NClient.Providers.Transport;
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
            ITransportProvider<TRequest, TResponse> transportProvider, 
            ITransportMessageBuilderProvider<TRequest, TResponse> transportMessageBuilderProvider)
        {
            Ensure.IsNotNull(transportProvider, nameof(transportProvider));
            Ensure.IsNotNull(transportMessageBuilderProvider, nameof(transportMessageBuilderProvider));
            
            return new NClientFactorySerializerBuilder<TRequest, TResponse>(_factoryName, new BuilderContext<TRequest, TResponse>()
                .WithHttpClientProvider(transportProvider, transportMessageBuilderProvider));
        }
    }
}
