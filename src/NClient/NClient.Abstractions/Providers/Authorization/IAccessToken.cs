namespace NClient.Providers.Authorization
{
    /// <summary>The access token for client authorization.</summary>
    public interface IAccessToken
    {
        /// <summary>The scheme to use for authorization.</summary>
        string Scheme { get; }
        
        /// <summary>The access token value.</summary>
        string Value { get; }
    }
}
