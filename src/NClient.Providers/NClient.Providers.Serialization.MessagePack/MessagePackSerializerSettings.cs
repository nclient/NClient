using MessagePack;
using NClient.Common.Helpers;

namespace NClient.Providers.Serialization.MessagePack
{
    /// <summary>The container of settings for MessagePack serializer.</summary>
    public class MessagePackSerializerSettings
    {
        /// <summary>Gets supported content type.</summary>
        public string ContentTypeHeader { get; } = string.Empty;
        
        /// <summary>An native description of options for running the <see cref="MessagePackSerializer"/>.</summary>
        public MessagePackSerializerOptions Options { get; }

        /// <summary>Initializes the container of options for MessagePack serializer.</summary>
        /// <param name="contentTypeHeader">The name of supported content type.</param>
        /// <param name="options">The description of options for running the <see cref="MessagePackSerializer"/>.</param>
        public MessagePackSerializerSettings(MimeType contentTypeHeader, MessagePackSerializerOptions? options = null)
        {
            Ensure.IsNotNull(contentTypeHeader, nameof(contentTypeHeader));

            ContentTypeHeader = contentTypeHeader.GetDescription();
            Options = options ?? MessagePackSerializerOptions.Standard;
        }
        
        /// <summary>Initializes the container of options for MessagePack serializer.</summary>
        /// <param name="contentTypeHeader">The name of supported content type.</param>
        /// <param name="options">The description of options for running the <see cref="MessagePackSerializer"/>.</param>
        public MessagePackSerializerSettings(string contentTypeHeader, MessagePackSerializerOptions? options = null)
        {
            Ensure.IsNotNullOrEmpty(contentTypeHeader, nameof(contentTypeHeader));
            
            ContentTypeHeader = contentTypeHeader;
            Options = options ?? MessagePackSerializerOptions.Standard;
        }
    }
}
