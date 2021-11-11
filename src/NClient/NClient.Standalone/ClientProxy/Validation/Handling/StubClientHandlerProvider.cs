using NClient.Providers;
using NClient.Providers.Handling;

namespace NClient.Standalone.ClientProxy.Validation.Handling
{
    public class StubClientHandlerProvider<TRequest, TResponse> : IClientHandlerProvider<TRequest, TResponse>
    {
        public IClientHandler<TRequest, TResponse> Create(IToolSet toolSet)
        {
            return new StubClientHandler<TRequest, TResponse>();
        }
    }
}
