using System.Net.Http;
using NClient.Providers.Transport.Http.System;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class UseSystemResponseValidationExtensions
    {
        public static INClientResponseValidationSelector<HttpRequestMessage, HttpResponseMessage> UseSystemResponseValidation(
            this INClientTransportResponseValidationSetter<HttpRequestMessage, HttpResponseMessage> transportResponseValidationSetter)
        {
            return transportResponseValidationSetter.Use(new DefaultSystemResponseValidatorSettings());
        }
    }
}
