using NClient.Common.Helpers;
using NClient.Providers.Serialization;
using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building.Factory
{
    internal class NClientFactorySerializerBuilder<TRequest, TResponse> 
        : INClientFactoryAdvancedSerializerBuilder<TRequest, TResponse>, INClientFactorySerializerBuilder<TRequest, TResponse>
    {
        private readonly string _factoryName;
        private readonly BuilderContext<TRequest, TResponse> _context;
        
        public NClientFactorySerializerBuilder(string factoryName, BuilderContext<TRequest, TResponse> context)
        {
            _factoryName = factoryName;
            _context = context;
        }
        
        public INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> UsingCustomSerializer(ISerializerProvider serializerProvider)
        {
            Ensure.IsNotNull(serializerProvider, nameof(serializerProvider));
            
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context
                .WithSerializer(serializerProvider));
        }
    }
}
