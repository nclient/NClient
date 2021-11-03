using NClient.Common.Helpers;
using NClient.Providers.Serialization;
using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building
{
    internal class NClientSerializerBuilder<TClient, TRequest, TResponse> : INClientAdvancedSerializerBuilder<TClient, TRequest, TResponse>
        where TClient : class
    {
        private readonly BuilderContext<TRequest, TResponse> _context;
        
        public NClientSerializerBuilder(BuilderContext<TRequest, TResponse> context)
        {
            _context = context;
        }
        
        public INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> UsingCustomSerializer(ISerializerProvider serializerProvider)
        {
            Ensure.IsNotNull(serializerProvider, nameof(serializerProvider));
            
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_context
                .WithSerializer(serializerProvider));
        }
    }
}
