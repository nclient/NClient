using NClient.Common.Helpers;
using NClient.Providers.Api;
using NClient.Standalone.ClientProxy.Building.Context;

namespace NClient.Standalone.ClientProxy.Building
{
    internal class NClientApiBuilder<TClient, TRequest, TResponse> : INClientApiBuilder<TClient, TRequest, TResponse> 
        where TClient : class
    {
        private readonly BuilderContext<TRequest, TResponse> _context;
        
        public NClientApiBuilder(BuilderContext<TRequest, TResponse> context)
        {
            _context = context;
        }
        
        public INClientSerializerBuilder<TClient, TRequest, TResponse> UsingCustomApi(IRequestBuilderProvider requestBuilderProvider)
        {
            Ensure.IsNotNull(requestBuilderProvider, nameof(requestBuilderProvider));
            
            return new NClientSerializerBuilder<TClient, TRequest, TResponse>(_context
                .WithRequestBuilderProvider(requestBuilderProvider));
        }
    }
}
