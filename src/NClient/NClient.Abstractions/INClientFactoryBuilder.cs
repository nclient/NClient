namespace NClient
{
    /// <summary>A builder abstraction used to create the client factory with custom providers.</summary>
    public interface INClientFactoryBuilder
    {
        /// <summary>Sets factory name.</summary>
        /// <param name="factoryName">The factory name.</param>
        INClientFactoryApiBuilder For(string factoryName);
    }
}
