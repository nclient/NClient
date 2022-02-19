using System;
using System.Net.Http;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NClient.Extensions.DependencyInjection.Tests.Helpers;
using NClient.Providers.Transport;
using NUnit.Framework;
using Polly;

namespace NClient.Extensions.DependencyInjection.Tests
{
    [Parallelizable]
    public class AddRestNClientExtensionsTest
    {
        [Test]
        public void AddRestNClient_WithoutHttpClientAndLogging_NotBeNull()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddRestNClient<ITestClientWithMetadata>(host: "http://localhost:5000");

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClientWithMetadata>();
            client.Should().NotBeNull();
        }
        
        [Test]
        public void AddRestNClient_WithHostProvider_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddSingleton<string>("http://localhost:5000");

            serviceCollection.AddRestNClient<ITestClientWithMetadata>(serviceProvider => 
                serviceProvider.GetRequiredService<string>());

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClientWithMetadata>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddRestNClient_WithLogging_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();

            serviceCollection.AddRestNClient<ITestClientWithMetadata>(host: "http://localhost:5000");

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClientWithMetadata>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddRestNClient_Builder_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();

            serviceCollection.AddRestNClient<ITestClientWithMetadata>(host: "http://localhost:5000")
                .ConfigureNClient(builder => builder
                    .WithFullResilience(getDelay: _ => TimeSpan.FromSeconds(0)));

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClientWithMetadata>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddRestNClient_BuilderWithCustomSettings_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();

            serviceCollection.AddRestNClient<ITestClientWithMetadata>(host: "http://localhost:5000")
                .ConfigureNClient(builder => builder
                    .WithPollyFullResilience(Policy.NoOpAsync<IResponseContext<HttpRequestMessage, HttpResponseMessage>>()));

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClientWithMetadata>();
            client.Should().NotBeNull();
        }
    }
}
