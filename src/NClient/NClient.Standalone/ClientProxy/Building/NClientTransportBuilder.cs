using NClient.Common.Helpers;
using NClient.Providers.Api;
using NClient.Providers.Host;
using NClient.Providers.Transport;
using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building
{
    internal class NClientTransportBuilder<TClient> : INClientTransportBuilder<TClient>
        where TClient : class
    {
        private readonly IHost? _host;
        private readonly IRequestBuilderProvider _requestBuilderProvider;

        public NClientTransportBuilder(IHost? host, IRequestBuilderProvider requestBuilderProvider)
        {
            _host = host;
            _requestBuilderProvider = requestBuilderProvider;
        }
        
        public INClientSerializationBuilder<TClient, TRequest, TResponse> UsingCustomTransport<TRequest, TResponse>(
            ITransportProvider<TRequest, TResponse> transportProvider, 
            ITransportRequestBuilderProvider<TRequest, TResponse> transportRequestBuilderProvider,
            IResponseBuilderProvider<TRequest, TResponse> responseBuilderProvider)
        {
            Ensure.IsNotNull(transportProvider, nameof(transportProvider));
            Ensure.IsNotNull(transportRequestBuilderProvider, nameof(transportRequestBuilderProvider));
            Ensure.IsNotNull(responseBuilderProvider, nameof(responseBuilderProvider));
            
            return new NClientSerializationBuilder<TClient, TRequest, TResponse>(new BuilderContext<TRequest, TResponse>()
                .WithHost(_host)
                .WithRequestBuilderProvider(_requestBuilderProvider)
                .WithTransport(transportProvider, transportRequestBuilderProvider, responseBuilderProvider));
        }
    }
}
