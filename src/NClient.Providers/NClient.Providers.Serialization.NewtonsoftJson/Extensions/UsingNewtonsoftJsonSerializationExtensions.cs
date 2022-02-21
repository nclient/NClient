using NClient.Common.Helpers;
using NClient.Providers.Serialization.NewtonsoftJson;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class UsingNewtonsoftJsonSerializationExtensions
    {
        /// <summary>Sets the Newtonsoft.Json serializer for the client.</summary>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> UsingNewtonsoftJsonSerialization<TClient, TRequest, TResponse>(
            this INClientSerializationBuilder<TClient, TRequest, TResponse> serializationBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(serializationBuilder, nameof(serializationBuilder));

            return serializationBuilder.UsingCustomSerializer(new NewtonsoftJsonSerializerProvider());
        }

        /// <summary>Sets the Newtonsoft.Json serializer for the client.</summary>
        /// <param name="serializationBuilder"></param>
        /// <param name="jsonSerializerSettings">The settings to be used with the Newtonsoft.Json serializer.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> UsingNewtonsoftJsonSerialization<TClient, TRequest, TResponse>(
            this INClientSerializationBuilder<TClient, TRequest, TResponse> serializationBuilder,
            JsonSerializerSettings jsonSerializerSettings)
            where TClient : class
        {
            Ensure.IsNotNull(serializationBuilder, nameof(serializationBuilder));
            Ensure.IsNotNull(jsonSerializerSettings, nameof(jsonSerializerSettings));

            return serializationBuilder
                .UsingCustomSerializer(new NewtonsoftJsonSerializerProvider(jsonSerializerSettings));
        }

        /// <summary>Sets the Newtonsoft.Json serializer for the client.</summary>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> UsingNewtonsoftJsonSerialization<TRequest, TResponse>(
            this INClientFactorySerializationBuilder<TRequest, TResponse> serializationBuilder)
        {
            Ensure.IsNotNull(serializationBuilder, nameof(serializationBuilder));

            return serializationBuilder.UsingCustomSerializer(new NewtonsoftJsonSerializerProvider());
        }

        /// <summary>Sets the Newtonsoft.Json serializer for the client.</summary>
        /// <param name="serializationBuilder"></param>
        /// <param name="jsonSerializerSettings">The settings to be used with the Newtonsoft.Json serializer.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> UsingNewtonsoftJsonSerialization<TRequest, TResponse>(
            this INClientFactorySerializationBuilder<TRequest, TResponse> serializationBuilder,
            JsonSerializerSettings jsonSerializerSettings)
        {
            Ensure.IsNotNull(serializationBuilder, nameof(serializationBuilder));
            Ensure.IsNotNull(jsonSerializerSettings, nameof(jsonSerializerSettings));

            return serializationBuilder
                .UsingCustomSerializer(new NewtonsoftJsonSerializerProvider(jsonSerializerSettings));
        }
    }
}
