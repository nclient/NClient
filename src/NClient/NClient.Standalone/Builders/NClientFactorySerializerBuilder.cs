using NClient.Abstractions.Builders;
using NClient.Abstractions.Serialization;
using NClient.Common.Helpers;
using NClient.Standalone.Builders.Context;

namespace NClient.Standalone.Builders
{
    internal class NClientFactorySerializerBuilder<TRequest, TResponse> : INClientFactorySerializerBuilder<TRequest, TResponse>
    {
        private readonly string _factoryName;
        private readonly BuilderContext<TRequest, TResponse> _context;
        
        public NClientFactorySerializerBuilder(string factoryName, BuilderContext<TRequest, TResponse> context)
        {
            _factoryName = factoryName;
            _context = context;
        }
        
        public INClientFactoryOptionalBuilder<TRequest, TResponse> UsingCustomSerializer(ISerializerProvider serializerProvider)
        {
            Ensure.IsNotNull(serializerProvider, nameof(serializerProvider));
            
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context
                .WithSerializer(serializerProvider));
        }
    }
}
