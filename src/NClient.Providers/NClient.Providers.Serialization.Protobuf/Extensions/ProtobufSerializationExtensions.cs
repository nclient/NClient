using NClient.Common.Helpers;

namespace NClient.Providers.Serialization.Protobuf.Extensions
{
    public static class ProtobufSerializationExtensions
    {
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithNewtonsoftJsonSerialization<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder) 
            where TClient : class
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            
            return optionalBuilder.WithCustomSerialization(new ProtobufSerializerProvider());
        }

        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithNewtonsoftJsonSerialization<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder,
            ProtobufSerializerSettings protobufSerializerSettings)
            where TClient : class
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(protobufSerializerSettings, nameof(protobufSerializerSettings));
            
            return optionalBuilder.WithCustomSerialization(new ProtobufSerializerProvider(protobufSerializerSettings));
        }

        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithNewtonsoftJsonSerialization<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> optionalBuilder)
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            
            return optionalBuilder.WithCustomSerialization(new ProtobufSerializerProvider());
        }

        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithNewtonsoftJsonSerialization<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> optionalBuilder,
            ProtobufSerializerSettings protobufSerializerSettings)
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(protobufSerializerSettings, nameof(protobufSerializerSettings));
            
            return optionalBuilder.WithCustomSerialization(new ProtobufSerializerProvider(protobufSerializerSettings));
        }
    }
}
