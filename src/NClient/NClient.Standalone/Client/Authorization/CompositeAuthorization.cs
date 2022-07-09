using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NClient.Providers.Authorization;

namespace NClient.Standalone.Client.Authorization
{
    internal class CompositeAuthorization : IAuthorization
    {
        private readonly IReadOnlyCollection<IAuthorization> _authorizations;
        
        public CompositeAuthorization(IReadOnlyCollection<IAuthorization> authorizations)
        {
            _authorizations = authorizations;
        }
        
        public async Task<IAccessTokens?> TryGetAccessTokensAsync(CancellationToken cancellationToken)
        {
            foreach (var authorization in _authorizations)
            {
                var tokens = await authorization.TryGetAccessTokensAsync(cancellationToken).ConfigureAwait(false);
                if (tokens is not null)
                    return tokens;
            }
            return null;
        }
    }
}
