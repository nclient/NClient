using NClient.Abstractions.Customization;
using NClient.Abstractions.HttpClients;
using NClient.Common.Helpers;
using RestSharp;
using RestSharp.Authenticators;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.HttpClient.RestSharp
{
    public static class CommonCustomizerExtensions
    {
        /// <summary>
        /// Sets RestSharp based <see cref="IHttpClientProvider{TRequest,TResponse}"/> used to create instance of <see cref="IHttpClient"/>.
        /// </summary>
        /// <param name="commonCustomizer"></param>
        public static TCustomizer UsingRestSharpHttpClient<TCustomizer, TInterface>(
            this INClientCommonCustomizer<TCustomizer, TInterface, IRestRequest, IRestResponse> commonCustomizer)
            where TCustomizer : INClientCommonCustomizer<TCustomizer, TInterface, IRestRequest, IRestResponse>
            where TInterface : class
        {
            Ensure.IsNotNull(commonCustomizer, nameof(commonCustomizer));

            return commonCustomizer.UsingCustomHttpClient(
                new RestSharpHttpClientProvider(),
                new RestSharpHttpMessageBuilderProvider(),
                new RestSharpHttpClientExceptionFactory());
        }

        /// <summary>
        /// Sets RestSharp based <see cref="IHttpClientProvider{TRequest,TResponse}"/> used to create instance of <see cref="IHttpClient"/>.
        /// </summary>
        /// <param name="commonCustomizer"></param>
        /// <param name="authenticator">The RestSharp authenticator.</param>
        public static TCustomizer UsingRestSharpHttpClient<TCustomizer, TInterface>(
            this INClientCommonCustomizer<TCustomizer, TInterface, IRestRequest, IRestResponse> commonCustomizer,
            IAuthenticator authenticator)
            where TCustomizer : INClientCommonCustomizer<TCustomizer, TInterface, IRestRequest, IRestResponse>
            where TInterface : class
        {
            Ensure.IsNotNull(commonCustomizer, nameof(commonCustomizer));

            return commonCustomizer.UsingCustomHttpClient(
                new RestSharpHttpClientProvider(authenticator),
                new RestSharpHttpMessageBuilderProvider(),
                new RestSharpHttpClientExceptionFactory());
        }
    }
}
