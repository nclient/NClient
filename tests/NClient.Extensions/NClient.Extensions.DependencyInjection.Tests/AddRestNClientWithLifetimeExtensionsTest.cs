using System;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NClient.Extensions.DependencyInjection.Tests.Helpers;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Helpers;
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
        
        [Test]
        public void AddRestNClientScoped_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddRestNClientScoped<IBasicClientWithMetadata>(host: api.Urls.First());
            
            serviceCollection.ServiceRegistered<IBasicClientWithMetadata>(ServiceLifetime.Scoped).Should().BeTrue();
        }
        
        [Test]
        public void AddRestNClientTransient_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddRestNClientTransient<IBasicClientWithMetadata>(host: api.Urls.First());
            
            serviceCollection.ServiceRegistered<IBasicClientWithMetadata>(ServiceLifetime.Transient).Should().BeTrue();
        }
        
        [Test]
        public void AddRestNClientSingleton_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddRestNClientSingleton<IBasicClientWithMetadata>(host: api.Urls.First());
            
            serviceCollection.ServiceRegistered<IBasicClientWithMetadata>(ServiceLifetime.Singleton).Should().BeTrue();
        }
        
        [Test]
        public void AddRestNClientScoped_WithHostProvider_UseHostFromServices()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var serviceCollection = new ServiceCollection().AddSingleton(api.Urls.First().ToUri());
            
            serviceCollection.AddRestNClientScoped(implementationFactory: (serviceProvider, builder) => builder
                .For<IBasicClientWithMetadata>(host: serviceProvider.GetRequiredService<Uri>())
                .Build());

            serviceCollection.ServiceRegistered<IBasicClientWithMetadata>(ServiceLifetime.Scoped).Should().BeTrue();
        }
        
        [Test]
        public void AddRestNClientTransient_WithHostProvider_UseHostFromServices()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var serviceCollection = new ServiceCollection().AddSingleton(api.Urls.First().ToUri());
            
            serviceCollection.AddRestNClientTransient(implementationFactory: (serviceProvider, builder) => builder
                .For<IBasicClientWithMetadata>(host: serviceProvider.GetRequiredService<Uri>())
                .Build());

            serviceCollection.ServiceRegistered<IBasicClientWithMetadata>(ServiceLifetime.Transient).Should().BeTrue();
        }
        
        [Test]
        public void AddRestNClientSingleton_WithHostProvider_UseHostFromServices()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var serviceCollection = new ServiceCollection().AddSingleton(api.Urls.First().ToUri());
            
            serviceCollection.AddRestNClientSingleton(implementationFactory: (serviceProvider, builder) => builder
                .For<IBasicClientWithMetadata>(host: serviceProvider.GetRequiredService<Uri>())
                .Build());

            serviceCollection.ServiceRegistered<IBasicClientWithMetadata>(ServiceLifetime.Singleton).Should().BeTrue();
        }

        [Test]
        public void AddRestNClientScoped_ImplementationFactory_UseImplementationFactory()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var host = api.Urls.First();
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddRestNClientScoped(implementationFactory: builder =>
                builder.For<IBasicClientWithMetadata>(host)
                    .WithoutResponseValidation()
                    .WithResponseValidation(
                        isSuccess: _ => false,
                        onFailure: _ => throw new InvalidOperationException())
                    .Build());

            serviceCollection.ServiceRegistered<IBasicClientWithMetadata>(ServiceLifetime.Scoped).Should().BeTrue();
        }

        [Test]
        public void AddRestNClientTransient_ImplementationFactory_UseImplementationFactory()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var host = api.Urls.First();
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddRestNClientTransient(implementationFactory: builder =>
                builder.For<IBasicClientWithMetadata>(host)
                    .WithoutResponseValidation()
                    .WithResponseValidation(
                        isSuccess: _ => false,
                        onFailure: _ => throw new InvalidOperationException())
                    .Build());

            serviceCollection.ServiceRegistered<IBasicClientWithMetadata>(ServiceLifetime.Transient).Should().BeTrue();
        }

        [Test]
        public void AddRestNClientSingleton_ImplementationFactory_UseImplementationFactory()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var host = api.Urls.First();
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddRestNClientSingleton(implementationFactory: builder =>
                builder.For<IBasicClientWithMetadata>(host)
                    .WithoutResponseValidation()
                    .WithResponseValidation(
                        isSuccess: _ => false,
                        onFailure: _ => throw new InvalidOperationException())
                    .Build());

            serviceCollection.ServiceRegistered<IBasicClientWithMetadata>(ServiceLifetime.Singleton).Should().BeTrue();
        }
    }
}
