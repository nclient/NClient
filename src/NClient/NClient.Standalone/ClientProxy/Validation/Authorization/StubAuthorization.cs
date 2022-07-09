using System.Threading;
using System.Threading.Tasks;
using NClient.Providers.Authorization;

namespace NClient.Standalone.ClientProxy.Validation.Authorization
{
    internal class StubAuthorization : IAuthorization
    {
        public Task<IAccessTokens?> TryGetAccessTokensAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult<IAccessTokens?>(null);
        }
    }
}
