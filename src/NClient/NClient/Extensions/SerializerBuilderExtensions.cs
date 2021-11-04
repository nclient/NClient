using NClient.Common.Helpers;
using NClient.Providers.Serialization;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class SerializerBuilderExtensions
    {
        /// <summary>
        /// Sets System.Text.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientAdvancedSerializerBuilder"></param>
        public static INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> UsingJsonSerializer<TClient, TRequest, TResponse>(
            this INClientAdvancedSerializerBuilder<TClient, TRequest, TResponse> clientAdvancedSerializerBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(clientAdvancedSerializerBuilder, nameof(clientAdvancedSerializerBuilder));

            return clientAdvancedSerializerBuilder.UsingSystemJsonSerialization();
        }
        
        /// <summary>
        /// Sets System.Text.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientSerializerBuilder"></param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> UsingJsonSerializer<TClient, TRequest, TResponse>(
            this INClientSerializerBuilder<TClient, TRequest, TResponse> clientSerializerBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(clientSerializerBuilder, nameof(clientSerializerBuilder));

            return clientSerializerBuilder.UsingSystemJsonSerialization();
        }
        
        /// <summary>
        /// Sets System.Text.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="factorySerializerBuilder"></param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> UsingJsonSerializer<TRequest, TResponse>(
            this INClientFactorySerializerBuilder<TRequest, TResponse> factorySerializerBuilder)
        {
            Ensure.IsNotNull(factorySerializerBuilder, nameof(factorySerializerBuilder));

            return factorySerializerBuilder.UsingSystemJsonSerialization();
        }
    }
}
