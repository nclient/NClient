using System;
using NClient.Exceptions; 

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class AsClientBuilderExtensions
    {
        private static NClientException GetExceptionForAsAdvancedMethods(Type builderType) =>
            new($"The builder '{builderType.Name}' cannot be advanced.");
        
        private static NClientException GetExceptionForAsBasicMethods(Type builderType) =>
            new($"The builder '{builderType.Name}' cannot be basic.");
        
        public static INClientAdvancedApiBuilder<TClient> AsAdvanced<TClient>(
            this INClientApiBuilder<TClient> builder)
            where TClient : class
        {
            return builder as INClientAdvancedApiBuilder<TClient>
                ?? throw GetExceptionForAsAdvancedMethods(builder.GetType());
        }
        
        public static INClientApiBuilder<TClient> AsBasic<TClient>(
            this INClientAdvancedApiBuilder<TClient> builder)
            where TClient : class
        {
            return builder as INClientApiBuilder<TClient>
                ?? throw GetExceptionForAsBasicMethods(builder.GetType());
        }
        
        public static INClientAdvancedTransportBuilder<TClient> AsAdvanced<TClient>(
            this INClientTransportBuilder<TClient> builder)
            where TClient : class
        {
            return builder as INClientAdvancedTransportBuilder<TClient>
                ?? throw GetExceptionForAsAdvancedMethods(builder.GetType());
        }
        
        public static INClientTransportBuilder<TClient> AsBasic<TClient>(
            this INClientAdvancedTransportBuilder<TClient> builder)
            where TClient : class
        {
            return builder as INClientTransportBuilder<TClient>
                ?? throw GetExceptionForAsBasicMethods(builder.GetType());
        }
        
        public static INClientAdvancedSerializationBuilder<TClient, TRequest, TResponse> AsAdvanced<TClient, TRequest, TResponse>(
            this INClientSerializationBuilder<TClient, TRequest, TResponse> builder)
            where TClient : class
        {
            return builder as INClientAdvancedSerializationBuilder<TClient, TRequest, TResponse>
                ?? throw GetExceptionForAsAdvancedMethods(builder.GetType());
        }
        
        public static INClientSerializationBuilder<TClient, TRequest, TResponse> AsBasic<TClient, TRequest, TResponse>(
            this INClientAdvancedSerializationBuilder<TClient, TRequest, TResponse> builder)
            where TClient : class
        {
            return builder as INClientSerializationBuilder<TClient, TRequest, TResponse>
                ?? throw GetExceptionForAsBasicMethods(builder.GetType());
        }
        
        public static INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> AsAdvanced<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> builder)
            where TClient : class
        {
            return builder as INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse>
                ?? throw GetExceptionForAsAdvancedMethods(builder.GetType());
        }
        
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> AsBasic<TClient, TRequest, TResponse>(
            this INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> builder)
            where TClient : class
        {
            return builder as INClientOptionalBuilder<TClient, TRequest, TResponse>
                ?? throw GetExceptionForAsBasicMethods(builder.GetType());
        }
    }
}
