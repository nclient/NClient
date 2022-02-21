using NClient.Common.Helpers;
using NClient.Providers.Serialization.ProtobufNet;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class ProtobufNetSerializationExtensions
    {
        /// <summary>Sets the ProtobufNet serializer for the client.</summary>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithProtobufNetSerialization<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder) 
            where TClient : class
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            
            return optionalBuilder.WithCustomSerialization(new ProtobufNetSerializerProvider());
        }

        /// <summary>Sets the ProtobufNet serializer for the client.</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="protobufNetSerializerSettings">The settings to be used with the ProtobufNet serializer.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithProtobufNetSerialization<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder,
            ProtobufNetSerializerSettings protobufNetSerializerSettings)
            where TClient : class
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(protobufNetSerializerSettings, nameof(protobufNetSerializerSettings));
            
            return optionalBuilder.WithCustomSerialization(new ProtobufNetSerializerProvider(protobufNetSerializerSettings));
        }

        /// <summary>Sets the ProtobufNet serializer for the client.</summary>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithProtobufNetSerialization<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> optionalBuilder)
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            
            return optionalBuilder.WithCustomSerialization(new ProtobufNetSerializerProvider());
        }

        /// <summary>Sets the ProtobufNet serializer for the client.</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="protobufNetSerializerSettings">The settings to be used with the ProtobufNet serializer.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithProtobufNetSerialization<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> optionalBuilder,
            ProtobufNetSerializerSettings protobufNetSerializerSettings)
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(protobufNetSerializerSettings, nameof(protobufNetSerializerSettings));
            
            return optionalBuilder.WithCustomSerialization(new ProtobufNetSerializerProvider(protobufNetSerializerSettings));
        }
    }
}
