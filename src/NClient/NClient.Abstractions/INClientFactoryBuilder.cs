namespace NClient
{
    /// <summary>
    /// A builder abstraction used to create the client factory with custom providers.
    /// </summary>
    public interface INClientFactoryBuilder
    {
        // TODO: doc
        INClientFactoryHttpClientBuilder For(string factoryName);
    }
}
