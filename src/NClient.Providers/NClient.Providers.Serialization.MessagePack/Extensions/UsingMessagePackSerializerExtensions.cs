using NClient.Common.Helpers;
using NClient.Providers.Serialization.MessagePack;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class UsingMessagePackSerializerExtensions
    {
        /// <summary>Sets the MessagePack serializer for the client.</summary>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> UsingMessagePackSerialization<TClient, TRequest, TResponse>(
            this INClientSerializationBuilder<TClient, TRequest, TResponse> serializationBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(serializationBuilder, nameof(serializationBuilder));

            return serializationBuilder.UsingCustomSerializer(new MessagePackSerializerProvider());
        }

        /// <summary>Sets the MessagePack serializer for the client.</summary>
        /// <param name="serializationBuilder"></param>
        /// <param name="messagePackSerializerSettings">The settings to be used with the MessagePack serializer.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> UsingMessagePackSerialization<TClient, TRequest, TResponse>(
            this INClientSerializationBuilder<TClient, TRequest, TResponse> serializationBuilder,
            MessagePackSerializerSettings messagePackSerializerSettings)
            where TClient : class
        {
            Ensure.IsNotNull(serializationBuilder, nameof(serializationBuilder));
            Ensure.IsNotNull(messagePackSerializerSettings, nameof(messagePackSerializerSettings));

            return serializationBuilder
                .UsingCustomSerializer(new MessagePackSerializerProvider(messagePackSerializerSettings));
        }

        /// <summary>Sets the MessagePack serializer for the client.</summary>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> UsingMessagePackSerialization<TRequest, TResponse>(
            this INClientFactorySerializationBuilder<TRequest, TResponse> serializationBuilder)
        {
            Ensure.IsNotNull(serializationBuilder, nameof(serializationBuilder));

            return serializationBuilder.UsingCustomSerializer(new MessagePackSerializerProvider());
        }

        /// <summary>Sets the MessagePack serializer for the client.</summary>
        /// <param name="serializationBuilder"></param>
        /// <param name="messagePackSerializerSettings">The settings to be used with the MessagePack serializer.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> UsingMessagePackSerialization<TRequest, TResponse>(
            this INClientFactorySerializationBuilder<TRequest, TResponse> serializationBuilder,
            MessagePackSerializerSettings messagePackSerializerSettings)
        {
            Ensure.IsNotNull(serializationBuilder, nameof(serializationBuilder));
            Ensure.IsNotNull(messagePackSerializerSettings, nameof(messagePackSerializerSettings));

            return serializationBuilder
                .UsingCustomSerializer(new MessagePackSerializerProvider(messagePackSerializerSettings));
        }
    }
}
