using System;
using System.Net.Http;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Resilience.Settings;

namespace NClient.Resilience
{
    public class NoResiliencePolicySettings : NoResiliencePolicySettingsBase<HttpRequestMessage, HttpResponseMessage>
    {
        public override Func<ResponseContext<HttpRequestMessage, HttpResponseMessage>, bool> ResultPredicate { get; set; } = context => !context.Response.IsSuccessStatusCode;
    }
}
