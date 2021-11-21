using NClient.Common.Helpers;

namespace NClient.Providers.Serialization.Protobuf.ProtobufNet.Extensions
{
    public static class ProtobufSerializationExtensions
    {
        /// <summary>
        /// Sets ProtobufNet based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="optionalBuilder"></param>
        /// <typeparam name="TClient"></typeparam>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <returns></returns>
        public static INClientOptionalBuilder<TClient, TClient, TResponse> WithProtobufNetSerialization<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TClient, TResponse> optionalBuilder) 
            where TClient : class
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            
            return optionalBuilder.WithCustomSerialization(new ProtobufSerializerProvider());
        }

        /// <summary>
        /// Sets ProtobufNet based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="protobufSerializerSettings">The settings to be used with <see cref="ProtobufSerializer"/>.</param>
        /// <typeparam name="TClient"></typeparam>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <returns></returns>
        public static INClientOptionalBuilder<TClient, TClient, TResponse> WithProtobufNetSerialization<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TClient, TResponse> optionalBuilder,
            ProtobufSerializerSettings protobufSerializerSettings)
            where TClient : class
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(protobufSerializerSettings, nameof(protobufSerializerSettings));
            
            return optionalBuilder.WithCustomSerialization(new ProtobufSerializerProvider(protobufSerializerSettings));
        }

        /// <summary>
        /// Sets ProtobufNet based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="optionalBuilder"></param>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <returns></returns>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithProtobufNetSerialization<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> optionalBuilder)
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            
            return optionalBuilder.WithCustomSerialization(new ProtobufSerializerProvider());
        }

        /// <summary>
        /// Sets ProtobufNet based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="protobufSerializerSettings">The settings to be used with <see cref="ProtobufSerializer"/>.</param>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <returns></returns>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithProtobufNetSerialization<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> optionalBuilder,
            ProtobufSerializerSettings protobufSerializerSettings)
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(protobufSerializerSettings, nameof(protobufSerializerSettings));
            
            return optionalBuilder.WithCustomSerialization(new ProtobufSerializerProvider(protobufSerializerSettings));
        }
    }
}
