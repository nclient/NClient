using NClient.Common.Helpers;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class JsonSerializerExtensions
    {
        /// <summary>Sets default JSON serializer based on System.Text.Json. This is an alias for the <see cref="UsingSystemTextJsonSerialization"/> method.</summary>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> UsingJsonSerializer<TClient, TRequest, TResponse>(
            this INClientSerializationBuilder<TClient, TRequest, TResponse> serializationBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(serializationBuilder, nameof(serializationBuilder));

            return serializationBuilder.UsingSystemTextJsonSerialization();
        }

        /// <summary>Sets default JSON serializer based on System.Text.Json. This is an alias for the <see cref="UsingSystemTextJsonSerialization"/> method.</summary>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> UsingJsonSerializer<TRequest, TResponse>(
            this INClientFactorySerializationBuilder<TRequest, TResponse> serializationBuilder)
        {
            Ensure.IsNotNull(serializationBuilder, nameof(serializationBuilder));

            return serializationBuilder.UsingSystemTextJsonSerialization();
        }
    }
}
