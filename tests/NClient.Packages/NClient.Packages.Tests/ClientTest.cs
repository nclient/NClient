using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NClient.Abstractions;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Extensions.DependencyInjection;
using NClient.Packages.Tests.Helpers;
using NUnit.Framework;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

#pragma warning disable 618

namespace NClient.Packages.Tests
{
    [Parallelizable]
    [Category("Packages")]
    public class ClientTest
    {
        [Path("api/[controller]")]
        public interface ITest : INClient
        {
            [GetMethod("[action]")]
            public Task<string> GetAsync(int id);
        }

        [Test]
        public async Task TestClient()
        {
            const int id = 1;
            const string host = "http://localhost:5002";
            var serviceCollection = new ServiceCollection().AddLogging();
            serviceCollection.AddNClient<ITest>(host, builder => builder
                .WithSafeResilience(getDelay: _ => TimeSpan.FromSeconds(0)).Build());
            var client = serviceCollection
                .BuildServiceProvider()
                .GetRequiredService<ITest>();
            using var server = RunMockServer(host, id);

            var result = await client.GetAsync(id);

            PackagesVersionProvider.GetCurrent<NClientGallery>().Should().Be(PackagesVersionProvider.GetNew());
            result.Should().Be("result");
        }

        private static IWireMockServer RunMockServer(string host, int id)
        {
            var api = WireMockServer.Start(host);
            api.Given(Request.Create()
                    .WithPath("/api/Test/GetAsync")
                    .WithParam("id", id.ToString())
                    .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson("result"));
            return api;
        }
    }
}
