using System.Net.Http;
using NClient.Providers.Transport.Http.System;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class SystemResponseValidationExtensions
    {
        public static INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> WithSystemResponseValidation<TClient>(
            this INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> optionalBuilder) 
            where TClient : class
        {
            return optionalBuilder.WithAdvancedResponseValidation(x => x
                .ForTransport().Use(new DefaultSystemResponseValidatorSettings()));
        }

        public static INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> WithSystemResponseValidation(
            this INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> optionalBuilder)
        {
            return optionalBuilder.WithAdvancedResponseValidation(x => x
                .ForTransport().Use(new DefaultSystemResponseValidatorSettings()));
        }
    }
}
