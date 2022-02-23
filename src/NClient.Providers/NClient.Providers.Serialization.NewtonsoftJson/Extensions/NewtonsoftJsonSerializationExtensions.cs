using NClient.Common.Helpers;
using NClient.Providers.Serialization.NewtonsoftJson;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class NewtonsoftJsonSerializationExtensions
    {
        /// <summary>Sets the Newtonsoft.Json serializer for the client.</summary>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithNewtonsoftJsonSerialization<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder) 
            where TClient : class
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            
            return optionalBuilder.WithCustomSerialization(new NewtonsoftJsonSerializerProvider());
        }

        /// <summary>Sets the Newtonsoft.Json serializer for the client.</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="jsonSerializerSettings">The settings to be used with the Newtonsoft.Json serializer.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithNewtonsoftJsonSerialization<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder,
            JsonSerializerSettings jsonSerializerSettings)
            where TClient : class
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(jsonSerializerSettings, nameof(jsonSerializerSettings));
            
            return optionalBuilder.WithCustomSerialization(new NewtonsoftJsonSerializerProvider(jsonSerializerSettings));
        }
        
        /// <summary>Sets the Newtonsoft.Json serializer for the client.</summary>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithNewtonsoftJsonSerialization<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> optionalBuilder)
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            
            return optionalBuilder.WithCustomSerialization(new NewtonsoftJsonSerializerProvider());
        }

        /// <summary>Sets the Newtonsoft.Json serializer for the client.</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="jsonSerializerSettings">The settings to be used with the Newtonsoft.Json serializer.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithNewtonsoftJsonSerialization<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> optionalBuilder,
            JsonSerializerSettings jsonSerializerSettings)
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(jsonSerializerSettings, nameof(jsonSerializerSettings));
            
            return optionalBuilder.WithCustomSerialization(new NewtonsoftJsonSerializerProvider(jsonSerializerSettings));
        }
    }
}
