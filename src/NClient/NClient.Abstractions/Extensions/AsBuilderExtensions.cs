// ReSharper disable once CheckNamespace

namespace NClient
{
    public static class AsBuilderExtensions
    {
        public static INClientAdvApiBuilder<TClient> AsAdvanced<TClient>(
            this INClientApiBuilder<TClient> clientApiBuilder)
            where TClient : class
        {
            return (INClientAdvApiBuilder<TClient>)clientApiBuilder;
        }
        
        public static INClientApiBuilder<TClient> AsBasic<TClient>(
            this INClientAdvApiBuilder<TClient> clientApiBuilder)
            where TClient : class
        {
            return clientApiBuilder;
        }
        
        public static INClientAdvTransportBuilder<TClient> AsAdvanced<TClient>(
            this INClientTransportBuilder<TClient> clientApiBuilder)
            where TClient : class
        {
            return (INClientAdvTransportBuilder<TClient>)clientApiBuilder;
        }
        
        public static INClientTransportBuilder<TClient> AsBasic<TClient>(
            this INClientAdvTransportBuilder<TClient> clientApiBuilder)
            where TClient : class
        {
            return clientApiBuilder;
        }
        
        public static INClientAdvSerializerBuilder<TClient, TRequest, TResponse> AsAdvanced<TClient, TRequest, TResponse>(
            this INClientSerializerBuilder<TClient, TRequest, TResponse> clientApiBuilder)
            where TClient : class
        {
            return (INClientAdvSerializerBuilder<TClient, TRequest, TResponse>)clientApiBuilder;
        }
        
        public static INClientSerializerBuilder<TClient, TRequest, TResponse> AsBasic<TClient, TRequest, TResponse>(
            this INClientAdvSerializerBuilder<TClient, TRequest, TResponse> clientApiBuilder)
            where TClient : class
        {
            return clientApiBuilder;
        }
        
        public static INClientAdvOptionalBuilder<TClient, TRequest, TResponse> AsAdvanced<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientApiBuilder)
            where TClient : class
        {
            return (INClientAdvOptionalBuilder<TClient, TRequest, TResponse>)clientApiBuilder;
        }
        
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> AsBasic<TClient, TRequest, TResponse>(
            this INClientAdvOptionalBuilder<TClient, TRequest, TResponse> clientApiBuilder)
            where TClient : class
        {
            return clientApiBuilder;
        }
    }
}
