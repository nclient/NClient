using System.Net.Http;
using NClient.Abstractions.Validation;

namespace NClient.Api.Tests.Helpers
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
