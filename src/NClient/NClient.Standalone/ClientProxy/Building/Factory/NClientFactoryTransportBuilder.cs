using NClient.Common.Helpers;
using NClient.Providers.Api;
using NClient.Providers.Transport;
using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building.Factory
{
    internal class NClientFactoryTransportBuilder : INClientFactoryTransportBuilder
    {
        private readonly string _factoryName;
        private readonly IRequestBuilderProvider _requestBuilderProvider;

        public NClientFactoryTransportBuilder(string factoryName, IRequestBuilderProvider requestBuilderProvider)
        {
            _factoryName = factoryName;
            _requestBuilderProvider = requestBuilderProvider;
        }
        
        public INClientFactorySerializerBuilder<TRequest, TResponse> UsingCustomTransport<TRequest, TResponse>(
            ITransportProvider<TRequest, TResponse> transportProvider, 
            ITransportMessageBuilderProvider<TRequest, TResponse> transportMessageBuilderProvider)
        {
            Ensure.IsNotNull(transportProvider, nameof(transportProvider));
            Ensure.IsNotNull(transportMessageBuilderProvider, nameof(transportMessageBuilderProvider));
            
            return new NClientFactorySerializerBuilder<TRequest, TResponse>(_factoryName, new BuilderContext<TRequest, TResponse>()
                .WithRequestBuilderProvider(_requestBuilderProvider)
                .WithHttpClientProvider(transportProvider, transportMessageBuilderProvider));
        }
    }
}
