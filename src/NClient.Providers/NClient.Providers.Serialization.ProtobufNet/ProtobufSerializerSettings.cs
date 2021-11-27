using NClient.Common.Helpers;

namespace NClient.Providers.Serialization.ProtobufNet
{
    public record ProtobufSerializerSettings
    {
        public string ContentTypeHeader { get; } = string.Empty;

        public ProtobufSerializerSettings(string contentTypeHeader)
        {
            Ensure.IsNotNullOrEmpty(contentTypeHeader, nameof(contentTypeHeader));

            ContentTypeHeader = contentTypeHeader;
        }
    }
}
