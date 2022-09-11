using NClient.Common.Helpers;
using NClient.Providers.Api;
using NClient.Providers.Host;

namespace NClient.Standalone.ClientProxy.Building
{
    internal class NClientApiBuilder<TClient> : INClientApiBuilder<TClient>
        where TClient : class
    {
        private readonly IHost? _host;
        
        public NClientApiBuilder()
        {
        }
        
        public NClientApiBuilder(IHost? host)
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
