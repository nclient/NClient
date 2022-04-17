using System.Threading;
using System.Threading.Tasks;
using NClient.Providers.Authorization;

namespace NClient.Standalone.ClientProxy.Validation.Authorization
{
    internal class StubAuthorization : IAuthorization
    {
        public Task<ITokens?> TryGetTokensAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult<ITokens?>(null);
        }
    }
}
