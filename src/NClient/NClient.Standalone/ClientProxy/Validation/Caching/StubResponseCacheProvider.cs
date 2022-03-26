using NClient.Providers;
using NClient.Providers.Caching;
using NClient.Providers.Transport;

namespace NClient.Standalone.ClientProxy.Validation.Caching
{
    internal class StubResponseCacheProvider : IResponseCacheProvider<IRequest, IResponse>
    {
        public IResponseCacheWorker<IRequest, IResponse> Create(IToolset toolset)
        {
            return new StubResponseCacheWorker();
        }
    }
}
