using NClient.Common.Helpers;
using NClient.Providers.Api.Rest;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class RestApiExtensions
    {
        /// <summary>The client should be used for a REST-like API.</summary>
        /// <typeparam name="TClient">The type of client interface.</typeparam>
        public static INClientTransportBuilder<TClient> UsingRestApi<TClient>(
            this INClientApiBuilder<TClient> apiBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(apiBuilder, nameof(apiBuilder));

            return apiBuilder.UsingCustomApi(new RestRequestBuilderProvider());
        }

        /// <summary>The client factory should be used for a REST-like API.</summary>
        public static INClientFactoryTransportBuilder UsingRestApi(
            this INClientFactoryApiBuilder apiBuilder)
        {
            Ensure.IsNotNull(apiBuilder, nameof(apiBuilder));

            return apiBuilder.UsingCustomApi(new RestRequestBuilderProvider());
        }
    }
}
