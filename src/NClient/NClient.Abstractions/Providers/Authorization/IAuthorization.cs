using System.Threading;
using System.Threading.Tasks;

namespace NClient.Providers.Authorization
{
    /// <summary>Provides the authentication interface for retrieving tokens for client authentication.</summary>
    public interface IAuthorization
    {
        /// <summary>Returns a tokens for client authentication.</summary>
        Task<ITokens?> TryGetTokensAsync(CancellationToken cancellationToken);
    }
}
