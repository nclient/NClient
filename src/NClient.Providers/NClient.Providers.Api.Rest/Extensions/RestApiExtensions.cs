using NClient.Common.Helpers;

namespace NClient.Providers.Api.Rest.Extensions
{
    // TODO: doc
    public static class RestApiExtensions
    {
        public static INClientTransportBuilder<TClient> UsingRestApi<TClient>(
            this INClientApiBuilder<TClient> clientAdvancedApiBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(clientAdvancedApiBuilder, nameof(clientAdvancedApiBuilder));

            return clientAdvancedApiBuilder.UsingCustomApi(new RestRequestBuilderProvider());
        }

        public static INClientFactoryTransportBuilder UsingRestApi(
            this INClientFactoryApiBuilder clientAdvancedApiBuilder)
        {
            Ensure.IsNotNull(clientAdvancedApiBuilder, nameof(clientAdvancedApiBuilder));

            return clientAdvancedApiBuilder.UsingCustomApi(new RestRequestBuilderProvider());
        }
    }
}
