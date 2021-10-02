using System.Net.Http;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NClient.Abstractions;
using NClient.Abstractions.Resilience;
using NClient.Providers.HttpClient.System;
using NClient.Providers.Resilience.Polly;
using NUnit.Framework;
using Polly;

namespace NClient.Extensions.DependencyInjection.Tests
{
    [Parallelizable]
    public class AddNClientFactoryExtensionsTest
    {
        [Test]
        public void AddNClientFactory_WithoutHttpClientAndLogging_NotBeNull()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddNClientFactory();

            var client = serviceCollection.BuildServiceProvider().GetService<INClientFactory>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddNClientFactory_WithLogging_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();

            serviceCollection.AddNClientFactory();

            var client = serviceCollection.BuildServiceProvider().GetService<INClientFactory>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddNClientFactory_SingleClient_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();
            serviceCollection.AddHttpClient("TestClient");

            serviceCollection.AddNClientFactory((serviceProvider, configure) => configure
                .UsingSystemHttpClient(
                    httpClientFactory: serviceProvider.GetRequiredService<IHttpClientFactory>(),
                    httpClientName: "TestClient"));

            var client = serviceCollection.BuildServiceProvider().GetService<INClientFactory>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddNClientFactory_Builder_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();

            serviceCollection.AddNClientFactory(builder => builder);

            var client = serviceCollection.BuildServiceProvider().GetService<INClientFactory>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddNClientFactory_BuilderWithCustomSettings_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();

            serviceCollection.AddNClientFactory(builder => builder
                .WithForcePollyResilience(Policy.NoOpAsync<ResponseContext<HttpRequestMessage, HttpResponseMessage>>()));

            var client = serviceCollection.BuildServiceProvider().GetService<INClientFactory>();
            client.Should().NotBeNull();
        }
    }
}
