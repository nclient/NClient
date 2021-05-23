using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NClient.Abstractions.HttpClients;
using NClient.Extensions.DependencyInjection.Tests.Helpers;
using NUnit.Framework;
using Polly;

namespace NClient.Extensions.DependencyInjection.Tests
{
    [Parallelizable]
    public class AddInterfaceBasedNClientExtensionsTest
    {
        [Test]
        public void AddNClient_ClientBuilder_NotBeNull()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddNClient(builder =>
                builder.Use<ITestClient>(host: "http://localhost:5000"));

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClient>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddNClient_JsonSerializerOptionsAndAsyncPolicy_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddHttpClient().AddLogging();

            serviceCollection.AddNClient<ITestClient>(
                host: "http://localhost:5000",
                new JsonSerializerOptions(),
                Policy.NoOpAsync<HttpResponse>());

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClient>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddNClient_JsonSerializerOptions_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddHttpClient().AddLogging();

            serviceCollection.AddNClient<ITestClient>(
                host: "http://localhost:5000",
                new JsonSerializerOptions());

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClient>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddNClient_AsyncPolicy_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddHttpClient().AddLogging();

            serviceCollection.AddNClient<ITestClient>(
                host: "http://localhost:5000",
                Policy.NoOpAsync<HttpResponse>());

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClient>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddNClient_OnlyHost_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddHttpClient().AddLogging();

            serviceCollection.AddNClient<ITestClient>(
                host: "http://localhost:5000");

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClient>();
            client.Should().NotBeNull();
        }
    }
}
