using NClient.Common.Helpers;
using NClient.Providers.Serialization;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class JsonSerializerExtensions
    {
        /// <summary>
        /// Sets System.Text.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientAdvancedSerializationBuilder"></param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> UsingJsonSerializer<TClient, TRequest, TResponse>(
            this INClientSerializationBuilder<TClient, TRequest, TResponse> clientAdvancedSerializationBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(clientAdvancedSerializationBuilder, nameof(clientAdvancedSerializationBuilder));

            return clientAdvancedSerializationBuilder.UsingSystemJsonSerialization();
        }

        /// <summary>
        /// Sets System.Text.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientAdvancedSerializationBuilder"></param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> UsingJsonSerializer<TRequest, TResponse>(
            this INClientFactorySerializationBuilder<TRequest, TResponse> clientAdvancedSerializationBuilder)
        {
            Ensure.IsNotNull(clientAdvancedSerializationBuilder, nameof(clientAdvancedSerializationBuilder));

            return clientAdvancedSerializationBuilder.UsingSystemJsonSerialization();
        }
    }
}
