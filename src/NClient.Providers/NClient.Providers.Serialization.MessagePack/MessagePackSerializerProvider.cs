using MessagePack;
using NClient.Common.Helpers;
using Microsoft.Extensions.Logging;

namespace NClient.Providers.Serialization.MessagePack
{
    public class MessagePackSerializerProvider : ISerializerProvider
    {
        private readonly MessagePackSerializerSettings _messagePackSerializerSettings;

        /// <summary>
        /// Creates the MessagePack based serializer provider.
        /// </summary>
        public MessagePackSerializerProvider()
        {
            _messagePackSerializerSettings = new MessagePackSerializerSettings(MimeType.ProperType, MessagePackSerializerOptions.Standard);
        }

        /// <summary>
        /// Creates the MessagePack based serializer provider.
        /// </summary>
        /// <param name="messagePackSerializerSettings">The settings to be used with <see cref="MessagePackSerializer"/>.</param>
        public MessagePackSerializerProvider(MessagePackSerializerSettings messagePackSerializerSettings)
        {
            Ensure.IsNotNull(messagePackSerializerSettings, nameof(messagePackSerializerSettings));

            _messagePackSerializerSettings = messagePackSerializerSettings;
        }

        public ISerializer Create(ILogger? logger)
        {
            return new MessagePackSerializer(_messagePackSerializerSettings);
        }
    }
}
