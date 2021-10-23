using NClient.Providers.Handling;

namespace NClient.Standalone.Client.Handling
{
    public class StubClientHandlerProvider<TRequest, TResponse> : IClientHandlerProvider<TRequest, TResponse>
    {
        public IClientHandler<TRequest, TResponse> Create()
        {
            return new StubClientHandler<TRequest, TResponse>();
        }
    }
}
