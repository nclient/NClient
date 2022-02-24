using System;
using System.IO;
using NClient.Common.Helpers;
using ProtoBuf;

namespace NClient.Providers.Serialization.ProtobufNet
{
    internal class ProtobufNetSerializer : ISerializer
    {
        public string ContentType { get; }

        public ProtobufNetSerializer(ProtobufNetSerializerSettings protobufNetSerializerSettings)
        {
            Ensure.IsNotNull(protobufNetSerializerSettings, nameof(protobufNetSerializerSettings));
            Ensure.IsNotNullOrEmpty(protobufNetSerializerSettings.ContentTypeHeader, nameof(protobufNetSerializerSettings.ContentTypeHeader));

            ContentType = protobufNetSerializerSettings.ContentTypeHeader;
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
