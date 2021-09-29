using NClient.Abstractions.Customization;
using NClient.Abstractions.Serialization;
using NClient.Common.Helpers;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Serialization.Newtonsoft
{
    public static class CommonCustomizerExtensions
    {
        /// <summary>
        /// Sets Newtonsoft.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="commonCustomizer"></param>
        public static TCustomizer UsingNewtonsoftSerializer<TCustomizer, TInterface, TRequest, TResponse>(
            this INClientCommonCustomizer<TCustomizer, TInterface, TRequest, TResponse> commonCustomizer)
            where TCustomizer : INClientCommonCustomizer<TCustomizer, TInterface, TRequest, TResponse>
            where TInterface : class
        {
            Ensure.IsNotNull(commonCustomizer, nameof(commonCustomizer));

            return commonCustomizer.UsingCustomSerializer(new NewtonsoftSerializerProvider());
        }

        /// <summary>
        /// Sets Newtonsoft.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="commonCustomizer"></param>
        /// <param name="jsonSerializerSettings">The settings to be used with <see cref="JsonSerializer"/>.</param>
        public static TCustomizer UsingNewtonsoftSerializer<TCustomizer, TInterface, TRequest, TResponse>(
            this INClientCommonCustomizer<TCustomizer, TInterface, TRequest, TResponse> commonCustomizer,
            JsonSerializerSettings jsonSerializerSettings)
            where TCustomizer : INClientCommonCustomizer<TCustomizer, TInterface, TRequest, TResponse>
            where TInterface : class
        {
            Ensure.IsNotNull(commonCustomizer, nameof(commonCustomizer));
            Ensure.IsNotNull(jsonSerializerSettings, nameof(jsonSerializerSettings));

            return commonCustomizer.UsingCustomSerializer(new NewtonsoftSerializerProvider(jsonSerializerSettings));
        }
    }
}
