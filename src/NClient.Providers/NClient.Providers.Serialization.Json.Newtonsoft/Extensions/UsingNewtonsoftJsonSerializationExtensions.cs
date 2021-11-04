using NClient.Common.Helpers;
using NClient.Providers.Serialization;
using NClient.Providers.Serialization.Json.Newtonsoft;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class UsingNewtonsoftJsonSerializationExtensions
    {
        /// <summary>
        /// Sets Newtonsoft.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientAdvancedSerializationBuilder"></param>
        public static INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> UsingNewtonsoftJsonSerialization<TClient, TRequest, TResponse>(
            this INClientAdvancedSerializationBuilder<TClient, TRequest, TResponse> clientAdvancedSerializationBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(clientAdvancedSerializationBuilder, nameof(clientAdvancedSerializationBuilder));

            return clientAdvancedSerializationBuilder.UsingCustomSerializer(new NewtonsoftJsonSerializerProvider());
        }
        
        /// <summary>
        /// Sets Newtonsoft.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientSerializationBuilder"></param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> UsingNewtonsoftJsonSerialization<TClient, TRequest, TResponse>(
            this INClientSerializationBuilder<TClient, TRequest, TResponse> clientSerializationBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(clientSerializationBuilder, nameof(clientSerializationBuilder));

            return UsingNewtonsoftJsonSerialization(clientSerializationBuilder.AsAdvanced()).AsBasic();
        }

        /// <summary>
        /// Sets Newtonsoft.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientAdvancedSerializationBuilder"></param>
        /// <param name="jsonSerializerSettings">The settings to be used with <see cref="JsonSerializer"/>.</param>
        public static INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> UsingNewtonsoftJsonSerialization<TClient, TRequest, TResponse>(
            this INClientAdvancedSerializationBuilder<TClient, TRequest, TResponse> clientAdvancedSerializationBuilder,
            JsonSerializerSettings jsonSerializerSettings)
            where TClient : class
        {
            Ensure.IsNotNull(clientAdvancedSerializationBuilder, nameof(clientAdvancedSerializationBuilder));
            Ensure.IsNotNull(jsonSerializerSettings, nameof(jsonSerializerSettings));

            return clientAdvancedSerializationBuilder
                .UsingCustomSerializer(new NewtonsoftJsonSerializerProvider(jsonSerializerSettings));
        }
        
        /// <summary>
        /// Sets Newtonsoft.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientSerializationBuilder"></param>
        /// <param name="jsonSerializerSettings">The settings to be used with <see cref="JsonSerializer"/>.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> UsingNewtonsoftJsonSerialization<TClient, TRequest, TResponse>(
            this INClientSerializationBuilder<TClient, TRequest, TResponse> clientSerializationBuilder,
            JsonSerializerSettings jsonSerializerSettings)
            where TClient : class
        {
            Ensure.IsNotNull(clientSerializationBuilder, nameof(clientSerializationBuilder));
            Ensure.IsNotNull(jsonSerializerSettings, nameof(jsonSerializerSettings));

            return UsingNewtonsoftJsonSerialization(clientSerializationBuilder.AsAdvanced(), jsonSerializerSettings).AsBasic();
        }
        
        /// <summary>
        /// Sets Newtonsoft.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientAdvancedSerializationBuilder"></param>
        public static INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> UsingNewtonsoftJsonSerialization<TRequest, TResponse>(
            this INClientFactoryAdvancedSerializationBuilder<TRequest, TResponse> clientAdvancedSerializationBuilder)
        {
            Ensure.IsNotNull(clientAdvancedSerializationBuilder, nameof(clientAdvancedSerializationBuilder));

            return clientAdvancedSerializationBuilder.UsingCustomSerializer(new NewtonsoftJsonSerializerProvider());
        }
        
        /// <summary>
        /// Sets Newtonsoft.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientSerializationBuilder"></param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> UsingNewtonsoftJsonSerialization<TRequest, TResponse>(
            this INClientFactorySerializationBuilder<TRequest, TResponse> clientSerializationBuilder)
        {
            Ensure.IsNotNull(clientSerializationBuilder, nameof(clientSerializationBuilder));

            return UsingNewtonsoftJsonSerialization(clientSerializationBuilder.AsAdvanced()).AsBasic();
        }

        /// <summary>
        /// Sets Newtonsoft.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientAdvancedSerializationBuilder"></param>
        /// <param name="jsonSerializerSettings">The settings to be used with <see cref="JsonSerializer"/>.</param>
        public static INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> UsingNewtonsoftJsonSerialization<TRequest, TResponse>(
            this INClientFactoryAdvancedSerializationBuilder<TRequest, TResponse> clientAdvancedSerializationBuilder,
            JsonSerializerSettings jsonSerializerSettings)
        {
            Ensure.IsNotNull(clientAdvancedSerializationBuilder, nameof(clientAdvancedSerializationBuilder));
            Ensure.IsNotNull(jsonSerializerSettings, nameof(jsonSerializerSettings));

            return clientAdvancedSerializationBuilder
                .UsingCustomSerializer(new NewtonsoftJsonSerializerProvider(jsonSerializerSettings));
        }
        
        /// <summary>
        /// Sets Newtonsoft.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientSerializationBuilder"></param>
        /// <param name="jsonSerializerSettings">The settings to be used with <see cref="JsonSerializer"/>.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> UsingNewtonsoftJsonSerialization<TRequest, TResponse>(
            this INClientFactorySerializationBuilder<TRequest, TResponse> clientSerializationBuilder,
            JsonSerializerSettings jsonSerializerSettings)
        {
            Ensure.IsNotNull(clientSerializationBuilder, nameof(clientSerializationBuilder));
            Ensure.IsNotNull(jsonSerializerSettings, nameof(jsonSerializerSettings));

            return UsingNewtonsoftJsonSerialization(clientSerializationBuilder.AsAdvanced(), jsonSerializerSettings).AsBasic();
        }
    }
}
