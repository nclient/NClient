using NClient.Abstractions.Providers.Serialization;

namespace NClient.Abstractions.Providers.Transport
{
    /// <summary>
    /// A provider abstraction for a component that can create <see cref="IHttpClient"/> instances.
    /// </summary>
    public interface IHttpClientProvider<TRequest, TResponse>
    {
        /// <summary>
        /// Creates and configures an instance of <see cref="IHttpClient"/> instance.
        /// </summary>
        /// <param name="serializer">The serializer for serializing a request body.</param>
        IHttpClient<TRequest, TResponse> Create(ISerializer serializer);
    }
}
