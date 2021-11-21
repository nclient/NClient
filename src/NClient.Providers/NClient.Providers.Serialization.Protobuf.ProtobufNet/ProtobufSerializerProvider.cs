using Microsoft.Extensions.Logging;
using NClient.Common.Helpers;

namespace NClient.Providers.Serialization.Protobuf.ProtobufNet
{
    public class ProtobufSerializerProvider : ISerializerProvider
    {
        private readonly ProtobufSerializerSettings _protobufSerializerSettings;

        /// <summary>
        /// Creates the Protobuf serializer provider.
        /// </summary>
        public ProtobufSerializerProvider()
        {
            _protobufSerializerSettings = new ProtobufSerializerSettings("application/proto");
        }

        /// <summary>
        /// Creates the Newtonsoft.Json based serializer provider.
        /// </summary>
        /// <param name="protobufSerializerSettings">The settings to be used with <see cref="ProtobufSerializer"/>.</param>
        public ProtobufSerializerProvider(ProtobufSerializerSettings protobufSerializerSettings)
        {
            Ensure.IsNotNull(protobufSerializerSettings, nameof(protobufSerializerSettings));

            _protobufSerializerSettings = protobufSerializerSettings;
        }

        public ISerializer Create(ILogger? logger)
        {
            return new ProtobufSerializer(_protobufSerializerSettings);
        }
    }
}
