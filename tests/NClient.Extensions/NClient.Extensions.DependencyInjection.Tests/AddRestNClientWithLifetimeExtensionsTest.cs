using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NClient.Extensions.DependencyInjection.Tests.Helpers;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NUnit.Framework;

namespace NClient.Extensions.DependencyInjection.Tests
{
    [Parallelizable]
    public class AddRestNClientWithLifetimeExtensionsTest
    {
        [Test]
        public void AddRestNClient_WithLifetime_NotThrow([Values] ServiceLifetime serviceLifetime)
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var serviceCollection = new ServiceCollection();
            
            serviceCollection.AddRestNClient<IBasicClientWithMetadata>(host: api.Urls.First(), serviceLifetime);
            
            serviceCollection.ServiceRegistered<IBasicClientWithMetadata>(serviceLifetime).Should().BeTrue();
        }
    }
}
