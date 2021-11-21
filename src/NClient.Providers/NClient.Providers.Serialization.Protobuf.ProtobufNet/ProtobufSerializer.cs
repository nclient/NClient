using System;
using System.IO;
using NClient.Common.Helpers;
using ProtoBuf;

namespace NClient.Providers.Serialization.Protobuf.ProtobufNet
{
    public class ProtobufSerializer : ISerializer
    {
        private readonly ProtobufSerializerSettings _protobufSerializerSettings;

        public string ContentType { get; } = String.Empty;

        public ProtobufSerializer(ProtobufSerializerSettings protobufSerializerSettings)
        {
            Ensure.IsNotNull(protobufSerializerSettings, nameof(protobufSerializerSettings));
            Ensure.IsNotNullOrEmpty(protobufSerializerSettings.ContentTypeHeader, nameof(protobufSerializerSettings.ContentTypeHeader));

            _protobufSerializerSettings = protobufSerializerSettings;
            ContentType = _protobufSerializerSettings.ContentTypeHeader;
        }

        public object? Deserialize(string source, Type returnType)
        {
            Ensure.IsNotNull(source, nameof(source));
            Ensure.IsNotNull(returnType, nameof(returnType));

            var bytes = Converters.GetBytes(source);
            
            using var memoryStream = new MemoryStream(bytes);
            return Serializer.Deserialize(returnType, memoryStream);
        }

        public string Serialize<T>(T? value)
        {
            Ensure.IsNotNull(value, nameof(value));
            
            using var memoryStream = new MemoryStream();
            Serializer.Serialize(memoryStream, value);
            
            return Converters.GetString(memoryStream.ToArray());
        }
    }
}
