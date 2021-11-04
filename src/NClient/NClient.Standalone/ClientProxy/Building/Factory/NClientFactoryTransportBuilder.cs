using NClient.Common.Helpers;
using NClient.Providers.Api;
using NClient.Providers.Transport;
using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building.Factory
{
    internal class NClientFactoryTransportBuilder : INClientFactoryAdvancedTransportBuilder, INClientFactoryTransportBuilder
    {
        private readonly string _factoryName;
        private readonly IRequestBuilderProvider _requestBuilderProvider;

        public NClientFactoryTransportBuilder(string factoryName, IRequestBuilderProvider requestBuilderProvider)
        {
            _factoryName = factoryName;
            _requestBuilderProvider = requestBuilderProvider;
        }
        
        public INClientFactoryAdvancedSerializerBuilder<TRequest, TResponse> UsingCustomTransport<TRequest, TResponse>(
            ITransportProvider<TRequest, TResponse> transportProvider, 
            ITransportRequestBuilderProvider<TRequest, TResponse> transportRequestBuilderProvider,
            IResponseBuilderProvider<TRequest, TResponse> responseBuilderProvider)
        {
            Ensure.IsNotNull(transportProvider, nameof(transportProvider));
            Ensure.IsNotNull(transportRequestBuilderProvider, nameof(transportRequestBuilderProvider));
            Ensure.IsNotNull(responseBuilderProvider, nameof(responseBuilderProvider));
            
            return new NClientFactorySerializerBuilder<TRequest, TResponse>(_factoryName, new BuilderContext<TRequest, TResponse>()
                .WithRequestBuilderProvider(_requestBuilderProvider)
                .WithTransport(transportProvider, transportRequestBuilderProvider, responseBuilderProvider));
        }
    }
}
