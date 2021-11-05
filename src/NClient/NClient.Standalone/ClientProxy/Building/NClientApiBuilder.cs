using NClient.Common.Helpers;
using NClient.Providers.Api;

namespace NClient.Standalone.ClientProxy.Building
{
    internal class NClientApiBuilder<TClient> : INClientApiBuilder<TClient>
        where TClient : class
    {
        private readonly string _host;
        
        public NClientApiBuilder(string host)
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
