using System;
using System.Net.Http;
using System.Threading.Tasks;
using NClient.Abstractions.Resilience;
using NClient.Abstractions.Resilience.Settings;

namespace NClient.Resilience
{
    public class NoResiliencePolicySettings : NoResiliencePolicySettingsBase<HttpRequestMessage, HttpResponseMessage>
    {
        public override Func<ResponseContext<HttpRequestMessage, HttpResponseMessage>, bool> ResultPredicate { get; set; } = context => !context.Response.IsSuccessStatusCode;
        public override Func<ResponseContext<HttpRequestMessage, HttpResponseMessage>, Task> OnFallbackAsync { get; set; } = context =>
        {
            context.Response.EnsureSuccessStatusCode();
            return Task.CompletedTask;
        };
    }
}
