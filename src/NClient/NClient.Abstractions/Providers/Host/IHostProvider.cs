namespace NClient.Providers.Host
{
    /// <summary>
    /// Provides the interface for retrieving host for client requests
    /// </summary>
    public interface IHostProvider
    {
        /// <summary>
        /// Returns a host for client requests
        /// </summary>
        public IHost Create(IToolset toolset);
    }
}
