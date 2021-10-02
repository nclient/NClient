using NClient.Abstractions.Customization;

namespace NClient.Abstractions
{
    /// <summary>
    /// A builder abstraction used to create the client factory with custom providers.
    /// </summary>
    public interface INClientFactoryBuilder<TRequest, TResponse>
    {
        // TODO: doc
        INClientFactoryCustomizer<TRequest, TResponse> For(string factoryName);
    }
}
