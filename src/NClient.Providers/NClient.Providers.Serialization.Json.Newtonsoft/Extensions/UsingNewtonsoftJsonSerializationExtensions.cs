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
        /// <param name="clientAdvancedSerializerBuilder"></param>
        public static INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> UsingNewtonsoftJsonSerialization<TClient, TRequest, TResponse>(
            this INClientAdvancedSerializerBuilder<TClient, TRequest, TResponse> clientAdvancedSerializerBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(clientAdvancedSerializerBuilder, nameof(clientAdvancedSerializerBuilder));

            return clientAdvancedSerializerBuilder.UsingCustomSerializer(new NewtonsoftJsonSerializerProvider());
        }
        
        /// <summary>
        /// Sets Newtonsoft.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientSerializerBuilder"></param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> UsingNewtonsoftJsonSerialization<TClient, TRequest, TResponse>(
            this INClientSerializerBuilder<TClient, TRequest, TResponse> clientSerializerBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(clientSerializerBuilder, nameof(clientSerializerBuilder));

            return UsingNewtonsoftJsonSerialization(clientSerializerBuilder.AsAdvanced()).AsBasic();
        }

        /// <summary>
        /// Sets Newtonsoft.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientAdvancedSerializerBuilder"></param>
        /// <param name="jsonSerializerSettings">The settings to be used with <see cref="JsonSerializer"/>.</param>
        public static INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> UsingNewtonsoftJsonSerialization<TClient, TRequest, TResponse>(
            this INClientAdvancedSerializerBuilder<TClient, TRequest, TResponse> clientAdvancedSerializerBuilder,
            JsonSerializerSettings jsonSerializerSettings)
            where TClient : class
        {
            Ensure.IsNotNull(clientAdvancedSerializerBuilder, nameof(clientAdvancedSerializerBuilder));
            Ensure.IsNotNull(jsonSerializerSettings, nameof(jsonSerializerSettings));

            return clientAdvancedSerializerBuilder
                .UsingCustomSerializer(new NewtonsoftJsonSerializerProvider(jsonSerializerSettings));
        }
        
        /// <summary>
        /// Sets Newtonsoft.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientSerializerBuilder"></param>
        /// <param name="jsonSerializerSettings">The settings to be used with <see cref="JsonSerializer"/>.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> UsingNewtonsoftJsonSerialization<TClient, TRequest, TResponse>(
            this INClientSerializerBuilder<TClient, TRequest, TResponse> clientSerializerBuilder,
            JsonSerializerSettings jsonSerializerSettings)
            where TClient : class
        {
            Ensure.IsNotNull(clientSerializerBuilder, nameof(clientSerializerBuilder));
            Ensure.IsNotNull(jsonSerializerSettings, nameof(jsonSerializerSettings));

            return UsingNewtonsoftJsonSerialization(clientSerializerBuilder.AsAdvanced(), jsonSerializerSettings).AsBasic();
        }
        
        /// <summary>
        /// Sets Newtonsoft.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientAdvancedSerializerBuilder"></param>
        public static INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> UsingNewtonsoftJsonSerialization<TRequest, TResponse>(
            this INClientFactoryAdvancedSerializerBuilder<TRequest, TResponse> clientAdvancedSerializerBuilder)
        {
            Ensure.IsNotNull(clientAdvancedSerializerBuilder, nameof(clientAdvancedSerializerBuilder));

            return clientAdvancedSerializerBuilder.UsingCustomSerializer(new NewtonsoftJsonSerializerProvider());
        }
        
        /// <summary>
        /// Sets Newtonsoft.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientSerializerBuilder"></param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> UsingNewtonsoftJsonSerialization<TRequest, TResponse>(
            this INClientFactorySerializerBuilder<TRequest, TResponse> clientSerializerBuilder)
        {
            Ensure.IsNotNull(clientSerializerBuilder, nameof(clientSerializerBuilder));

            return UsingNewtonsoftJsonSerialization(clientSerializerBuilder.AsAdvanced()).AsBasic();
        }

        /// <summary>
        /// Sets Newtonsoft.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientAdvancedSerializerBuilder"></param>
        /// <param name="jsonSerializerSettings">The settings to be used with <see cref="JsonSerializer"/>.</param>
        public static INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> UsingNewtonsoftJsonSerialization<TRequest, TResponse>(
            this INClientFactoryAdvancedSerializerBuilder<TRequest, TResponse> clientAdvancedSerializerBuilder,
            JsonSerializerSettings jsonSerializerSettings)
        {
            Ensure.IsNotNull(clientAdvancedSerializerBuilder, nameof(clientAdvancedSerializerBuilder));
            Ensure.IsNotNull(jsonSerializerSettings, nameof(jsonSerializerSettings));

            return clientAdvancedSerializerBuilder
                .UsingCustomSerializer(new NewtonsoftJsonSerializerProvider(jsonSerializerSettings));
        }
        
        /// <summary>
        /// Sets Newtonsoft.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientSerializerBuilder"></param>
        /// <param name="jsonSerializerSettings">The settings to be used with <see cref="JsonSerializer"/>.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> UsingNewtonsoftJsonSerialization<TRequest, TResponse>(
            this INClientFactorySerializerBuilder<TRequest, TResponse> clientSerializerBuilder,
            JsonSerializerSettings jsonSerializerSettings)
        {
            Ensure.IsNotNull(clientSerializerBuilder, nameof(clientSerializerBuilder));
            Ensure.IsNotNull(jsonSerializerSettings, nameof(jsonSerializerSettings));

            return UsingNewtonsoftJsonSerialization(clientSerializerBuilder.AsAdvanced(), jsonSerializerSettings).AsBasic();
        }
    }
}
