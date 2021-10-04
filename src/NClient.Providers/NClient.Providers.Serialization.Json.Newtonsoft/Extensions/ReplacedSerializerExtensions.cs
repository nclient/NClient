using NClient.Abstractions.Builders;
using NClient.Common.Helpers;
using NClient.Providers.Serialization.Json.Newtonsoft;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Serialization.Newtonsoft
{
    // TODO: doc
    public static class ReplacedSerializerExtensions
    {
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithSerializerReplacedByNewtonsoftJson<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder) 
            where TClient : class
        {
            Ensure.IsNotNull(clientOptionalBuilder, nameof(clientOptionalBuilder));
            
            return clientOptionalBuilder.WithSerializerReplacedBy(new NewtonsoftJsonSerializerProvider());
        }
        
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithSerializerReplacedByNewtonsoftJson<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> factoryOptionalBuilder)
        {
            Ensure.IsNotNull(factoryOptionalBuilder, nameof(factoryOptionalBuilder));
            
            return factoryOptionalBuilder.WithSerializerReplacedBy(new NewtonsoftJsonSerializerProvider());
        }
        
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithSerializerReplacedByNewtonsoftJson<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientOptionalBuilder,
            JsonSerializerSettings jsonSerializerSettings)
            where TClient : class
        {
            Ensure.IsNotNull(clientOptionalBuilder, nameof(clientOptionalBuilder));
            Ensure.IsNotNull(jsonSerializerSettings, nameof(jsonSerializerSettings));
            
            return clientOptionalBuilder.WithSerializerReplacedBy(new NewtonsoftJsonSerializerProvider(jsonSerializerSettings));
        }
        
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithSerializerReplacedByNewtonsoftJson<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> factoryOptionalBuilder,
            JsonSerializerSettings jsonSerializerSettings)
        {
            Ensure.IsNotNull(factoryOptionalBuilder, nameof(factoryOptionalBuilder));
            Ensure.IsNotNull(jsonSerializerSettings, nameof(jsonSerializerSettings));
            
            return factoryOptionalBuilder.WithSerializerReplacedBy(new NewtonsoftJsonSerializerProvider(jsonSerializerSettings));
        }
    }
}
