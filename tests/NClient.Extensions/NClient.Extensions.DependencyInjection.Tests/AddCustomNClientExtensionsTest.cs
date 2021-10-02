using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NClient.Abstractions.Resilience;
using NClient.Extensions.DependencyInjection.Tests.Helpers;
using NClient.Providers.HttpClient.RestSharp;
using NClient.Providers.Resilience.Polly;
using NClient.Providers.Serialization.Newtonsoft;
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

            serviceCollection.AddCustomNClient<ITestClientWithMetadata, IRestRequest, IRestResponse>(host: "http://localhost:5000", configure => configure
                .UsingRestSharpHttpClient()
                .UsingNewtonsoftJsonSerializer());

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClientWithMetadata>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddCustomNClient_WithLogging_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();

            serviceCollection.AddCustomNClient<ITestClientWithMetadata, IRestRequest, IRestResponse>(host: "http://localhost:5000", configure => configure
                .UsingRestSharpHttpClient()
                .UsingNewtonsoftJsonSerializer());

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClientWithMetadata>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddCustomNClient_BuilderWithCustomSettings_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();

            serviceCollection.AddCustomNClient<ITestClientWithMetadata, IRestRequest, IRestResponse>(host: "http://localhost:5000", configure => configure
                .UsingRestSharpHttpClient()
                .UsingNewtonsoftJsonSerializer()
                .WithForcePollyResilience(Policy.NoOpAsync<ResponseContext<IRestRequest, IRestResponse>>()));

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClientWithMetadata>();
            client.Should().NotBeNull();
        }
    }
}
