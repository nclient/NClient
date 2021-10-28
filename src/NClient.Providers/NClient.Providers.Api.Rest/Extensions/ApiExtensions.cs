using NClient.Common.Helpers;

namespace NClient.Providers.Api.Rest.Extensions
{
    // TODO: doc
    public static class ApiExtensions
    {
        public static INClientTransportBuilder<TClient> UsingRestApi<TClient>(
            this INClientApiBuilder<TClient> clientApiBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(clientApiBuilder, nameof(clientApiBuilder));

            return clientApiBuilder.UsingCustomApi(new RestRequestBuilderProvider());
        }
        
        public static INClientFactoryTransportBuilder UsingRestApi(
            this INClientFactoryApiBuilder factoryApiBuilder)
        {
            Ensure.IsNotNull(factoryApiBuilder, nameof(factoryApiBuilder));
            
            return factoryApiBuilder.UsingCustomApi(new RestRequestBuilderProvider());
        }
    }
}
