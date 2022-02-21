using MessagePack;
using NClient.Common.Helpers;
using Microsoft.Extensions.Logging;

namespace NClient.Providers.Serialization.MessagePack
{
    /// <summary>The MessagePack based provider for a component that can create <see cref="ISerializer"/> instances.</summary>
    public class MessagePackSerializerProvider : ISerializerProvider
    {
        private readonly MessagePackSerializerSettings _messagePackSerializerSettings;

        /// <summary>Initializes the MessagePack based serializer provider.</summary>
        public MessagePackSerializerProvider()
        {
            _messagePackSerializerSettings = new MessagePackSerializerSettings(MimeType.ProperType, MessagePackSerializerOptions.Standard);
        }

        /// <summary>Initializes the MessagePack based serializer provider.</summary>
        /// <param name="messagePackSerializerSettings">The settings to be used with <see cref="MessagePackSerializer"/>.</param>
        public MessagePackSerializerProvider(MessagePackSerializerSettings messagePackSerializerSettings)
        {
            Ensure.IsNotNull(messagePackSerializerSettings, nameof(messagePackSerializerSettings));

            _messagePackSerializerSettings = messagePackSerializerSettings;
        }

        /// <summary>Creates MessagePack <see cref="ISerializer"/> instance.</summary>
        /// <param name="logger">Optional logger. If it is not passed, then logs will not be written.</param>
        public ISerializer Create(ILogger? logger)
        {
            return new MessagePackSerializer(_messagePackSerializerSettings);
        }
    }
}
