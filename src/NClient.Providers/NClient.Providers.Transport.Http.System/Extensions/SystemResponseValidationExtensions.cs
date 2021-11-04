using System.Net.Http;
using NClient.Providers.Transport.Http.System;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class SystemResponseValidationExtensions
    {
        public static INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> WithSystemResponseValidation<TClient>(
            this INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> clientOptionalBuilder) 
            where TClient : class
        {
            return clientOptionalBuilder.AsAdvanced()
                .WithResponseValidation(x => x
                    .ForTransport().Use(new DefaultSystemResponseValidatorSettings()))
                .AsBasic();
        }

        public static INClientResponseValidationSelector<HttpRequestMessage, HttpResponseMessage> UseSystemResponseValidation(
            this INClientTransportResponseValidationSetter<HttpRequestMessage, HttpResponseMessage> transportResponseValidationSetter)
        {
            return transportResponseValidationSetter.Use(new DefaultSystemResponseValidatorSettings());
        }
        
        public static INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> UseSystemResponseValidation(
            this INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> factoryOptionalBuilder)
        {
            return factoryOptionalBuilder.WithCustomResponseValidation(new DefaultSystemResponseValidatorSettings());
        }
    }
}
