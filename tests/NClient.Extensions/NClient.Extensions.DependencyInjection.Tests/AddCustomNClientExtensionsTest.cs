using System.Net.Http;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NClient.Extensions.DependencyInjection.Tests.Helpers;
using NClient.Providers.Api.Rest.Extensions;
using NClient.Providers.Transport;
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

            serviceCollection.AddCustomNClient(builder => builder
                .For<ITestClientWithMetadata>(host: "http://localhost:5000")
                .UsingRestApi()
                .UsingRestSharpTransport()
                .UsingNewtonsoftJsonSerialization()
                .Build());

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClientWithMetadata>();
            client.Should().NotBeNull();
        }
        
        [Test]
        public void AddCustomNClient_WithHostProvider_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddSingleton<string>("http://localhost:5000");

            serviceCollection.AddCustomNClient((serviceProvider, builder) => builder
                .For<ITestClientWithMetadata>(serviceProvider.GetRequiredService<string>())
                .UsingRestApi()
                .UsingRestSharpTransport()
                .UsingNewtonsoftJsonSerialization()
                .Build());

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClientWithMetadata>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddCustomNClient_WithLogging_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();

            serviceCollection.AddCustomNClient(builder => builder
                .For<ITestClientWithMetadata>(host: "http://localhost:5000")
                .UsingRestApi()
                .UsingRestSharpTransport()
                .UsingNewtonsoftJsonSerialization()
                .Build());

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClientWithMetadata>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddCustomNClient_BuilderWithCustomSettings_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();

            serviceCollection.AddCustomNClient(builder => builder
                .For<ITestClientWithMetadata>(host: "http://localhost:5000")
                .UsingRestApi()
                .UsingRestSharpTransport()
                .UsingNewtonsoftJsonSerialization()
                .WithPollyFullResilience(Policy.NoOpAsync<IResponseContext<IRestRequest, IRestResponse>>())
                .Build());

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClientWithMetadata>();
            client.Should().NotBeNull();
        }
        
        [Test]
        public void AddCustomNClient_SingleClient_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();
            serviceCollection.AddHttpClient("TestClient");

            serviceCollection.AddCustomNClient((serviceProvider, builder) => builder
                .For<ITestClientWithMetadata>(host: "http://localhost:5000")
                .UsingRestApi()
                .UsingSystemNetHttpTransport(
                    httpClientFactory: serviceProvider.GetRequiredService<IHttpClientFactory>(),
                    httpClientName: "TestClient")
                .UsingJsonSerializer()
                .Build());

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClientWithMetadata>();
            client.Should().NotBeNull();
        }
    }
}
