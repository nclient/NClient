using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NClient.Abstractions;
using NClient.Abstractions.HttpClients;
using NClient.Extensions.DependencyInjection.Tests.Helpers;
using NUnit.Framework;
using Polly;

namespace NClient.Extensions.DependencyInjection.Tests
{
    [Parallelizable]
    public class AddNClientFactoryExtensionsTest
    {
        [Test]
        public void AddNClientFactory_ClientFactoryBuilder_NotBeNull()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddNClientFactory(builder => builder);

            var nclientFactory = serviceCollection.BuildServiceProvider()
                .GetRequiredService<INClientFactory>();
            var client = nclientFactory.Create<ITestClient>(host: "http://localhost:5000");
            client.Should().NotBeNull();
        }

        [Test]
        public void AddNClientFactory_JsonSerializerOptionsAndAsyncPolicy_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddHttpClient().AddLogging();

            serviceCollection.AddNClientFactory(
                new JsonSerializerOptions(),
                Policy.NoOpAsync<HttpResponse>());

            var nclientFactory = serviceCollection.BuildServiceProvider()
                .GetRequiredService<INClientFactory>();
            var client = nclientFactory.Create<ITestClient>(host: "http://localhost:5000");
            client.Should().NotBeNull();
        }

        [Test]
        public void AddNClientFactory_JsonSerializerOptions_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddHttpClient().AddLogging();

            serviceCollection.AddNClientFactory(new JsonSerializerOptions());

            var nclientFactory = serviceCollection.BuildServiceProvider()
                .GetRequiredService<INClientFactory>();
            var client = nclientFactory.Create<ITestClient>(host: "http://localhost:5000");
            client.Should().NotBeNull();
        }

        [Test]
        public void AddNClientFactory_AsyncPolicy_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddHttpClient().AddLogging();

            serviceCollection.AddNClientFactory(Policy.NoOpAsync<HttpResponse>());

            var nclientFactory = serviceCollection.BuildServiceProvider()
                .GetRequiredService<INClientFactory>();
            var client = nclientFactory.Create<ITestClient>(host: "http://localhost:5000");
            client.Should().NotBeNull();
        }

        [Test]
        public void AddNClientFactory_OnlyHost_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddHttpClient().AddLogging();

            serviceCollection.AddNClientFactory();

            var nclientFactory = serviceCollection.BuildServiceProvider()
                .GetRequiredService<INClientFactory>();
            var client = nclientFactory.Create<ITestClient>(host: "http://localhost:5000");
            client.Should().NotBeNull();
        }
    }
}