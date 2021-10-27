using NClient.Common.Helpers;
using NClient.Providers.Transport;
using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building.Factory
{
    internal class NClientFactoryTransportBuilder : INClientFactoryTransportBuilder
    {
        private readonly string _factoryName;
        
        public NClientFactoryTransportBuilder(string factoryName)
        {
            _factoryName = factoryName;
        }
        
        public INClientFactoryApiBuilder<TRequest, TResponse> UsingCustomTransport<TRequest, TResponse>(
            ITransportProvider<TRequest, TResponse> transportProvider, 
            ITransportMessageBuilderProvider<TRequest, TResponse> transportMessageBuilderProvider)
        {
            Ensure.IsNotNull(transportProvider, nameof(transportProvider));
            Ensure.IsNotNull(transportMessageBuilderProvider, nameof(transportMessageBuilderProvider));
            
            return new NClientFactoryApiBuilder<TRequest, TResponse>(_factoryName, new BuilderContext<TRequest, TResponse>()
                .WithHttpClientProvider(transportProvider, transportMessageBuilderProvider));
        }
    }
}
