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
            return factoryOptionalBuilder.AsAdvanced()
                .WithResponseValidation(x => x
                    .ForTransport().Use(new DefaultRestSharpResponseValidatorSettings()))
                .AsBasic();
        }
        
        public static INClientResponseValidationSelector<IRestRequest, IRestResponse> UseRestSharpResponseValidation(
            this INClientTransportResponseValidationSetter<IRestRequest, IRestResponse> transportResponseValidationSetter)
        {
            return transportResponseValidationSetter.Use(new DefaultRestSharpResponseValidatorSettings());
        }
        
        public static INClientFactoryOptionalBuilder<IRestRequest, IRestResponse> WithRestSharpResponseValidation(
            this INClientFactoryOptionalBuilder<IRestRequest, IRestResponse> factoryOptionalBuilder)
        {
            return factoryOptionalBuilder.WithCustomResponseValidation(new DefaultRestSharpResponseValidatorSettings());
        }
    }
}
