﻿using System.Net.Http;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NClient.Providers.Transport;
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

            serviceCollection.AddCustomNClientFactory(configure => configure
                .UsingRestApi()
                .UsingRestSharpTransport()
                .UsingNewtonsoftJsonSerialization()
                .Build());

            var client = serviceCollection.BuildServiceProvider().GetService<INClientFactory>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddCustomNClientFactory_WithLogging_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();

            serviceCollection.AddCustomNClientFactory(configure => configure
                .UsingRestApi()
                .UsingRestSharpTransport()
                .UsingNewtonsoftJsonSerialization()
                .Build());

            var client = serviceCollection.BuildServiceProvider().GetService<INClientFactory>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddCustomNClientFactory_BuilderWithCustomSettings_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddHttpClient().AddLogging();

            serviceCollection.AddCustomNClientFactory(configure => configure
                .UsingRestApi()
                .UsingRestSharpTransport()
                .UsingNewtonsoftJsonSerialization()
                .WithPollyFullResilience(Policy.NoOpAsync<IResponseContext<IRestRequest, IRestResponse>>())
                .Build());

            var client = serviceCollection.BuildServiceProvider().GetService<INClientFactory>();
            client.Should().NotBeNull();
        }
        
        [Test]
        public void AddCustomNClientFactory_SingleClient_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();
            serviceCollection.AddHttpClient("TestClient");

            serviceCollection.AddCustomNClientFactory((serviceProvider, configure) => configure
                .UsingRestApi()
                .UsingSystemNetHttpTransport(
                    httpClientFactory: serviceProvider.GetRequiredService<IHttpClientFactory>(),
                    httpClientName: "TestClient")
                .UsingJsonSerializer()
                .Build());

            var client = serviceCollection.BuildServiceProvider().GetService<INClientFactory>();
            client.Should().NotBeNull();
        }
    }
}
