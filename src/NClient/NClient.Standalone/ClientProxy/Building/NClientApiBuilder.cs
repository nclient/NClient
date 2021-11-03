using NClient.Common.Helpers;
using NClient.Providers.Api;

namespace NClient.Standalone.ClientProxy.Building
{
    internal class NClientApiBuilder<TClient> : INClientAdvancedApiBuilder<TClient> 
        where TClient : class
    {
        private readonly string _host;
        
        public NClientApiBuilder(string host)
        {
            _host = host;
        }
        
        public INClientAdvancedTransportBuilder<TClient> UsingCustomApi(IRequestBuilderProvider requestBuilderProvider)
        {
            Ensure.IsNotNull(requestBuilderProvider, nameof(requestBuilderProvider));
            
            return new NClientTransportBuilder<TClient>(_host, requestBuilderProvider);
        }
    }
}
