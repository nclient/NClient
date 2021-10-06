using System.Net.Http;
using NClient.Abstractions.Ensuring;

namespace NClient.Api.Tests.Helpers
{
    public class CustomEnsuringSettings : EnsuringSettings<HttpRequestMessage, HttpResponseMessage>
    {
        public CustomEnsuringSettings() : base(
            isSuccess: _ => true, 
            onFailure: _ =>
            {
            })
        {
        }
    }
}
