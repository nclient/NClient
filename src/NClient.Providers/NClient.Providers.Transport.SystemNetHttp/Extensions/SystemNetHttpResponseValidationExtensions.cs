using System.Net.Http;
using NClient.Providers.Transport.SystemNetHttp;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class SystemNetHttpResponseValidationExtensions
    {
        /// <summary>Sets default System.Net.Http validation the contents of the response received from transport.</summary>
        public static INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> WithSystemNetHttpResponseValidation<TClient>(
            this INClientOptionalBuilder<TClient, HttpRequestMessage, HttpResponseMessage> optionalBuilder) 
            where TClient : class
        {
            return optionalBuilder.WithAdvancedResponseValidation(x => x
                .ForTransport().Use(new DefaultSystemNetHttpResponseValidatorSettings()));
        }

        /// <summary>Sets default System.Net.Http validation the contents of the response received from transport.</summary>
        public static INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> WithSystemNetHttpResponseValidation(
            this INClientFactoryOptionalBuilder<HttpRequestMessage, HttpResponseMessage> optionalBuilder)
        {
            return optionalBuilder.WithAdvancedResponseValidation(x => x
                .ForTransport().Use(new DefaultSystemNetHttpResponseValidatorSettings()));
        }
    }
}
