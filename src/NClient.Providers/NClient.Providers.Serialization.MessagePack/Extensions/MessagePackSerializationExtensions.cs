using NClient.Common.Helpers;
using NClient.Providers.Serialization.MessagePack;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class MessagePackSerializationExtensions
    {
        /// <summary>Sets the MessagePack serializer for the client.</summary>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithMessagePackSerialization<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder) 
            where TClient : class
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            
            return optionalBuilder.WithCustomSerialization(new MessagePackSerializerProvider());
        }

        /// <summary>Sets the MessagePack serializer for the client.</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="messagePackSerializerSettings">The settings to be used with the MessagePack serializer.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithMessagePackSerialization<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder,
            MessagePackSerializerSettings messagePackSerializerSettings)
            where TClient : class
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(messagePackSerializerSettings, nameof(messagePackSerializerSettings));
            
            return optionalBuilder.WithCustomSerialization(new MessagePackSerializerProvider(messagePackSerializerSettings));
        }
        /// <summary>Sets the MessagePack serializer for the client.</summary>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithMessagePackSerialization<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> optionalBuilder)
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            
            return optionalBuilder.WithCustomSerialization(new MessagePackSerializerProvider());
        }

        /// <summary>Sets the MessagePack serializer for the client.</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="messagePackSerializerSettings">The settings to be used with the MessagePack serializer.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithMessagePackSerialization<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> optionalBuilder,
            MessagePackSerializerSettings messagePackSerializerSettings)
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(messagePackSerializerSettings, nameof(messagePackSerializerSettings));
            
            return optionalBuilder.WithCustomSerialization(new MessagePackSerializerProvider(messagePackSerializerSettings));
        }
    }
}
