using System;
using System.Threading.Tasks;
using NClient.Abstractions.Resilience.Settings;
using NClient.Providers.HttpClient.RestSharp;
using NClient.Providers.Resilience.Polly;
using NClient.Providers.Serialization.Newtonsoft;
using NClient.Testing.Common.Clients;
using NUnit.Framework;
using RestSharp;

namespace NClient.Api.Tests
{
    public class CustomClientUsecasesTest
    {
        [Test]
        public void Test()
        {
            var client = new CustomNClientBuilder<IRestRequest, IRestResponse>()
                .For<IBasicClient>(host: "")
                .UsingRestSharpHttpClient()
                .UsingNewtonsoftJsonSerializer()
                .WithoutHandling()
                .WithCustomResilience(customizer => customizer
                    .ForMethod(x => (Func<int, Task<int>>)x.GetAsync)
                    .Use(new ConfiguredPollyResiliencePolicyProvider<IRestRequest, IRestResponse>(new ResiliencePolicySettings<IRestRequest, IRestResponse>
                    (
                        retryCount: 0,
                        sleepDuration: _ => TimeSpan.FromSeconds(0),
                        resultPredicate: _ => false,
                        onFallbackAsync: _ => Task.CompletedTask
                    ))))
                .WithoutLogging();
        }
    }
}
