using System.Text.Json;
using NClient.Common.Helpers;
using NClient.Providers.Serialization;
using NClient.Providers.Serialization.Json.System;

// ReSharper disable once CheckNamespace
namespace NClient
{
    // TODO: check doc
    public static class SystemJsonSerializationExtensions
    {
        /// <summary>
        /// Sets System.Text.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientAdvancedOptionalBuilder"></param>
        public static INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithSystemJsonSerialization<TClient, TRequest, TResponse>(
            this INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> clientAdvancedOptionalBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(clientAdvancedOptionalBuilder, nameof(clientAdvancedOptionalBuilder));

            return clientAdvancedOptionalBuilder.WithCustomSerialization(new SystemJsonSerializerProvider());
        }
        
        /// <summary>
        /// Sets System.Text.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientOptionalBuilder"></param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithSystemJsonSerialization<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(clientOptionalBuilder, nameof(clientOptionalBuilder));

            return WithSystemJsonSerialization(clientOptionalBuilder.AsAdvanced()).AsBasic();
        }
        
        /// <summary>
        /// Sets System.Text.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="factoryOptionalBuilder"></param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithSystemJsonSerialization<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> factoryOptionalBuilder)
        {
            Ensure.IsNotNull(factoryOptionalBuilder, nameof(factoryOptionalBuilder));

            return factoryOptionalBuilder.WithCustomSerialization(new SystemJsonSerializerProvider());
        }

        /// <summary>
        /// Sets System.Text.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientAdvancedOptionalBuilder"></param>
        /// <param name="jsonSerializerOptions">The options to be used with <see cref="JsonSerializer"/>.</param>
        public static INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithSystemJsonSerialization<TClient, TRequest, TResponse>(
            this INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> clientAdvancedOptionalBuilder,
            JsonSerializerOptions jsonSerializerOptions)
            where TClient : class
        {
            Ensure.IsNotNull(clientAdvancedOptionalBuilder, nameof(clientAdvancedOptionalBuilder));
            Ensure.IsNotNull(jsonSerializerOptions, nameof(jsonSerializerOptions));

            return clientAdvancedOptionalBuilder.WithCustomSerialization(new SystemJsonSerializerProvider(jsonSerializerOptions));
        }
        
        /// <summary>
        /// Sets System.Text.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientOptionalBuilder"></param>
        /// <param name="jsonSerializerOptions">The options to be used with <see cref="JsonSerializer"/>.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithSystemJsonSerialization<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder,
            JsonSerializerOptions jsonSerializerOptions)
            where TClient : class
        {
            Ensure.IsNotNull(clientOptionalBuilder, nameof(clientOptionalBuilder));
            Ensure.IsNotNull(jsonSerializerOptions, nameof(jsonSerializerOptions));

            return WithSystemJsonSerialization(clientOptionalBuilder.AsAdvanced(), jsonSerializerOptions).AsBasic();
        }
        
        /// <summary>
        /// Sets System.Text.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="factoryOptionalBuilder"></param>
        /// <param name="jsonSerializerOptions">The options to be used with <see cref="JsonSerializer"/>.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithSystemJsonSerialization<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> factoryOptionalBuilder,
            JsonSerializerOptions jsonSerializerOptions)
        {
            Ensure.IsNotNull(factoryOptionalBuilder, nameof(factoryOptionalBuilder));
            Ensure.IsNotNull(jsonSerializerOptions, nameof(jsonSerializerOptions));

            return factoryOptionalBuilder.WithCustomSerialization(new SystemJsonSerializerProvider(jsonSerializerOptions));
        }
    }
}
