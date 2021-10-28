using NClient.Common.Helpers;
using NClient.Providers.Api;
using NClient.Providers.Transport;
using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building
{
    internal class NClientTransportBuilder<TClient> : INClientTransportBuilder<TClient> where TClient : class
    {
        private readonly string _host;
        private readonly IRequestBuilderProvider _requestBuilderProvider;

        public NClientTransportBuilder(string host, IRequestBuilderProvider requestBuilderProvider)
        {
            _host = host;
            _requestBuilderProvider = requestBuilderProvider;
        }
        
        public INClientSerializerBuilder<TClient, TRequest, TResponse> UsingCustomTransport<TRequest, TResponse>(
            ITransportProvider<TRequest, TResponse> transportProvider, 
            ITransportMessageBuilderProvider<TRequest, TResponse> transportMessageBuilderProvider)
        {
            Ensure.IsNotNull(transportProvider, nameof(transportProvider));
            Ensure.IsNotNull(transportMessageBuilderProvider, nameof(transportMessageBuilderProvider));
            
            return new NClientSerializerBuilder<TClient, TRequest, TResponse>(new BuilderContext<TRequest, TResponse>()
                .WithHost(_host)
                .WithRequestBuilderProvider(_requestBuilderProvider)
                .WithHttpClientProvider(transportProvider, transportMessageBuilderProvider));
        }
    }
}
