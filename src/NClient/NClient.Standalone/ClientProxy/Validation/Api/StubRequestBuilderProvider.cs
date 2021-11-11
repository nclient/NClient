using NClient.Providers;
using NClient.Providers.Api;

namespace NClient.Standalone.ClientProxy.Validation.Api
{
    public class StubRequestBuilderProvider : IRequestBuilderProvider
    {
        public IRequestBuilder Create(IToolSet toolSet)
        {
            return new StubRequestBuilder();
        }
    }
}
