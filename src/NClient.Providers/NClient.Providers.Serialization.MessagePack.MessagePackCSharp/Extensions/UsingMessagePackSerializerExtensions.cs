using NClient.Common.Helpers;

namespace NClient.Providers.Serialization.MessagePack.MessagePackCSharp.Extensions
{
    public static class UsingMessagePackSerializerExtensions
    {
        /// <summary>
        /// Sets MessagePack based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="serializationBuilder"></param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> UsingMessagePackSerialization<TClient, TRequest, TResponse>(
            this INClientSerializationBuilder<TClient, TRequest, TResponse> serializationBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(serializationBuilder, nameof(serializationBuilder));

            return serializationBuilder.UsingCustomSerializer(new MessagePackSerializerProvider());
        }

        /// <summary>
        /// Sets MessagePack based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="serializationBuilder"></param>
        /// <param name="messagePackSerializerSettings">The settings to be used with <see cref="MessagePackSerializer"/>.</param>
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

        /// <summary>
        /// Sets MessagePack based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="serializationBuilder"></param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> UsingMessagePackSerialization<TRequest, TResponse>(
            this INClientFactorySerializationBuilder<TRequest, TResponse> serializationBuilder)
        {
            Ensure.IsNotNull(serializationBuilder, nameof(serializationBuilder));

            return serializationBuilder.UsingCustomSerializer(new MessagePackSerializerProvider());
        }

        /// <summary>
        /// Sets MessagePack based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="serializationBuilder"></param>
        /// <param name="messagePackSerializerSettings">The settings to be used with <see cref="MessagePackSerializer"/>.</param>
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
