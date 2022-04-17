using System;

namespace NClient.Providers.Authorization
{
    /// <summary>Provides the authentication interface for retrieving tokens for client authentication.</summary>
    public interface ITokens
    {
        /// <summary>Returns a token string that is associated with the specified URI.</summary>
        /// <param name="uri">The <see cref="T:System.Uri" /> that the client is providing authentication for.</param>
        IToken? TryGetToken(Uri uri);
    }
}
