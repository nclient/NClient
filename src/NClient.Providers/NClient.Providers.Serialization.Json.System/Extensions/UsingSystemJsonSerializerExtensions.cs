using System.Text.Json;
using NClient.Common.Helpers;
using NClient.Providers.Serialization;
using NClient.Providers.Serialization.Json.System;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class UsingSystemJsonSerializerExtensions
    {
        /// <summary>
        /// Sets System.Text.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientAdvancedSerializationBuilder"></param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> UsingSystemJsonSerialization<TClient, TRequest, TResponse>(
            this INClientSerializationBuilder<TClient, TRequest, TResponse> clientAdvancedSerializationBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(clientAdvancedSerializationBuilder, nameof(clientAdvancedSerializationBuilder));

            return clientAdvancedSerializationBuilder.UsingCustomSerializer(new SystemJsonSerializerProvider());
        }

        /// <summary>
        /// Sets System.Text.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientSerializationBuilder"></param>
        /// <param name="jsonSerializerOptions">The options to be used with <see cref="JsonSerializer"/>.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> UsingSystemJsonSerialization<TClient, TRequest, TResponse>(
            this INClientSerializationBuilder<TClient, TRequest, TResponse> clientSerializationBuilder,
            JsonSerializerOptions jsonSerializerOptions)
            where TClient : class
        {
            Ensure.IsNotNull(clientSerializationBuilder, nameof(clientSerializationBuilder));
            Ensure.IsNotNull(jsonSerializerOptions, nameof(jsonSerializerOptions));

            return clientSerializationBuilder.UsingCustomSerializer(new SystemJsonSerializerProvider(jsonSerializerOptions));
        }

        /// <summary>
        /// Sets System.Text.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientAdvancedSerializationBuilder"></param>
        public static INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> UsingSystemJsonSerialization<TRequest, TResponse>(
            this INClientFactoryAdvancedSerializationBuilder<TRequest, TResponse> clientAdvancedSerializationBuilder)
        {
            Ensure.IsNotNull(clientAdvancedSerializationBuilder, nameof(clientAdvancedSerializationBuilder));

            return clientAdvancedSerializationBuilder.UsingCustomSerializer(new SystemJsonSerializerProvider());
        }
        
        /// <summary>
        /// Sets System.Text.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientSerializationBuilder"></param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> UsingSystemJsonSerialization<TRequest, TResponse>(
            this INClientFactorySerializationBuilder<TRequest, TResponse> clientSerializationBuilder)
        {
            Ensure.IsNotNull(clientSerializationBuilder, nameof(clientSerializationBuilder));

            return UsingSystemJsonSerialization(clientSerializationBuilder.AsAdvanced()).AsBasic();
        }

        /// <summary>
        /// Sets System.Text.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientSerializationBuilder"></param>
        /// <param name="jsonSerializerOptions">The options to be used with <see cref="JsonSerializer"/>.</param>
        public static INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> UsingSystemJsonSerialization<TRequest, TResponse>(
            this INClientFactoryAdvancedSerializationBuilder<TRequest, TResponse> clientSerializationBuilder,
            JsonSerializerOptions jsonSerializerOptions)
        {
            Ensure.IsNotNull(clientSerializationBuilder, nameof(clientSerializationBuilder));
            Ensure.IsNotNull(jsonSerializerOptions, nameof(jsonSerializerOptions));

            return clientSerializationBuilder.UsingCustomSerializer(new SystemJsonSerializerProvider(jsonSerializerOptions));
        }

        /// <summary>
        /// Sets System.Text.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientSerializationBuilder"></param>
        /// <param name="jsonSerializerOptions">The options to be used with <see cref="JsonSerializer"/>.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> UsingSystemJsonSerialization<TRequest, TResponse>(
            this INClientFactorySerializationBuilder<TRequest, TResponse> clientSerializationBuilder,
            JsonSerializerOptions jsonSerializerOptions)
        {
            Ensure.IsNotNull(clientSerializationBuilder, nameof(clientSerializationBuilder));
            Ensure.IsNotNull(jsonSerializerOptions, nameof(jsonSerializerOptions));

            return UsingSystemJsonSerialization(clientSerializationBuilder.AsAdvanced(), jsonSerializerOptions).AsBasic();
        }
    }
}
