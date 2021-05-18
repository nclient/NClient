using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NClient.Abstractions.HttpClients;
using NClient.Providers.HttpClient.System;
using NClient.Providers.Resilience.Polly;
using NClient.Providers.Serialization.System;
using NUnit.Framework;
using Polly;

#pragma warning disable 618

namespace NClient.Extensions.DependencyInjection.Tests
{
    [Parallelizable]
    public class InterfaceBasedClientExtensions
    {
        [Test]
        public void AddNClient_ClientBuilder_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();

            serviceCollection.AddNClient(builder => builder
                    .Use<ITestClient, TestController>(host: "http://localhost:5000", new SystemHttpClientProvider(), new SystemSerializerProvider()));

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClient>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddNClient_JsonSerializerOptionsAndAsyncPolicy_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddHttpClient().AddLogging();

            serviceCollection.AddNClient<ITestClient, TestController>(
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

            serviceCollection.AddNClient<ITestClient, TestController>(
                host: "http://localhost:5000",
                new JsonSerializerOptions());

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClient>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddNClient_AsyncPolicy_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddHttpClient().AddLogging();

            serviceCollection.AddNClient<ITestClient, TestController>(
                host: "http://localhost:5000",
                Policy.NoOpAsync<HttpResponse>());

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClient>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddNClient_OnlyHost_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddHttpClient().AddLogging();

            serviceCollection.AddNClient<ITestClient, TestController>(
                host: "http://localhost:5000");

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClient>();
            client.Should().NotBeNull();
        }
    }
}
