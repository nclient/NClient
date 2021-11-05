using NClient.Common.Helpers;
using NClient.Providers.Serialization.Json.Newtonsoft;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace NClient
{
    // TODO: doc
    public static class NewtonsoftJsonSerializationExtensions
    {
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithNewtonsoftJsonSerialization<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientAdvancedOptionalBuilder) 
            where TClient : class
        {
            Ensure.IsNotNull(clientAdvancedOptionalBuilder, nameof(clientAdvancedOptionalBuilder));
            
            return clientAdvancedOptionalBuilder.WithCustomSerialization(new NewtonsoftJsonSerializerProvider());
        }

        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithNewtonsoftJsonSerialization<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientAdvancedOptionalBuilder,
            JsonSerializerSettings jsonSerializerSettings)
            where TClient : class
        {
            Ensure.IsNotNull(clientAdvancedOptionalBuilder, nameof(clientAdvancedOptionalBuilder));
            Ensure.IsNotNull(jsonSerializerSettings, nameof(jsonSerializerSettings));
            
            return clientAdvancedOptionalBuilder.WithCustomSerialization(new NewtonsoftJsonSerializerProvider(jsonSerializerSettings));
        }

        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithNewtonsoftJsonSerialization<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> clientAdvancedOptionalBuilder)
        {
            Ensure.IsNotNull(clientAdvancedOptionalBuilder, nameof(clientAdvancedOptionalBuilder));
            
            return clientAdvancedOptionalBuilder.WithCustomSerialization(new NewtonsoftJsonSerializerProvider());
        }

        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithNewtonsoftJsonSerialization<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> clientAdvancedOptionalBuilder,
            JsonSerializerSettings jsonSerializerSettings)
        {
            Ensure.IsNotNull(clientAdvancedOptionalBuilder, nameof(clientAdvancedOptionalBuilder));
            Ensure.IsNotNull(jsonSerializerSettings, nameof(jsonSerializerSettings));
            
            return clientAdvancedOptionalBuilder.WithCustomSerialization(new NewtonsoftJsonSerializerProvider(jsonSerializerSettings));
        }
    }
}
