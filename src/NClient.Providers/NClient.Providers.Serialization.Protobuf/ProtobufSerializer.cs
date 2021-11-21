using System;
using System.IO;
using NClient.Common.Helpers;
using ProtoBuf;

namespace NClient.Providers.Serialization.Protobuf
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

            byte[] bytes = new byte[source.Length];
            Buffer.BlockCopy(source.ToCharArray(), 0, bytes, 0, bytes.Length);
            
            using var memoryStream = new MemoryStream(bytes);
            return Serializer.Deserialize(returnType, memoryStream);
        }

        public string Serialize<T>(T? value)
        {
            Ensure.IsNotNull(value, nameof(value));
            
            using var memoryStream = new MemoryStream();
            Serializer.Serialize(memoryStream, value);
            var serializedBytes = memoryStream.ToArray();
            
            var chars = new char[serializedBytes.Length];
            Buffer.BlockCopy(serializedBytes, 0, chars, 0, serializedBytes.Length);
            return new string(chars);
        }
    }
}
