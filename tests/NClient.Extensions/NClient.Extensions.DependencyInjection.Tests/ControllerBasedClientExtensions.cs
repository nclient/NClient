using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NClient.Abstractions.HttpClients;
using NClient.Providers.HttpClient.RestSharp;
using NClient.Providers.Resilience.Polly;
using NUnit.Framework;
using Polly;

namespace NClient.Extensions.DependencyInjection.Tests
{
    [Parallelizable]
    public class ControllerBasedClientExtensions
    {
        [Test]
        public void AddNClient_ClientProvider_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();

            serviceCollection.AddNClient(builder => 
                builder.Use<ITestClient>(host: "http://localhost:5000", new RestSharpHttpClientProvider()));

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClient>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddNClient_HttpClientAndResilienceProviders_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();

            serviceCollection.AddNClient<ITestClient>(
                host: "http://localhost:5000", 
                new RestSharpHttpClientProvider(),
                new PollyResiliencePolicyProvider(Policy.NoOpAsync<HttpResponse>()));

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClient>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddNClient_HttpClientProvider_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();

            serviceCollection.AddNClient<ITestClient>(
                host: "http://localhost:5000",
                new RestSharpHttpClientProvider());

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClient>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddNClient_AsyncPolicy_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();

            serviceCollection.AddNClient<ITestClient>(
                host: "http://localhost:5000",
                Policy.NoOpAsync<HttpResponse>());

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClient>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddNClient_OnlyHost_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();

            serviceCollection.AddNClient<ITestClient>(
                host: "http://localhost:5000");

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClient>();
            client.Should().NotBeNull();
        }
    }
}
