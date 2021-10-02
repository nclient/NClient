using System.Net.Http;
using NClient.Abstractions.Customization;
using NClient.Common.Helpers;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.HttpClient.System
{
    public static class CommonCustomizerExtensions
    {
        /// <summary>
        /// Sets System.Net.Http based <see cref="IHttpClientProvider"/> used to create instance of <see cref="IHttpClient"/>.
        /// </summary>
        /// <param name="commonCustomizer"></param>
        public static TCustomizer UsingSystemHttpClient<TCustomizer, TClient>(
            this INClientCommonCustomizer<TCustomizer, TClient, HttpRequestMessage, HttpResponseMessage> commonCustomizer)
            where TCustomizer : INClientCommonCustomizer<TCustomizer, TClient, HttpRequestMessage, HttpResponseMessage>
            where TClient : class
        {
            Ensure.IsNotNull(commonCustomizer, nameof(commonCustomizer));

            return commonCustomizer.UsingCustomHttpClient(
                new SystemHttpClientProvider(), 
                new SystemHttpMessageBuilderProvider(),
                new SystemHttpClientExceptionFactory());
        }
        
        /// <summary>
        /// Sets System.Net.Http based <see cref="IHttpClientProvider"/> used to create instance of <see cref="IHttpClient"/>.
        /// </summary>
        /// <param name="commonCustomizer"></param>
        /// <param name="httpClientFactory">The factory abstraction used to create instance of <see cref="System.Net.Http.HttpClient"/> instances.</param>
        /// <param name="httpClientName">The logical name of <see cref="System.Net.Http.HttpClient"/> to create.</param>
        public static TCustomizer UsingSystemHttpClient<TCustomizer, TClient>(
            this INClientCommonCustomizer<TCustomizer, TClient, HttpRequestMessage, HttpResponseMessage> commonCustomizer,
            IHttpClientFactory httpClientFactory, string? httpClientName = null)
            where TCustomizer : INClientCommonCustomizer<TCustomizer, TClient, HttpRequestMessage, HttpResponseMessage>
            where TClient : class
        {
            Ensure.IsNotNull(commonCustomizer, nameof(commonCustomizer));
            Ensure.IsNotNull(httpClientFactory, nameof(httpClientFactory));

            return commonCustomizer.UsingCustomHttpClient(
                new SystemHttpClientProvider(httpClientFactory, httpClientName), 
                new SystemHttpMessageBuilderProvider(),
                new SystemHttpClientExceptionFactory());
        }

        /// <summary>
        /// Sets System.Net.Http based <see cref="IHttpClientProvider"/> used to create instance of <see cref="IHttpClient"/>.
        /// </summary>
        /// <param name="commonCustomizer"></param>
        /// <param name="httpMessageHandler">The HTTP message handler.</param>
        public static TCustomizer UsingCustomHttpClient<TCustomizer, TClient>(
            this INClientCommonCustomizer<TCustomizer, TClient, HttpRequestMessage, HttpResponseMessage> commonCustomizer,
            HttpMessageHandler httpMessageHandler)
            where TCustomizer : INClientCommonCustomizer<TCustomizer, TClient, HttpRequestMessage, HttpResponseMessage>
            where TClient : class
        {
            Ensure.IsNotNull(commonCustomizer, nameof(commonCustomizer));
            Ensure.IsNotNull(httpMessageHandler, nameof(httpMessageHandler));

            return commonCustomizer.UsingCustomHttpClient(
                new SystemHttpClientProvider(httpMessageHandler), 
                new SystemHttpMessageBuilderProvider(),
                new SystemHttpClientExceptionFactory());
        }
    }
}
