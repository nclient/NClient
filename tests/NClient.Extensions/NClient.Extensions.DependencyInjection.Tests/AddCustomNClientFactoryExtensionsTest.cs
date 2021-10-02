using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NClient.Abstractions;
using NClient.Abstractions.Resilience;
using NClient.Providers.HttpClient.RestSharp;
using NClient.Providers.Resilience.Polly;
using NClient.Providers.Serialization.Newtonsoft;
using NUnit.Framework;
using Polly;
using RestSharp;

namespace NClient.Extensions.DependencyInjection.Tests
{
    [Parallelizable]
    public class AddCustomNClientFactoryExtensionsTest
    {
        [Test]
        public void AddCustomNClientFactory_WithoutHttpClientAndLogging_NotBeNull()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddCustomNClientFactory<IRestRequest, IRestResponse>(configure => configure
                .UsingRestSharpHttpClient()
                .UsingNewtonsoftJsonSerializer());

            var client = serviceCollection.BuildServiceProvider().GetService<INClientFactory>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddCustomNClientFactory_WithLogging_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();

            serviceCollection.AddCustomNClientFactory<IRestRequest, IRestResponse>(configure => configure
                .UsingRestSharpHttpClient()
                .UsingNewtonsoftJsonSerializer());

            var client = serviceCollection.BuildServiceProvider().GetService<INClientFactory>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddCustomNClientFactory_BuilderWithCustomSettings_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddHttpClient().AddLogging();

            serviceCollection.AddCustomNClientFactory<IRestRequest, IRestResponse>(configure => configure
                .UsingRestSharpHttpClient()
                .UsingNewtonsoftJsonSerializer()
                .WithForcePollyResilience(Policy.NoOpAsync<ResponseContext<IRestRequest, IRestResponse>>()));

            var client = serviceCollection.BuildServiceProvider().GetService<INClientFactory>();
            client.Should().NotBeNull();
        }
    }
}
