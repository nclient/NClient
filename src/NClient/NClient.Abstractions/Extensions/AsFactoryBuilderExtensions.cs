using System;
using NClient.Exceptions; 

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class AsFactoryBuilderExtensions
    {
        private static NClientException GetExceptionForAsAdvancedMethods(Type builderType) =>
            new($"The builder '{builderType.Name}' cannot be advanced.");
        
        private static NClientException GetExceptionForAsBasicMethods(Type builderType) =>
            new($"The builder '{builderType.Name}' cannot be basic.");
        
        public static INClientFactoryAdvancedApiBuilder AsAdvanced(
            this INClientFactoryApiBuilder builder)
        {
            return builder as INClientFactoryAdvancedApiBuilder
                ?? throw GetExceptionForAsAdvancedMethods(builder.GetType());
        }
        
        public static INClientFactoryApiBuilder AsBasic(
            this INClientFactoryAdvancedApiBuilder builder)
        {
            return builder as INClientFactoryApiBuilder
                ?? throw GetExceptionForAsBasicMethods(builder.GetType());
        }
        
        public static INClientFactoryAdvancedTransportBuilder AsAdvanced(
            this INClientFactoryTransportBuilder builder)
        {
            return builder as INClientFactoryAdvancedTransportBuilder
                ?? throw GetExceptionForAsAdvancedMethods(builder.GetType());
        }
        
        public static INClientFactoryTransportBuilder AsBasic(
            this INClientFactoryAdvancedTransportBuilder builder)
        {
            return builder as INClientFactoryTransportBuilder
                ?? throw GetExceptionForAsBasicMethods(builder.GetType());
        }
        
        public static INClientFactoryAdvancedSerializationBuilder<TRequest, TResponse> AsAdvanced<TRequest, TResponse>(
            this INClientFactorySerializationBuilder<TRequest, TResponse> builder)
        {
            return builder as INClientFactoryAdvancedSerializationBuilder<TRequest, TResponse>
                ?? throw GetExceptionForAsAdvancedMethods(builder.GetType());
        }
        
        public static INClientFactorySerializationBuilder<TRequest, TResponse> AsBasic<TRequest, TResponse>(
            this INClientFactoryAdvancedSerializationBuilder<TRequest, TResponse> builder)
        {
            return builder as INClientFactorySerializationBuilder<TRequest, TResponse>
                ?? throw GetExceptionForAsBasicMethods(builder.GetType());
        }
        
        public static INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> AsAdvanced<TRequest, TResponse>(
            this INClientFactoryOptionalBuilder<TRequest, TResponse> builder)
        {
            return builder as INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse>
                ?? throw GetExceptionForAsAdvancedMethods(builder.GetType());
        }
        
        public static INClientFactoryOptionalBuilder<TRequest, TResponse> AsBasic<TRequest, TResponse>(
            this INClientFactoryAdvancedOptionalBuilder<TRequest, TResponse> builder)
        {
            return builder as INClientFactoryOptionalBuilder<TRequest, TResponse>
                ?? throw GetExceptionForAsBasicMethods(builder.GetType());
        }
    }
}
