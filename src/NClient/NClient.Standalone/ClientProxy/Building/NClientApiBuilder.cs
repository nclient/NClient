using System;
using NClient.Common.Helpers;
using NClient.Providers.Api;

namespace NClient.Standalone.ClientProxy.Building
{
    internal class NClientApiBuilder<TClient> : INClientApiBuilder<TClient>
        where TClient : class
    {
        private readonly Uri _host;
        
        public NClientApiBuilder(Uri host)
        {
            _host = host;
        }
        
        public INClientTransportBuilder<TClient> UsingCustomApi(IRequestBuilderProvider requestBuilderProvider)
        {
            Ensure.IsNotNull(requestBuilderProvider, nameof(requestBuilderProvider));
            
            return new NClientTransportBuilder<TClient>(_host, requestBuilderProvider);
        }
    }
}
