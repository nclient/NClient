using System.Net.Http;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NClient.Abstractions.Resilience;
using NClient.Extensions.DependencyInjection.Tests.Helpers;
using NClient.Providers.HttpClient.System;
using NClient.Providers.Resilience.Polly;
using NUnit.Framework;
using Polly;

namespace NClient.Extensions.DependencyInjection.Tests
{
    [Parallelizable]
    public class AddNClientExtensionsTest
    {
        [Test]
        public void AddNClient_WithoutHttpClientAndLogging_NotBeNull()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddNClient<ITestClientWithMetadata>(host: "http://localhost:5000");

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClientWithMetadata>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddNClient_WithLogging_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();

            serviceCollection.AddNClient<ITestClientWithMetadata>(host: "http://localhost:5000");

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClientWithMetadata>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddNClient_SingleClient_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();
            serviceCollection.AddHttpClient("TestClient");

            serviceCollection.AddNClient<ITestClientWithMetadata>(host: "http://localhost:5000", (serviceProvider, configure) => configure
                .UsingSystemHttpClient(
                    httpClientFactory: serviceProvider.GetRequiredService<IHttpClientFactory>(),
                    httpClientName: "TestClient"));

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClientWithMetadata>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddNClient_Builder_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();

            serviceCollection.AddNClient<ITestClientWithMetadata>(
                host: "http://localhost:5000", builder => builder.WithForceResilience());

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClientWithMetadata>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddNClient_BuilderWithCustomSettings_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();

            serviceCollection.AddNClient<ITestClientWithMetadata>(
                host: "http://localhost:5000", builder => builder
                    .WithForcePollyResilience(Policy.NoOpAsync<ResponseContext<HttpRequestMessage, HttpResponseMessage>>()));

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClientWithMetadata>();
            client.Should().NotBeNull();
        }
    }
}
