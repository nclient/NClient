using NClient.Providers.Transport.Http.RestSharp;
using RestSharp;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class RestSharpResponseValidationExtensions
    {
        public static INClientOptionalBuilder<TClient, IRestRequest, IRestResponse> WithRestSharpResponseValidation<TClient>(
            this INClientOptionalBuilder<TClient, IRestRequest, IRestResponse> factoryOptionalBuilder) 
            where TClient : class
        {
            return factoryOptionalBuilder.WithAdvancedResponseValidation(x => x
                .ForTransport().Use(new DefaultRestSharpResponseValidatorSettings()));
        }
        
        public static INClientFactoryOptionalBuilder<IRestRequest, IRestResponse> WithRestSharpResponseValidation(
            this INClientFactoryOptionalBuilder<IRestRequest, IRestResponse> factoryOptionalBuilder)
        {
            return factoryOptionalBuilder.WithAdvancedResponseValidation(x => x
                .ForTransport().Use(new DefaultRestSharpResponseValidatorSettings()));
        }
    }
}
