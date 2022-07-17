namespace NClient.Providers.Authorization
{
    /// <summary>Provides the authentication interface for client authentication.</summary>
    public interface IAuthorizationProvider
    {
        /// <summary>Returns a authentication for client.</summary>
        IAuthorization Create(IToolset toolset);
    }
}
