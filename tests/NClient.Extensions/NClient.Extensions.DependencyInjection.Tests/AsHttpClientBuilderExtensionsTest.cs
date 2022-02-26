using System;
using System.Linq;
using System.Net.Http;
using NClient.Core.Helpers;
using NClient.Testing.Common.Helpers;
using NUnit.Framework;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NClient.Testing.Common.Clients;

namespace NClient.Extensions.DependencyInjection.Tests
{
    [Parallelizable]
    public class AsHttpClientBuilderExtensionsTest
    {
        [Test]
        public void AsHttpClientBuilder_WithDefaultRequestHeaders_NotThrow()
        {
            var serviceCollection = new ServiceCollection();
            var guidProvideMock = new Mock<IGuidProvider>();
            guidProvideMock.Setup(x => x.Create()).Returns(Guid.Empty);
            AddRestNClientExtensions.GuidProvider = new Mock<IGuidProvider>().Object;

            serviceCollection.AddRestNClient<IBasicClientWithMetadata>(host: "http://localhost:5000".ToUri())
                .AsHttpClientBuilder()
                .ConfigureHttpClient(x => x.DefaultRequestHeaders.Add(name: "Name", value: "Value"));
            
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var client = serviceProvider.GetService<IBasicClientWithMetadata>();
            client.Should().NotBeNull();
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            httpClientFactory.Should().NotBeNull();
            var httpClient = httpClientFactory!.CreateClient(name: Guid.Empty.ToString());
            httpClient.DefaultRequestHeaders.Should().ContainSingle(x => x.Key == "Name" && x.Value.Single() == "Value");
        }
    }
}
