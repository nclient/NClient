using NClient.Providers.Handling;

namespace NClient.Standalone.ClientProxy.Validator.Handling
{
    public class StubClientHandlerProvider<TRequest, TResponse> : IClientHandlerProvider<TRequest, TResponse>
    {
        public IClientHandler<TRequest, TResponse> Create()
        {
            return new StubClientHandler<TRequest, TResponse>();
        }
    }
}
