using NClient.Common.Helpers;
using NClient.Providers.Api;

namespace NClient.Standalone.ClientProxy.Building.Factory
{
    internal class NClientFactoryApiBuilder
        : INClientFactoryAdvancedApiBuilder, INClientFactoryApiBuilder
    {
        private readonly string _factoryName;
        
        public NClientFactoryApiBuilder(string factoryName)
        {
            _factoryName = factoryName;
        }
        
        public INClientFactoryAdvancedTransportBuilder UsingCustomApi(IRequestBuilderProvider requestBuilderProvider)
        {
            Ensure.IsNotNull(requestBuilderProvider, nameof(requestBuilderProvider));
            
            return new NClientFactoryTransportBuilder(_factoryName, requestBuilderProvider);
        }
    }
}
