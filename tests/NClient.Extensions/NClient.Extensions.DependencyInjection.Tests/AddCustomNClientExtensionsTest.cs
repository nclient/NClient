using System.Net.Http;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NClient.Extensions.DependencyInjection.Tests.Helpers;
using NClient.Providers.Api.Rest.Extensions;
using NClient.Providers.Resilience;
using NUnit.Framework;
using Polly;
using RestSharp;

namespace NClient.Extensions.DependencyInjection.Tests
{
    [Parallelizable]
    public class AddCustomNClientExtensionsTest
    {
        [Test]
        public void AddCustomNClient_WithoutHttpClientAndLogging_NotBeNull()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddCustomNClient<ITestClientWithMetadata>(host: "http://localhost:5000", configure => configure
                .UsingRestApi()
                .UsingRestSharpTransport()
                .UsingNewtonsoftJsonSerializer()
                .Build());

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClientWithMetadata>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddCustomNClient_WithLogging_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();

            serviceCollection.AddCustomNClient<ITestClientWithMetadata>(host: "http://localhost:5000", configure => configure
                .UsingRestApi()
                .UsingRestSharpTransport()
                .UsingNewtonsoftJsonSerializer()
                .Build());

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClientWithMetadata>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddCustomNClient_BuilderWithCustomSettings_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();

            serviceCollection.AddCustomNClient<ITestClientWithMetadata>(host: "http://localhost:5000", configure => configure
                .UsingRestApi()
                .UsingRestSharpTransport()
                .UsingNewtonsoftJsonSerializer()
                .WithFullPollyResilience(Policy.NoOpAsync<IResponseContext<IRestRequest, IRestResponse>>())
                .Build());

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClientWithMetadata>();
            client.Should().NotBeNull();
        }
        
        [Test]
        public void AddCustomNClient_SingleClient_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();
            serviceCollection.AddHttpClient("TestClient");

            serviceCollection.AddCustomNClient<ITestClientWithMetadata>(host: "http://localhost:5000", (serviceProvider, configure) => configure
                .UsingRestApi()
                .UsingSystemHttpTransport(
                    httpClientFactory: serviceProvider.GetRequiredService<IHttpClientFactory>(),
                    httpClientName: "TestClient")
                .UsingJsonSerializer()
                .Build());

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClientWithMetadata>();
            client.Should().NotBeNull();
        }
    }
}
