using NClient.Providers.Serialization;

namespace NClient.Providers.Transport
{
    /// <summary>
    /// A provider abstraction for a component that can create <see cref="ITransport{TRequest,TResponse}"/> instances.
    /// </summary>
    public interface ITransportProvider<TRequest, TResponse>
    {
        /// <summary>
        /// Creates and configures an instance of <see cref="ITransport{TRequest,TResponse}"/> instance.
        /// </summary>
        /// <param name="serializer">The serializer for serializing a request body.</param>
        ITransport<TRequest, TResponse> Create(ISerializer serializer);
    }
}
