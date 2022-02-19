using System;
using NClient.Common.Helpers;
using NClient.Providers.Api;
using NClient.Providers.Transport;
using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building
{
    internal class NClientTransportBuilder<TClient> : INClientTransportBuilder<TClient>
        where TClient : class
    {
        private readonly Uri _baseUri;
        private readonly IRequestBuilderProvider _requestBuilderProvider;

        public NClientTransportBuilder(Uri baseUri, IRequestBuilderProvider requestBuilderProvider)
        {
            _baseUri = baseUri;
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
                .WithBaseUri(_baseUri)
                .WithRequestBuilderProvider(_requestBuilderProvider)
                .WithTransport(transportProvider, transportRequestBuilderProvider, responseBuilderProvider));
        }
    }
}
