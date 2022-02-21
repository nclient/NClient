using System.Text.Json;
using NClient.Common.Helpers;
using NClient.Providers.Serialization.SystemTextJson;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class UsingSystemTextJsonSerializerExtensions
    {
        /// <summary>Sets the System.Text.Json serializer for the client.</summary>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> UsingSystemTextJsonSerialization<TClient, TRequest, TResponse>(
            this INClientSerializationBuilder<TClient, TRequest, TResponse> serializationBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(serializationBuilder, nameof(serializationBuilder));

            return serializationBuilder.UsingCustomSerializer(new SystemTextJsonSerializerProvider());
        }

        /// <summary>Sets the System.Text.Json serializer for the client.</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="jsonSerializerOptions">The settings to be used with the Newtonsoft.Json serializer.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> UsingSystemTextJsonSerialization<TClient, TRequest, TResponse>(
            this INClientSerializationBuilder<TClient, TRequest, TResponse> serializationBuilder,
            JsonSerializerOptions jsonSerializerOptions)
            where TClient : class
        {
            Ensure.IsNotNull(serializationBuilder, nameof(serializationBuilder));
            Ensure.IsNotNull(jsonSerializerOptions, nameof(jsonSerializerOptions));

            return serializationBuilder.UsingCustomSerializer(new SystemTextJsonSerializerProvider(jsonSerializerOptions));
        }

        /// <summary>Sets the System.Text.Json serializer for the client.</summary>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> UsingSystemTextJsonSerialization<TRequest, TResponse>(
            this INClientFactorySerializationBuilder<TRequest, TResponse> serializationBuilder)
        {
            Ensure.IsNotNull(serializationBuilder, nameof(serializationBuilder));

            return serializationBuilder.UsingCustomSerializer(new SystemTextJsonSerializerProvider());
        }

        /// <summary>Sets the System.Text.Json serializer for the client.</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="jsonSerializerOptions">The settings to be used with the Newtonsoft.Json serializer.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> UsingSystemTextJsonSerialization<TRequest, TResponse>(
            this INClientFactorySerializationBuilder<TRequest, TResponse> serializationBuilder,
            JsonSerializerOptions jsonSerializerOptions)
        {
            Ensure.IsNotNull(serializationBuilder, nameof(serializationBuilder));
            Ensure.IsNotNull(jsonSerializerOptions, nameof(jsonSerializerOptions));

            return serializationBuilder.UsingCustomSerializer(new SystemTextJsonSerializerProvider(jsonSerializerOptions));
        }
    }
}
