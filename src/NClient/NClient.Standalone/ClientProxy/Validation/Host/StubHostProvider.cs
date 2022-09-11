using NClient.Providers;
using NClient.Providers.Host;

namespace NClient.Standalone.ClientProxy.Validation.Host
{
    internal class StubHostProvider : IHostProvider
    {
        public IHost Create(IToolset toolset)
        {
            return new StubHost();
        }
    }
}
