using NClient.Common.Helpers;
using NClient.Providers.Serialization;
using NClient.Providers.Serialization.ProtobufNet;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class ProtobufNetSerializationExtensions
    {
        /// <summary>
        /// Sets ProtobufNet based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="optionalBuilder"></param>
        /// <typeparam name="TClient"></typeparam>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <returns></returns>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithProtobufNetSerialization<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder) 
            where TClient : class
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            
            return optionalBuilder.WithCustomSerialization(new ProtobufNetSerializerProvider());
        }

        /// <summary>
        /// Sets ProtobufNet based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="protobufNetSerializerSettings">The settings to be used with <see cref="ProtobufNetSerializer"/>.</param>
        /// <typeparam name="TClient"></typeparam>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <returns></returns>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithProtobufNetSerialization<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder,
            ProtobufNetSerializerSettings protobufNetSerializerSettings)
            where TClient : class
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(protobufNetSerializerSettings, nameof(protobufNetSerializerSettings));
            
            return optionalBuilder.WithCustomSerialization(new ProtobufNetSerializerProvider(protobufNetSerializerSettings));
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
            
            return optionalBuilder.WithCustomSerialization(new ProtobufNetSerializerProvider());
        }

        /// <summary>
        /// Sets ProtobufNet based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="protobufNetSerializerSettings">The settings to be used with <see cref="ProtobufNetSerializer"/>.</param>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <returns></returns>
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
