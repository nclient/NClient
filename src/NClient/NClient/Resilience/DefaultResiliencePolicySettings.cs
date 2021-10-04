using System;
using System.Net.Http;
using System.Threading.Tasks;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Resilience.Settings;

namespace NClient.Resilience
{
    // TODO: это не Defaul, Defaul - без ретрая
    public class DefaultResiliencePolicySettings : DefaultResiliencePolicySettingsBase<HttpRequestMessage, HttpResponseMessage>
    {
        public override Func<ResponseContext<HttpRequestMessage, HttpResponseMessage>, bool> ResultPredicate { get; set; } = context => !context.Response.IsSuccessStatusCode;
    }
}
