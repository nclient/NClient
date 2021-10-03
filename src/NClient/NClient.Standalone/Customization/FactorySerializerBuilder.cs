using NClient.Abstractions.Builders;
using NClient.Abstractions.Serialization;
using NClient.Common.Helpers;
using NClient.Customization.Context;

namespace NClient.Customization
{
    public class SerializerBuilder<TClient, TRequest, TResponse> : INClientSerializerBuilder<TClient, TRequest, TResponse>
        where TClient : class
    {
        private readonly CustomizerContext<TRequest, TResponse> _context;
        
        public SerializerBuilder(CustomizerContext<TRequest, TResponse> context)
        {
            _context = context;
        }
        
        public INClientOptionalBuilder<TClient, TRequest, TResponse> UsingCustomSerializer(ISerializerProvider serializerProvider)
        {
            Ensure.IsNotNull(serializerProvider, nameof(serializerProvider));

            _context.SetSerializer(serializerProvider);
            return new ClientOptionalBuilder<TClient, TRequest, TResponse>(_context);
        }
    }
    
    public class FactorySerializerBuilder<TRequest, TResponse> : INClientFactorySerializerBuilder<TRequest, TResponse>
    {
        private readonly string _factoryName;
        private readonly CustomizerContext<TRequest, TResponse> _context;
        
        public FactorySerializerBuilder(string factoryName, CustomizerContext<TRequest, TResponse> context)
        {
            _factoryName = factoryName;
            _context = context;
        }
        
        public INClientFactoryOptionalBuilder<TRequest, TResponse> UsingCustomSerializer(ISerializerProvider serializerProvider)
        {
            Ensure.IsNotNull(serializerProvider, nameof(serializerProvider));

            _context.SetSerializer(serializerProvider);
            return new FactoryOptionalBuilder<TRequest, TResponse>(_factoryName, _context);
        }
    }
}
