using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NClient.Abstractions.Clients;
using NClient.Abstractions.HttpClients;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Extensions.DependencyInjection;
using NClient.Packages.Tests.Helpers;
using NUnit.Framework;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Polly;

namespace NClient.Packages.Tests
{
    [Parallelizable]
    [Category("Packages")]
    public class ClientTest
    {
        public interface ITestController : INClient
        {
            Task<string> GetAsync(int id);
        }

        [ApiController]
        [Route("api/[controller]")]
        public class TestController : ControllerBase, ITestController
        {
            [HttpGet("[action]")]
            public Task<string> GetAsync(int id) => Task.FromResult("result");
        }

        [Test]
        public async Task ControllerBasedClient()
        {
            const int id = 1;
            const string host = "http://localhost:5001";
            var policy = Policy
                .HandleResult<HttpResponse>(x => !x.IsSuccessful)
                .Or<Exception>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromMilliseconds(Math.Pow(2, retryAttempt)));
            var client = new ServiceCollection()
                .AddHttpClient()
                .AddNClient<ITestController, TestController>(host, policy)
                .AddLogging()
                .BuildServiceProvider()
                .GetRequiredService<ITestController>();
            using var server = RunMockServer(host, id);

            var result = await client.GetAsync(id);

            PackagesVersionProvider.GetCurrent<NClientBuilder>().Should().Be(PackagesVersionProvider.GetNew());
            result.Should().Be("result");
        }

        [Path("api/[controller]")]
        public interface ITest : INClient
        {
            [GetMethod("[action]")]
            public Task<string> GetAsync(int id) => Task.FromResult("result");
        }

        [Test]
        public async Task InterfaceBasedClient()
        {
            const int id = 1;
            const string host = "http://localhost:5002";
            var policy = Policy
                .HandleResult<HttpResponse>(x => !x.IsSuccessful)
                .Or<Exception>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromMilliseconds(Math.Pow(2, retryAttempt)));
            var client = new ServiceCollection()
                .AddHttpClient()
                .AddNClient<ITest>(host, policy)
                .AddLogging()
                .BuildServiceProvider()
                .GetRequiredService<ITest>();
            using var server = RunMockServer(host, id);

            var result = await client.GetAsync(id);

            PackagesVersionProvider.GetCurrent<NClientBuilder>().Should().Be(PackagesVersionProvider.GetNew());
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
