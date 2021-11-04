// ReSharper disable once CheckNamespace

namespace NClient
{
    // TODO: Add validation
    public static class AsFactoryBuilderExtensions
    {
        public static INClientFactoryAdvancedApiBuilder AsAdvanced(
            this INClientFactoryApiBuilder clientApiBuilder)
        {
            return (INClientFactoryAdvancedApiBuilder)clientApiBuilder;
        }
        
        public static INClientFactoryApiBuilder AsBasic(
            this INClientFactoryAdvancedApiBuilder clientApiBuilder)
        {
            return (INClientFactoryApiBuilder)clientApiBuilder;
        }
        
        public static INClientFactoryAdvancedTransportBuilder AsAdvanced(
            this INClientFactoryTransportBuilder clientApiBuilder)
        {
            return (INClientFactoryAdvancedTransportBuilder)clientApiBuilder;
        }
        
        public static INClientFactoryTransportBuilder AsBasic(
            this INClientFactoryAdvancedTransportBuilder clientApiBuilder)
        {
            return (INClientFactoryTransportBuilder)clientApiBuilder;
        }
        
        public static INClientFactoryAdvancedSerializerBuilder<TRequest, TResponse> AsAdvanced<TRequest, TResponse>(
            this INClientFactorySerializerBuilder<TRequest, TResponse> clientApiBuilder)
        {
            return (INClientFactoryAdvancedSerializerBuilder<TRequest, TResponse>)clientApiBuilder;
        }
        
        public static INClientFactorySerializerBuilder<TRequest, TResponse> AsBasic<TRequest, TResponse>(
            this INClientFactoryAdvancedSerializerBuilder<TRequest, TResponse> clientApiBuilder)
        {
            return (INClientFactorySerializerBuilder<TRequest, TResponse>)clientApiBuilder;
        }
        
        public static INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> AsAdvanced<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> clientApiBuilder)
        {
            return (INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse>)clientApiBuilder;
        }
        
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> AsBasic<TRequest, TResponse>(
            this INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> clientApiBuilder)
        {
            return (INClientFactoryOptionalBuilder<TRequest, TResponse>)clientApiBuilder;
        }
    }
}
