using NClient.Providers;
using NClient.Providers.Authorization;

namespace NClient.Standalone.ClientProxy.Validation.Authorization
{
    internal class StubAuthorizationProvider : IAuthorizationProvider
    {
        public IAuthorization Create(IToolset toolset)
        {
            return new StubAuthorization();
        }
    }
}
