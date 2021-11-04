using NClient.Common.Helpers;

namespace NClient.Providers.Api.Rest.Extensions
{
    // TODO: doc
    public static class RestApiExtensions
    {
        public static INClientAdvancedTransportBuilder<TClient> UsingRestApi<TClient>(
            this INClientAdvancedApiBuilder<TClient> clientAdvancedApiBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(clientAdvancedApiBuilder, nameof(clientAdvancedApiBuilder));

            return clientAdvancedApiBuilder.UsingCustomApi(new RestRequestBuilderProvider());
        }
        
        public static INClientTransportBuilder<TClient> UsingRestApi<TClient>(
            this INClientApiBuilder<TClient> clientApiBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(clientApiBuilder, nameof(clientApiBuilder));

            return UsingRestApi(clientApiBuilder.AsAdvanced()).AsBasic();
        }
        
        public static INClientFactoryAdvancedTransportBuilder UsingRestApi(
            this INClientFactoryAdvancedApiBuilder clientAdvancedApiBuilder)
        {
            Ensure.IsNotNull(clientAdvancedApiBuilder, nameof(clientAdvancedApiBuilder));

            return clientAdvancedApiBuilder.UsingCustomApi(new RestRequestBuilderProvider());
        }
        
        public static INClientFactoryTransportBuilder UsingRestApi(
            this INClientFactoryApiBuilder clientApiBuilder)
        {
            Ensure.IsNotNull(clientApiBuilder, nameof(clientApiBuilder));

            return UsingRestApi(clientApiBuilder.AsAdvanced()).AsBasic();
        }
    }
}
