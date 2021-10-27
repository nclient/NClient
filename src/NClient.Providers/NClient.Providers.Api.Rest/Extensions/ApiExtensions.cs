using NClient.Common.Helpers;

namespace NClient.Providers.Api.Rest.Extensions
{
    // TODO: doc
    public static class ApiExtensions
    {
        public static INClientSerializerBuilder<TClient, TRequest, TResponse> UsingRestApi<TClient, TRequest, TResponse>(
            this INClientApiBuilder<TClient, TRequest, TResponse> clientApiBuilder)
            where TClient : class
        {
            Ensure.IsNotNull(clientApiBuilder, nameof(clientApiBuilder));

            return clientApiBuilder.UsingCustomApi(new RestRequestBuilderProvider());
        }
        
        public static INClientFactorySerializerBuilder<TRequest, TResponse> UsingRestApi<TRequest, TResponse>(
            this INClientFactoryApiBuilder<TRequest, TResponse> factoryApiBuilder)
        {
            Ensure.IsNotNull(factoryApiBuilder, nameof(factoryApiBuilder));
            
            return factoryApiBuilder.UsingCustomApi(new RestRequestBuilderProvider());
        }
    }
}
