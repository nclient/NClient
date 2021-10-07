using NClient.Abstractions.Builders;
using NClient.Abstractions.Serialization;
using NClient.Common.Helpers;
using NClient.Standalone.Builders.Context;

namespace NClient.Standalone.Builders
{
    internal class NClientSerializerBuilder<TClient, TRequest, TResponse> : INClientSerializerBuilder<TClient, TRequest, TResponse>
        where TClient : class
    {
        private readonly BuilderContext<TRequest, TResponse> _context;
        
        public NClientSerializerBuilder(BuilderContext<TRequest, TResponse> context)
        {
            _context = context;
        }
        
        public INClientOptionalBuilder<TClient, TRequest, TResponse> UsingCustomSerializer(ISerializerProvider serializerProvider)
        {
            Ensure.IsNotNull(serializerProvider, nameof(serializerProvider));
            
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithSerializer(serializerProvider));
        }
    }
}
