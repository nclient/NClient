using Microsoft.Extensions.Logging;
using NClient.Common.Helpers;

namespace NClient.Providers.Serialization.ProtobufNet
{
    public class ProtobufNetSerializerProvider : ISerializerProvider
    {
        private readonly ProtobufNetSerializerSettings _protobufNetSerializerSettings;

        /// <summary>
        /// Creates the Protobuf serializer provider.
        /// </summary>
        public ProtobufNetSerializerProvider()
        {
            _protobufNetSerializerSettings = new ProtobufNetSerializerSettings("application/proto");
        }

        /// <summary>
        /// Creates the Newtonsoft.Json based serializer provider.
        /// </summary>
        /// <param name="protobufNetSerializerSettings">The settings to be used with <see cref="ProtobufNetSerializer"/>.</param>
        public ProtobufNetSerializerProvider(ProtobufNetSerializerSettings protobufNetSerializerSettings)
        {
            Ensure.IsNotNull(protobufNetSerializerSettings, nameof(protobufNetSerializerSettings));

            _protobufNetSerializerSettings = protobufNetSerializerSettings;
        }

        public ISerializer Create(ILogger? logger)
        {
            return new ProtobufNetSerializer(_protobufNetSerializerSettings);
        }
    }
}
