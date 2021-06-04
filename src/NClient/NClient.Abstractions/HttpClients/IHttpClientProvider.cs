using NClient.Abstractions.Serialization;

namespace NClient.Abstractions.HttpClients
{
    /// <summary>
    /// A provider abstraction for a component that can create <see cref="IHttpClient"/> instances.
    /// </summary>
    public interface IHttpClientProvider
    {
        /// <summary>
        /// Creates and configures an instance of <see cref="IHttpClient"/> instance.
        /// </summary>
        /// <param name="serializer">The serializer for serializing a request body.</param>
        IHttpClient Create(ISerializer serializer);
    }
}
