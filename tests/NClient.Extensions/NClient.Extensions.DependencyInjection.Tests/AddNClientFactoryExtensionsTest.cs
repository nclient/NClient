using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NClient.Abstractions;
using NClient.Abstractions.HttpClients;
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
        public void AddNClientFactory_Default_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddHttpClient().AddLogging();

            serviceCollection.AddNClientFactory();

            var client = serviceCollection.BuildServiceProvider().GetService<INClientFactory>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddNClientFactory_NamedClient_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();
            serviceCollection.AddHttpClient("TestClient");

            serviceCollection.AddNClientFactory(httpClientName: "TestClient");

            var client = serviceCollection.BuildServiceProvider().GetService<INClientFactory>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddNClientFactory_Builder_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddHttpClient().AddLogging();

            serviceCollection.AddNClientFactory(builder => builder);

            var client = serviceCollection.BuildServiceProvider().GetService<INClientFactory>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddNClientFactory_BuilderWithCustomSettings_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddHttpClient().AddLogging();

            serviceCollection.AddNClientFactory(builder => builder
                .WithResiliencePolicy(Policy.NoOpAsync<HttpResponse>()));

            var client = serviceCollection.BuildServiceProvider().GetService<INClientFactory>();
            client.Should().NotBeNull();
        }
    }
}