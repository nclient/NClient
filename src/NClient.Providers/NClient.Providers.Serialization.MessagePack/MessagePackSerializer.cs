using System;
using System.Text;
using NClient.Common.Helpers;
using MP = MessagePack;

namespace NClient.Providers.Serialization.MessagePack
{
    internal class MessagePackSerializer : ISerializer
    {
        private readonly MessagePackSerializerSettings _messagePackSerializerSettings;

        //INFO: https://github.com/msgpack/msgpack/issues/194
        public string ContentType { get; } = "application/msgpack";

        public MessagePackSerializer(MessagePackSerializerSettings messagePackSerializerSettings)
        {
            Ensure.IsNotNull(messagePackSerializerSettings, nameof(messagePackSerializerSettings));
            Ensure.IsNotNullOrEmpty(messagePackSerializerSettings.ContentTypeHeader, nameof(messagePackSerializerSettings.ContentTypeHeader));
            
            _messagePackSerializerSettings = messagePackSerializerSettings;
            ContentType = messagePackSerializerSettings.ContentTypeHeader;
        }

        public object? Deserialize(string source, Type returnType)
        {
            Ensure.IsNotNull(source, nameof(source));
            Ensure.IsNotNull(returnType, nameof(returnType));

            return MP.MessagePackSerializer.Typeless.Deserialize(Encoding.UTF8.GetBytes(source), _messagePackSerializerSettings.Options);
        }

        public string Serialize<T>(T? value)
        {
            return Encoding.UTF8.GetString(MP.MessagePackSerializer.Serialize(value, _messagePackSerializerSettings.Options));
        }
    }
}
