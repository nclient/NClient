using NClient.Common.Helpers;

namespace NClient.Providers.Serialization.ProtobufNet
{
    /// <summary>The container of settings for ProtobufNet serializer.</summary>
    public record ProtobufNetSerializerSettings
    {
        /// <summary>Gets supported content type.</summary>
        public string ContentTypeHeader { get; } = string.Empty;

        /// <summary>Initializes the container of settings for ProtobufNet serializer.</summary>
        /// <param name="contentTypeHeader">The name of supported content type.</param>
        public ProtobufNetSerializerSettings(string contentTypeHeader)
        {
            Ensure.IsNotNullOrEmpty(contentTypeHeader, nameof(contentTypeHeader));

            ContentTypeHeader = contentTypeHeader;
        }
    }
}
