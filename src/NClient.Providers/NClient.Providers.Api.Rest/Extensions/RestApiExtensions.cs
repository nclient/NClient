using NClient.Common.Helpers;

namespace NClient.Providers.Api.Rest.Extensions
{
    // TODO: doc
    public static class RestApiExtensions
    {
        public static INClientTransportBuilder<TClient> UsingRestApi<TClient>(
            this INClientApiBuilder<TClient> apiBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(apiBuilder, nameof(apiBuilder));

            return apiBuilder.UsingCustomApi(new RestRequestBuilderProvider());
        }

        public static INClientFactoryTransportBuilder UsingRestApi(
            this INClientFactoryApiBuilder apiBuilder)
        {
            Ensure.IsNotNull(apiBuilder, nameof(apiBuilder));

            return apiBuilder.UsingCustomApi(new RestRequestBuilderProvider());
        }
    }
}
