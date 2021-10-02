using NClient.Abstractions.Customization;

namespace NClient.Abstractions
{
    /// <summary>
    /// An builder abstraction used to create the client.
    /// </summary>
    public interface INClientBuilder<TRequest, TResponse>
    {
        /// <summary>
        /// Sets the main client settings.
        /// </summary>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <typeparam name="TInterface">The type of interface of controller used to create the client.</typeparam>
        INClientBuilderCustomizer<TInterface, TRequest, TResponse> For<TInterface>(string host)
            where TInterface : class;
    }
}
