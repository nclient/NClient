using NClient.Common.Helpers;
using NClient.Providers.Serialization.ProtobufNet;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class UsingProtobufNetSerializationExtensions
    {
        /// <summary>Sets the ProtobufNet serializer for the client.</summary>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> UsingProtobufNetSerialization<TClient, TRequest, TResponse>(
            this INClientSerializationBuilder<TClient, TRequest, TResponse> serializationBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(serializationBuilder, nameof(serializationBuilder));

            return serializationBuilder.UsingCustomSerializer(new ProtobufNetSerializerProvider());
        }

        /// <summary>Sets the ProtobufNet serializer for the client.</summary>
        /// <param name="serializationBuilder"></param>
        /// <param name="protobufNetSerializerSettings">The settings to be used with the ProtobufNet serializer.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> UsingProtobufNetSerialization<TClient, TRequest, TResponse>(
            this INClientSerializationBuilder<TClient, TRequest, TResponse> serializationBuilder,
            ProtobufNetSerializerSettings protobufNetSerializerSettings)
            where TClient : class
        {
            Ensure.IsNotNull(serializationBuilder, nameof(serializationBuilder));
            Ensure.IsNotNull(protobufNetSerializerSettings, nameof(protobufNetSerializerSettings));

            return serializationBuilder
                .UsingCustomSerializer(new ProtobufNetSerializerProvider(protobufNetSerializerSettings));
        }

        /// <summary>Sets the ProtobufNet serializer for the client.</summary>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> UsingProtobufNetSerialization<TRequest, TResponse>(
            this INClientFactorySerializationBuilder<TRequest, TResponse> serializationBuilder)
        {
            Ensure.IsNotNull(serializationBuilder, nameof(serializationBuilder));

            return serializationBuilder.UsingCustomSerializer(new ProtobufNetSerializerProvider());
        }

        /// <summary>Sets the ProtobufNet serializer for the client.</summary>
        /// <param name="serializationBuilder"></param>
        /// <param name="protobufNetSerializerSettings">The settings to be used with the ProtobufNet serializer.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> UsingProtobufNetSerialization<TRequest, TResponse>(
            this INClientFactorySerializationBuilder<TRequest, TResponse> serializationBuilder,
            ProtobufNetSerializerSettings protobufNetSerializerSettings)
        {
            Ensure.IsNotNull(serializationBuilder, nameof(serializationBuilder));
            Ensure.IsNotNull(protobufNetSerializerSettings, nameof(protobufNetSerializerSettings));

            return serializationBuilder
                .UsingCustomSerializer(new ProtobufNetSerializerProvider(protobufNetSerializerSettings));
        }
    }
}
