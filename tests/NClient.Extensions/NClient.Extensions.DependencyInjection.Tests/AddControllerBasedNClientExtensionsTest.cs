using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NClient.Abstractions.Resilience;
using NClient.Extensions.DependencyInjection.Tests.Helpers;
using NUnit.Framework;
using Polly;

#pragma warning disable 618

namespace NClient.Extensions.DependencyInjection.Tests
{
    [Parallelizable]
    public class AddControllerBasedNClientExtensionsTest
    {
        [Test]
        public void AddNClient_WithoutHttpClientAndLogging_NotBeNull()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddNClient<ITestClient, TestController>(host: "http://localhost:5000");

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClient>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddNClient_OnlyHost_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddHttpClient().AddLogging();

            serviceCollection.AddNClient<ITestClient, TestController>(host: "http://localhost:5000");

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClient>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddNClient_NamedClient_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();
            serviceCollection.AddHttpClient("TestClient");

            serviceCollection.AddNClient<ITestClient, TestController>(
                host: "http://localhost:5000",
                httpClientName: "TestClient");

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClient>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddNClient_Builder_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddHttpClient().AddLogging();

            serviceCollection.AddNClient<ITestClient, TestController>(
                host: "http://localhost:5000", builder => builder.WithResiliencePolicy());

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClient>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddNClient_BuilderWithCustomSettings_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddHttpClient().AddLogging();

            serviceCollection.AddNClient<ITestClient, TestController>(
                host: "http://localhost:5000", builder => builder
                    .WithResiliencePolicy(Policy.NoOpAsync<ResponseContext>()));

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClient>();
            client.Should().NotBeNull();
        }
    }
}
