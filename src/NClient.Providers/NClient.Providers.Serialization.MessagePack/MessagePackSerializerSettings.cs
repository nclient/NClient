using MessagePack;
using NClient.Common.Helpers;

namespace NClient.Providers.Serialization.MessagePack
{
    public class MessagePackSerializerSettings
    {
        public string ContentTypeHeader { get; } = string.Empty;
        public MessagePackSerializerOptions Options { get; }

        public MessagePackSerializerSettings(MimeType contentTypeHeader, MessagePackSerializerOptions? options = null)
        {
            Ensure.IsNotNull(contentTypeHeader, nameof(contentTypeHeader));

            ContentTypeHeader = contentTypeHeader.GetDescription();
            Options = options ?? MessagePackSerializerOptions.Standard;
        }
        
        public MessagePackSerializerSettings(string contentTypeHeader, MessagePackSerializerOptions? options = null)
        {
            Ensure.IsNotNullOrEmpty(contentTypeHeader, nameof(contentTypeHeader));
            
            ContentTypeHeader = contentTypeHeader;
            Options = options ?? MessagePackSerializerOptions.Standard;
        }
    }
}
