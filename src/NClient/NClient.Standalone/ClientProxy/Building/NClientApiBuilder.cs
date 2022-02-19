using System;
using NClient.Common.Helpers;
using NClient.Providers.Api;

namespace NClient.Standalone.ClientProxy.Building
{
    internal class NClientApiBuilder<TClient> : INClientApiBuilder<TClient>
        where TClient : class
    {
        private readonly Uri _baseUri;
        
        public NClientApiBuilder(Uri baseUri)
        {
            _baseUri = baseUri;
        }
        
        public INClientTransportBuilder<TClient> UsingCustomApi(IRequestBuilderProvider requestBuilderProvider)
        {
            Ensure.IsNotNull(requestBuilderProvider, nameof(requestBuilderProvider));
            
            return new NClientTransportBuilder<TClient>(_baseUri, requestBuilderProvider);
        }
    }
}
