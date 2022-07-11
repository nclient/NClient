using System.Threading;
using System.Threading.Tasks;
using NClient.Providers.Authorization;

namespace NClient.Standalone.Client.Authorization
{
    internal class Authorization : IAuthorization
    {
        private readonly IAccessTokens? _accessTokens;

        public Authorization()
        {
        }

        public Authorization(IAccessTokens accessTokens)
        {
            _accessTokens = accessTokens;
        }

        public Task<IAccessTokens?> TryGetAccessTokensAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_accessTokens);
        }
    }
}
