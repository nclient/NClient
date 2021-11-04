using NClient.Providers.Transport.Http.RestSharp;
using RestSharp;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class ResponseValidationExtensions
    {
        public static INClientOptionalBuilder<TClient, IRestRequest, IRestResponse> WithRestSharpResponseValidation<TClient>(
            this INClientOptionalBuilder<TClient, IRestRequest, IRestResponse> factoryOptionalBuilder) 
            where TClient : class
        {
            return factoryOptionalBuilder.AsAdvanced()
                .WithResponseValidation(x => x
                    .WithCustomResponseValidation(new DefaultRestSharpResponseValidatorSettings()))
                .AsBasic();
        }
        
        public static INClientAdvancedResponseValidationSetter<IRestRequest, IRestResponse> WithRestSharpResponseValidation(
            this INClientAdvancedResponseValidationSetter<IRestRequest, IRestResponse> advancedResponseValidationSetter)
        {
            return advancedResponseValidationSetter.WithCustomResponseValidation(new DefaultRestSharpResponseValidatorSettings());
        }
        
        public static INClientFactoryOptionalBuilder<IRestRequest, IRestResponse> WithRestSharpResponseValidation(
            this INClientFactoryOptionalBuilder<IRestRequest, IRestResponse> factoryOptionalBuilder)
        {
            return factoryOptionalBuilder.WithCustomResponseValidation(new DefaultRestSharpResponseValidatorSettings());
        }
    }
}
