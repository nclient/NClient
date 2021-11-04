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

        public static INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> WithCustomResponseValidation(
            this INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> factoryOptionalBuilder)
        {
            return factoryOptionalBuilder.WithCustomResponseValidation(new DefaultSystemResponseValidatorSettings());
        }
    }
}
