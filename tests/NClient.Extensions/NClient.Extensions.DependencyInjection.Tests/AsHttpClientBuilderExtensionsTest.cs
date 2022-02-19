using System.Linq;
using System.Net.Http;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NClient.Extensions.DependencyInjection.Tests.Helpers;
using NUnit.Framework;

namespace NClient.Extensions.DependencyInjection.Tests
{
    [Parallelizable]
    public class AsHttpClientBuilderExtensionsTest
    {
        [Test]
        public void AsHttpClientBuilder_WithDefaultRequestHeaders_NotThrow()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddRestNClient<ITestClientWithMetadata>(host: "http://localhost:5000", clientName: "testClient")
                .AsHttpClientBuilder()
                .ConfigureHttpClient(x => x.DefaultRequestHeaders.Add(name: "Name", value: "Value"));
            
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var client = serviceProvider.GetService<ITestClientWithMetadata>();
            client.Should().NotBeNull();
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            httpClientFactory.Should().NotBeNull();
            var httpClient = httpClientFactory!.CreateClient("testClient");
            httpClient.DefaultRequestHeaders.Should().ContainSingle(x => x.Key == "Name" && x.Value.Single() == "Value");
        }
    }
}
