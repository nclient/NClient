using NClient.Common.Helpers;
using NClient.Providers.Transport;
using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building
{
    internal class NClientHttpClientBuilder<TClient> : INClientHttpClientBuilder<TClient> where TClient : class
    {
        private readonly string _host;
        
        public NClientHttpClientBuilder(string host)
        {
            _host = host;
        }
        
        public INClientSerializerBuilder<TClient, TRequest, TResponse> UsingCustomHttpClient<TRequest, TResponse>(
            ITransportProvider<TRequest, TResponse> transportProvider, 
            ITransportMessageBuilderProvider<TRequest, TResponse> transportMessageBuilderProvider)
        {
            Ensure.IsNotNull(transportProvider, nameof(transportProvider));
            Ensure.IsNotNull(transportMessageBuilderProvider, nameof(transportMessageBuilderProvider));
            
            return new NClientSerializerBuilder<TClient, TRequest, TResponse>(new BuilderContext<TRequest, TResponse>()
                .WithHost(_host)
                .WithHttpClientProvider(transportProvider, transportMessageBuilderProvider));
        }
    }
}
