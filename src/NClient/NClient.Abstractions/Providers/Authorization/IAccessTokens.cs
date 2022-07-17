using System;

namespace NClient.Providers.Authorization
{
    /// <summary>Provides the authentication interface for retrieving access tokens for client authentication.</summary>
    public interface IAccessTokens
    {
        /// <summary>Returns an access token string that is associated with the specified URI.</summary>
        /// <param name="uri">The <see cref="T:System.Uri" /> that the client is providing authentication for.</param>
        IAccessToken? TryGet(Uri uri);
    }
}
