using NClient.Common.Helpers;
using NClient.Providers.Api;
using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building.Factory
{
    internal class NClientFactoryApiBuilder<TRequest, TResponse> : INClientFactoryApiBuilder<TRequest, TResponse>
    {
        private readonly string _factoryName;
        private readonly BuilderContext<TRequest, TResponse> _context;
        
        public NClientFactoryApiBuilder(string factoryName, BuilderContext<TRequest, TResponse> context)
        {
            _factoryName = factoryName;
            _context = context;
        }
        
        public INClientFactorySerializerBuilder<TRequest, TResponse> UsingCustomApi(IRequestBuilderProvider requestBuilderProvider)
        {
            Ensure.IsNotNull(requestBuilderProvider, nameof(requestBuilderProvider));
            
            return new NClientFactorySerializerBuilder<TRequest, TResponse>(_factoryName, _context
                .WithRequestBuilderProvider(requestBuilderProvider));
        }
    }
}
