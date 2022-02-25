using System.Text.Json;
using NClient.Common.Helpers;
using NClient.Providers.Serialization.SystemTextJson;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class SystemTextJsonSerializationExtensions
    {
        /// <summary>Sets the System.Text.Json serializer for the client.</summary>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithSystemTextJsonSerialization<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));

            return optionalBuilder.WithCustomSerialization(new SystemTextJsonSerializerProvider());
        }

        /// <summary>Sets the System.Text.Json serializer for the client.</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="jsonSerializerOptions">The settings to be used with the Newtonsoft.Json serializer.</param>
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> WithSystemTextJsonSerialization<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> optionalBuilder,
            JsonSerializerOptions jsonSerializerOptions)
            where TClient : class
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(jsonSerializerOptions, nameof(jsonSerializerOptions));

            return optionalBuilder.WithCustomSerialization(new SystemTextJsonSerializerProvider(jsonSerializerOptions));
        }

        /// <summary>Sets the System.Text.Json serializer for the client.</summary>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithSystemTextJsonSerialization<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> optionalBuilder)
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));

            return optionalBuilder.WithCustomSerialization(new SystemTextJsonSerializerProvider());
        }

        /// <summary>Sets the System.Text.Json serializer for the client.</summary>
        /// <param name="optionalBuilder"></param>
        /// <param name="jsonSerializerOptions">The settings to be used with the Newtonsoft.Json serializer.</param>
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> WithSystemTextJsonSerialization<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> optionalBuilder,
            JsonSerializerOptions jsonSerializerOptions)
        {
            Ensure.IsNotNull(optionalBuilder, nameof(optionalBuilder));
            Ensure.IsNotNull(jsonSerializerOptions, nameof(jsonSerializerOptions));

            return optionalBuilder.WithCustomSerialization(new SystemTextJsonSerializerProvider(jsonSerializerOptions));
        }
    }
}
