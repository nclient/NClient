using System.Text.Json;
using NClient.Abstractions.Customization;
using NClient.Abstractions.Serialization;
using NClient.Common.Helpers;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Serialization.System
{
    public static class CommonCustomizerExtensions
    {
        /// <summary>
        /// Sets System.Text.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="commonCustomizer"></param>
        public static TCustomizer UsingSystemSerializer<TCustomizer, TInterface, TRequest, TResponse>(
            this INClientCommonCustomizer<TCustomizer, TInterface, TRequest, TResponse> commonCustomizer)
            where TCustomizer : INClientCommonCustomizer<TCustomizer, TInterface, TRequest, TResponse>
            where TInterface : class
        {
            Ensure.IsNotNull(commonCustomizer, nameof(commonCustomizer));

            return commonCustomizer.UsingCustomSerializer(new SystemSerializerProvider());
        }

        /// <summary>
        /// Sets System.Text.Json based <see cref="ISerializerProvider"/> used to create instance of <see cref="ISerializer"/>.
        /// </summary>
        /// <param name="commonCustomizer"></param>
        /// <param name="jsonSerializerOptions">The options to be used with <see cref="JsonSerializer"/>.</param>
        public static TCustomizer UsingSystemSerializer<TCustomizer, TInterface, TRequest, TResponse>(
            this INClientCommonCustomizer<TCustomizer, TInterface, TRequest, TResponse> commonCustomizer,
            JsonSerializerOptions jsonSerializerOptions)
            where TCustomizer : INClientCommonCustomizer<TCustomizer, TInterface, TRequest, TResponse>
            where TInterface : class
        {
            Ensure.IsNotNull(commonCustomizer, nameof(commonCustomizer));
            Ensure.IsNotNull(jsonSerializerOptions, nameof(jsonSerializerOptions));

            return commonCustomizer.UsingCustomSerializer(new SystemSerializerProvider(jsonSerializerOptions));
        }
    }
}
