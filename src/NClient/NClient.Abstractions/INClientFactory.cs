namespace NClient
{
    /// <summary>A factory abstraction used to create the client with custom providers.</summary>
    public interface INClientFactory
    {
        public string Name { get; set; }
        
        /// <summary>Sets the main client settings.</summary>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <typeparam name="TClient">The type of interface of controller used to create the client.</typeparam>
        TClient Create<TClient>(string host)
            where TClient : class;
    }
}
