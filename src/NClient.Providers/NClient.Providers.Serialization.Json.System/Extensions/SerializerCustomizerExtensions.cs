using System.Text.Json;
using NClient.Abstractions.Builders;
using NClient.Abstractions.Serialization;
using NClient.Common.Helpers;
using NClient.Providers.Serialization.Json.System;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Serialization.System
{
    public static class SerializerCustomizerExtensions
    {
        /// <summary>
        /// Sets System.Text.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientSerializerBuilder"></param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> UsingSystemJsonSerializer<TClient, TRequest, TResponse>(
            this INClientSerializerBuilder<TClient, TRequest, TResponse> clientSerializerBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(clientSerializerBuilder, nameof(clientSerializerBuilder));

            return clientSerializerBuilder.UsingCustomSerializer(new SystemJsonSerializerProvider());
        }
        
        /// <summary>
        /// Sets System.Text.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="factorySerializerBuilder"></param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> UsingSystemJsonSerializer<TRequest, TResponse>(
            this INClientFactorySerializerBuilder<TRequest, TResponse> factorySerializerBuilder)
        {
            Ensure.IsNotNull(factorySerializerBuilder, nameof(factorySerializerBuilder));

            return factorySerializerBuilder.UsingCustomSerializer(new SystemJsonSerializerProvider());
        }

        /// <summary>
        /// Sets System.Text.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="clientSerializerBuilder"></param>
        /// <param name="jsonSerializerOptions">The options to be used with <see cref="JsonSerializer"/>.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> UsingSystemJsonSerializer<TClient, TRequest, TResponse>(
            this INClientSerializerBuilder<TClient, TRequest, TResponse> clientSerializerBuilder,
            JsonSerializerOptions jsonSerializerOptions)
            where TClient : class
        {
            Ensure.IsNotNull(clientSerializerBuilder, nameof(clientSerializerBuilder));
            Ensure.IsNotNull(jsonSerializerOptions, nameof(jsonSerializerOptions));

            return clientSerializerBuilder.UsingCustomSerializer(new SystemJsonSerializerProvider(jsonSerializerOptions));
        }
        
        /// <summary>
        /// Sets System.Text.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="factorySerializerBuilder"></param>
        /// <param name="jsonSerializerOptions">The options to be used with <see cref="JsonSerializer"/>.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> UsingSystemJsonSerializer<TRequest, TResponse>(
            this INClientFactorySerializerBuilder<TRequest, TResponse> factorySerializerBuilder,
            JsonSerializerOptions jsonSerializerOptions)
        {
            Ensure.IsNotNull(factorySerializerBuilder, nameof(factorySerializerBuilder));
            Ensure.IsNotNull(jsonSerializerOptions, nameof(jsonSerializerOptions));

            return factorySerializerBuilder.UsingCustomSerializer(new SystemJsonSerializerProvider(jsonSerializerOptions));
        }
    }
}
