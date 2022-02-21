using NClient.Common.Helpers;
using NClient.Providers.Serialization;
using NClient.Providers.Serialization.MessagePack;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class MessagePackSerializationExtensions
    {
        /// <summary>Sets MessagePack based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.</summary>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithMessagePackSerialization<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder) 
            where TClient : class
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            
            return optionalBuilder.WithCustomSerialization(new MessagePackSerializerProvider());
        }

        /// <summary>Sets MessagePack based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="messagePackSerializerSettings">The settings to be used with <see cref="MessagePackSerializer"/>.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithMessagePackSerialization<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder,
            MessagePackSerializerSettings messagePackSerializerSettings)
            where TClient : class
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(messagePackSerializerSettings, nameof(messagePackSerializerSettings));
            
            return optionalBuilder.WithCustomSerialization(new MessagePackSerializerProvider(messagePackSerializerSettings));
        }

        /// <summary>Sets MessagePack based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.</summary>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithMessagePackSerialization<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> optionalBuilder)
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            
            return optionalBuilder.WithCustomSerialization(new MessagePackSerializerProvider());
        }

        /// <summary>Sets MessagePack based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="messagePackSerializerSettings">The settings to be used with <see cref="MessagePackSerializer"/>.</param>
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
