namespace NClient.Providers.Authorization
{
    /// <summary>The access token for client authorization.</summary>
    public class AccessToken : IAccessToken
    {
        /// <summary>The scheme to use for authorization.</summary>
        public string Scheme { get; }
        
        /// <summary>The access token value.</summary>
        public string Value { get; }
        
        /// <summary>Creates the access token for client authorization.</summary>
        /// <param name="scheme">The scheme to use for authorization.</param>
        /// <param name="value">The access token value.</param>
        public AccessToken(string scheme, string value)
        {
            Scheme = scheme;
            Value = value;
        }
    }
}
