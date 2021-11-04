using NClient.Common.Helpers;
using NClient.Providers.Serialization;
using NClient.Providers.Serialization.Json.Newtonsoft;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class SerializerBuilderExtensions
    {
        /// <summary>
        /// Sets Newtonsoft.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientSerializerBuilder"></param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> UsingNewtonsoftJsonSerializer<TClient, TRequest, TResponse>(
            this INClientSerializerBuilder<TClient, TRequest, TResponse> clientSerializerBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(clientSerializerBuilder, nameof(clientSerializerBuilder));

            return clientSerializerBuilder.AsAdvanced()
                .UsingCustomSerializer(new NewtonsoftJsonSerializerProvider())
                .AsBasic();
        }
        
        /// <summary>
        /// Sets Newtonsoft.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="factorySerializerBuilder"></param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> UsingNewtonsoftJsonSerializer<TRequest, TResponse>(
            this INClientFactorySerializerBuilder<TRequest, TResponse> factorySerializerBuilder)
        {
            Ensure.IsNotNull(factorySerializerBuilder, nameof(factorySerializerBuilder));

            return factorySerializerBuilder.UsingCustomSerializer(new NewtonsoftJsonSerializerProvider());
        }

        /// <summary>
        /// Sets Newtonsoft.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientSerializerBuilder"></param>
        /// <param name="jsonSerializerSettings">The settings to be used with <see cref="JsonSerializer"/>.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> UsingNewtonsoftJsonSerializer<TClient, TRequest, TResponse>(
            this INClientSerializerBuilder<TClient, TRequest, TResponse> clientSerializerBuilder,
            JsonSerializerSettings jsonSerializerSettings)
            where TClient : class
        {
            Ensure.IsNotNull(clientSerializerBuilder, nameof(clientSerializerBuilder));
            Ensure.IsNotNull(jsonSerializerSettings, nameof(jsonSerializerSettings));

            return clientSerializerBuilder.AsAdvanced()
                .UsingCustomSerializer(new NewtonsoftJsonSerializerProvider(jsonSerializerSettings))
                .AsBasic();
        }
        
        /// <summary>
        /// Sets Newtonsoft.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="factorySerializerBuilder"></param>
        /// <param name="jsonSerializerSettings">The settings to be used with <see cref="JsonSerializer"/>.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> UsingNewtonsoftJsonSerializer<TRequest, TResponse>(
            this INClientFactorySerializerBuilder<TRequest, TResponse> factorySerializerBuilder,
            JsonSerializerSettings jsonSerializerSettings)
        {
            Ensure.IsNotNull(factorySerializerBuilder, nameof(factorySerializerBuilder));
            Ensure.IsNotNull(jsonSerializerSettings, nameof(jsonSerializerSettings));

            return factorySerializerBuilder.UsingCustomSerializer(new NewtonsoftJsonSerializerProvider(jsonSerializerSettings));
        }
    }
}
