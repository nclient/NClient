using Microsoft.Extensions.Logging;
using NClient.Common.Helpers;

namespace NClient.Providers.Serialization.ProtobufNet
{
    /// <summary>The ProtobufNet based provider for a component that can create <see cref="ISerializer"/> instances.</summary>
    public class ProtobufNetSerializerProvider : ISerializerProvider
    {
        private readonly ProtobufNetSerializerSettings _protobufNetSerializerSettings;

        /// <summary>Initializes the ProtobufNet serializer provider.</summary>
        public ProtobufNetSerializerProvider()
        {
            _protobufNetSerializerSettings = new ProtobufNetSerializerSettings("application/proto");
        }

        /// <summary>Initializes the ProtobufNet based serializer provider.</summary>
        /// <param name="protobufNetSerializerSettings">The settings to be used with <see cref="ProtobufNetSerializer"/>.</param>
        public ProtobufNetSerializerProvider(ProtobufNetSerializerSettings protobufNetSerializerSettings)
        {
            Ensure.IsNotNull(protobufNetSerializerSettings, nameof(protobufNetSerializerSettings));

            _protobufNetSerializerSettings = protobufNetSerializerSettings;
        }

        /// <summary>Creates ProtobufNet <see cref="ISerializer"/> instance.</summary>
        /// <param name="logger">Optional logger. If it is not passed, then logs will not be written.</param>
        public ISerializer Create(ILogger logger)
        {
            return new ProtobufNetSerializer(_protobufNetSerializerSettings);
        }
    }
}
