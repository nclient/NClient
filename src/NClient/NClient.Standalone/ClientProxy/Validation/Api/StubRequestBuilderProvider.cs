using NClient.Providers;
using NClient.Providers.Api;

namespace NClient.Standalone.ClientProxy.Validation.Api
{
    internal class StubRequestBuilderProvider : IRequestBuilderProvider
    {
        public IRequestBuilder Create(IToolset toolset)
        {
            return new StubRequestBuilder();
        }
    }
}
