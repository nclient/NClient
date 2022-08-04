using NClient.Providers;
using NClient.Providers.Caching;

namespace NClient.Standalone.ClientProxy.Validation.Caching
{
    internal class StubResponseCacheProvider : IResponseCacheProvider
    {
        public IResponseCacheWorker Create(IToolset toolset)
        {
            return new StubResponseCacheWorker();
        }
    }
}
