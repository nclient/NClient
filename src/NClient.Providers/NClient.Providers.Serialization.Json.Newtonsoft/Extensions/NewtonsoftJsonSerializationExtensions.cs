using NClient.Common.Helpers;
using NClient.Providers.Serialization.Json.Newtonsoft;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace NClient
{
    // TODO: doc
    public static class NewtonsoftJsonSerializationExtensions
    {
        public static INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithNewtonsoftJsonSerialization<TClient, TRequest, TResponse>(
            this INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> clientAdvancedOptionalBuilder) 
            where TClient : class
        {
            Ensure.IsNotNull(clientAdvancedOptionalBuilder, nameof(clientAdvancedOptionalBuilder));
            
            return clientAdvancedOptionalBuilder.WithCustomSerialization(new NewtonsoftJsonSerializerProvider());
        }
        
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithNewtonsoftJsonSerialization<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder) 
            where TClient : class
        {
            Ensure.IsNotNull(clientOptionalBuilder, nameof(clientOptionalBuilder));
            
            return WithNewtonsoftJsonSerialization(clientOptionalBuilder.AsAdvanced()).AsBasic();
        }

        public static INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> WithNewtonsoftJsonSerialization<TClient, TRequest, TResponse>(
            this INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> clientAdvancedOptionalBuilder,
            JsonSerializerSettings jsonSerializerSettings)
            where TClient : class
        {
            Ensure.IsNotNull(clientAdvancedOptionalBuilder, nameof(clientAdvancedOptionalBuilder));
            Ensure.IsNotNull(jsonSerializerSettings, nameof(jsonSerializerSettings));
            
            return clientAdvancedOptionalBuilder.WithCustomSerialization(new NewtonsoftJsonSerializerProvider(jsonSerializerSettings));
        }
        
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithNewtonsoftJsonSerialization<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder,
            JsonSerializerSettings jsonSerializerSettings)
            where TClient : class
        {
            Ensure.IsNotNull(clientOptionalBuilder, nameof(clientOptionalBuilder));
            Ensure.IsNotNull(jsonSerializerSettings, nameof(jsonSerializerSettings));
            
            return WithNewtonsoftJsonSerialization(clientOptionalBuilder.AsAdvanced(), jsonSerializerSettings).AsBasic();
        }
        
        public static INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> WithNewtonsoftJsonSerialization<TRequest, TResponse>(
            this INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> clientAdvancedOptionalBuilder)
        {
            Ensure.IsNotNull(clientAdvancedOptionalBuilder, nameof(clientAdvancedOptionalBuilder));
            
            return clientAdvancedOptionalBuilder.WithCustomSerialization(new NewtonsoftJsonSerializerProvider());
        }
        
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithNewtonsoftJsonSerialization<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> clientOptionalBuilder)
        {
            Ensure.IsNotNull(clientOptionalBuilder, nameof(clientOptionalBuilder));
            
            return WithNewtonsoftJsonSerialization(clientOptionalBuilder.AsAdvanced()).AsBasic();
        }

        public static INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> WithNewtonsoftJsonSerialization<TRequest, TResponse>(
            this INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> clientAdvancedOptionalBuilder,
            JsonSerializerSettings jsonSerializerSettings)
        {
            Ensure.IsNotNull(clientAdvancedOptionalBuilder, nameof(clientAdvancedOptionalBuilder));
            Ensure.IsNotNull(jsonSerializerSettings, nameof(jsonSerializerSettings));
            
            return clientAdvancedOptionalBuilder.WithCustomSerialization(new NewtonsoftJsonSerializerProvider(jsonSerializerSettings));
        }
        
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithNewtonsoftJsonSerialization<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> clientOptionalBuilder,
            JsonSerializerSettings jsonSerializerSettings)
        {
            Ensure.IsNotNull(clientOptionalBuilder, nameof(clientOptionalBuilder));
            Ensure.IsNotNull(jsonSerializerSettings, nameof(jsonSerializerSettings));
            
            return WithNewtonsoftJsonSerialization(clientOptionalBuilder.AsAdvanced(), jsonSerializerSettings).AsBasic();
        }
    }
}
