


using NClient.Providers.Serialization;

namespace NClient.Providers.Transport
{
    /// <summary>
    /// A provider abstraction for a component that can create <see cref="IHttpClient{TRequest,TResponse}"/> instances.
    /// </summary>
    public interface IHttpClientProvider<TRequest, TResponse>
    {
        /// <summary>
        /// Creates and configures an instance of <see cref="IHttpClient{TRequest,TResponse}"/> instance.
        /// </summary>
        /// <param name="serializer">The serializer for serializing a request body.</param>
        IHttpClient<TRequest, TResponse> Create(ISerializer serializer);
    }
}
