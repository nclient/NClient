using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NClient.Providers.HttpClient.RestSharp;
using NClient.Providers.Resilience.Polly;
using NClient.Testing.Common.Clients;
using NUnit.Framework;
using Polly;

namespace NClient.Extensions.DependencyInjection.Tests
{
    [Parallelizable]
    public class ServiceCollectionExtensionsTest
    {
        [Test]
        public void AddNClient_ClientProvider_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();

            serviceCollection.AddNClient(provider => 
                provider.Use<IBasicClient>(host: "http://localhost:5000", new RestSharpHttpClientProvider()));

            var client = serviceCollection.BuildServiceProvider().GetService<IBasicClient>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddNClient_HttpClientAndResilienceProviders_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();

            serviceCollection.AddNClient<IBasicClient>(
                host: "http://localhost:5000", 
                new RestSharpHttpClientProvider(),
                new PollyResiliencePolicyProvider(Policy.NoOpAsync()));

            var client = serviceCollection.BuildServiceProvider().GetService<IBasicClient>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddNClient_HttpClientProvider_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();

            serviceCollection.AddNClient<IBasicClient>(
                host: "http://localhost:5000",
                new RestSharpHttpClientProvider());

            var client = serviceCollection.BuildServiceProvider().GetService<IBasicClient>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddNClient_AsyncPolicy_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();

            serviceCollection.AddNClient<IBasicClient>(
                host: "http://localhost:5000",
                Policy.NoOpAsync());

            var client = serviceCollection.BuildServiceProvider().GetService<IBasicClient>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddNClient_OnlyHost_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();

            serviceCollection.AddNClient<IBasicClient>(
                host: "http://localhost:5000");

            var client = serviceCollection.BuildServiceProvider().GetService<IBasicClient>();
            client.Should().NotBeNull();
        }
    }
}
