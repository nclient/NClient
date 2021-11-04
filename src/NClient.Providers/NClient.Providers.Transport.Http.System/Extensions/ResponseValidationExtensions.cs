using System.Net.Http;
using NClient.Providers.Transport.Http.System;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class ResponseValidationExtensions
    {
        public static INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> WithSystemResponseValidation<TClient>(
            this INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> clientOptionalBuilder) 
            where TClient : class
        {
            return clientOptionalBuilder.AsAdvanced()
                .WithResponseValidation(x => x
                    .WithCustomResponseValidation(new DefaultSystemResponseValidatorSettings()))
                .AsBasic();
        }
        
        public static INClientAdvancedResponseValidationSetter<HttpRequestMessage, HttpResponseMessage> WithSystemResponseValidation(
            this INClientAdvancedResponseValidationSetter<HttpRequestMessage, HttpResponseMessage> advancedResponseValidationSetter)
        {
            return advancedResponseValidationSetter.WithCustomResponseValidation(new DefaultSystemResponseValidatorSettings());
        }
        
        public static INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> WithSystemResponseValidation(
            this INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> factoryOptionalBuilder)
        {
            return factoryOptionalBuilder.WithCustomResponseValidation(new DefaultSystemResponseValidatorSettings());
        }
    }
}
