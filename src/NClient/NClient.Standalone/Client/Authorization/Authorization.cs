using System.Threading;
using System.Threading.Tasks;
using NClient.Providers.Authorization;

namespace NClient.Standalone.Client.Authorization
{
    internal class Authorization : IAuthorization
    {
        private readonly ITokens? _tokens;

        public Authorization()
        {
        }

        public Authorization(ITokens tokens)
        {
            _tokens = tokens;
        }

        public Task<ITokens?> TryGetTokensAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_tokens);
        }
    }
}
