using System.Net.Http;
using NClient.Providers.Validation;

namespace NClient.Api.Tests.Stubs
{
    public class CustomResponseValidatorSettings : ResponseValidatorSettings<HttpRequestMessage, HttpResponseMessage>
    {
        public CustomResponseValidatorSettings() : base(
            isSuccess: _ => true, 
            onFailure: _ =>
            {
            })
        {
        }
    }
}
