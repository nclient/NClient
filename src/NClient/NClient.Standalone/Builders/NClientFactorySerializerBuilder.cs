using NClient.Abstractions.Builders;
using NClient.Abstractions.Serialization;
using NClient.Builders.Context;
using NClient.Common.Helpers;

namespace NClient.Builders
{
    internal class NClientFactorySerializerBuilder<TRequest, TResponse> : INClientFactorySerializerBuilder<TRequest, TResponse>
    {
        private readonly string _factoryName;
        private readonly CustomizerContext<TRequest, TResponse> _context;
        
        public NClientFactorySerializerBuilder(string factoryName, CustomizerContext<TRequest, TResponse> context)
        {
            _factoryName = factoryName;
            _context = context;
        }
        
        public INClientFactoryOptionalBuilder<TRequest, TResponse> UsingCustomSerializer(ISerializerProvider serializerProvider)
        {
            Ensure.IsNotNull(serializerProvider, nameof(serializerProvider));

            _context.SetSerializer(serializerProvider);
            return new NClientFactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context);
        }
    }
}
