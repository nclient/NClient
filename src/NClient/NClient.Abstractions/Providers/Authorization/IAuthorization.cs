using System.Threading;
using System.Threading.Tasks;

namespace NClient.Providers.Authorization
{
    /// <summary>Provides the authentication interface for retrieving access tokens for client authentication.</summary>
    public interface IAuthorization
    {
        /// <summary>Returns an access tokens for client authentication.</summary>
        Task<IAccessTokens?> TryGetAccessTokensAsync(CancellationToken cancellationToken);
    }
}
