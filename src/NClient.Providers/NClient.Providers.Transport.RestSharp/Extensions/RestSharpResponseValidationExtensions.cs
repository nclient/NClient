using NClient.Providers.Transport.RestSharp;
using RestSharp;

// ReSharper disable once CheckNamespace
namespace NClient
{
    public static class RestSharpResponseValidationExtensions
    {
        /// <summary>Sets default RestSharp validation the contents of the response received from transport.</summary>
        public static INClientOptionalBuilder<TClient, IRestRequest, IRestResponse> WithRestSharpResponseValidation<TClient>(
            this INClientOptionalBuilder<TClient, IRestRequest, IRestResponse> factoryOptionalBuilder) 
            where TClient : class
        {
            return factoryOptionalBuilder.WithAdvancedResponseValidation(x => x
                .ForTransport().Use(new DefaultRestSharpResponseValidatorSettings()));
        }
        
        /// <summary>Sets default RestSharp validation the contents of the response received from transport.</summary>
        public static INClientFactoryOptionalBuilder<IRestRequest, IRestResponse> WithRestSharpResponseValidation(
            this INClientFactoryOptionalBuilder<IRestRequest, IRestResponse> factoryOptionalBuilder)
        {
            return factoryOptionalBuilder.WithAdvancedResponseValidation(x => x
                .ForTransport().Use(new DefaultRestSharpResponseValidatorSettings()));
        }
    }
}
