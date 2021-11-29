using NClient.Common.Helpers;

namespace NClient.Providers.Serialization.ProtobufNet
{
    public record ProtobufNetSerializerSettings
    {
        public string ContentTypeHeader { get; } = string.Empty;

        public ProtobufNetSerializerSettings(string contentTypeHeader)
        {
            Ensure.IsNotNullOrEmpty(contentTypeHeader, nameof(contentTypeHeader));

            ContentTypeHeader = contentTypeHeader;
        }
    }
}
