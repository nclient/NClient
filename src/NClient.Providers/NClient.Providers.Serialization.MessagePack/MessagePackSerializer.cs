using System;
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

        //SUGGESTION: Use byte array instead of string 
        public object? Deserialize(string source, Type returnType)
        {
            Ensure.IsNotNull(source, nameof(source));
            Ensure.IsNotNull(returnType, nameof(returnType));

            return MP.MessagePackSerializer.Deserialize(returnType, Converters.GetBytes(source), _messagePackSerializerSettings.Options);
        }

        public string Serialize<T>(T? value)
        {
            Ensure.IsNotNull(value, nameof(value));
            
            return Converters.GetString(MP.MessagePackSerializer.Serialize(value, _messagePackSerializerSettings.Options));
        }
    }
}
