﻿using System.Net.Http;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NClient.Abstractions.Resilience;
using NClient.Extensions.DependencyInjection.Tests.Helpers;
using NClient.Providers.HttpClient.RestSharp;
using NClient.Providers.HttpClient.System;
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

            serviceCollection.AddCustomNClient<ITestClientWithMetadata>(host: "http://localhost:5000", configure => configure
                .UsingRestSharpHttpClient()
                .UsingNewtonsoftJsonSerializer()
                .Build());

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClientWithMetadata>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddCustomNClient_WithLogging_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();

            serviceCollection.AddCustomNClient<ITestClientWithMetadata>(host: "http://localhost:5000", configure => configure
                .UsingRestSharpHttpClient()
                .UsingNewtonsoftJsonSerializer()
                .Build());

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClientWithMetadata>();
            client.Should().NotBeNull();
        }

        [Test]
        public void AddCustomNClient_BuilderWithCustomSettings_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();

            serviceCollection.AddCustomNClient<ITestClientWithMetadata>(host: "http://localhost:5000", configure => configure
                .UsingRestSharpHttpClient()
                .UsingNewtonsoftJsonSerializer()
                .WithForcePollyResilience(Policy.NoOpAsync<ResponseContext<IRestRequest, IRestResponse>>())
                .Build());

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClientWithMetadata>();
            client.Should().NotBeNull();
        }
        
        [Test]
        public void AddCustomNClient_SingleClient_NotBeNull()
        {
            var serviceCollection = new ServiceCollection().AddLogging();
            serviceCollection.AddHttpClient("TestClient");

            serviceCollection.AddCustomNClient<ITestClientWithMetadata>(host: "http://localhost:5000", (serviceProvider, configure) => configure
                .UsingSystemHttpClient(
                    httpClientFactory: serviceProvider.GetRequiredService<IHttpClientFactory>(),
                    httpClientName: "TestClient")
                .UsingJsonSerializer()
                .Build());

            var client = serviceCollection.BuildServiceProvider().GetService<ITestClientWithMetadata>();
            client.Should().NotBeNull();
        }
    }
}