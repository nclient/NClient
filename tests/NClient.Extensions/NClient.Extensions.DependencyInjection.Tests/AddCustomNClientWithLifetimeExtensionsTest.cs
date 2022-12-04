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
    public class AddCustomNClientWithLifetimeExtensionsTest
    {
        [Test]
        public void AddCustomNClient_WithLifetime_NotThrow([Values] ServiceLifetime serviceLifetime)
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var serviceCollection = new ServiceCollection();
            
            serviceCollection.AddCustomNClient<IBasicClientWithMetadata>(
                host: api.Urls.First(),
                implementationFactory: builder => builder
                    .UsingRestApi()
                    .UsingHttpTransport()
                    .UsingJsonSerializer()
                    .Build(),
                serviceLifetime);

            serviceCollection.ServiceRegistered<IBasicClientWithMetadata>(serviceLifetime).Should().BeTrue();
        }
        
        [Test]
        public void AddCustomNClientScoped_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var serviceCollection = new ServiceCollection();
            
            serviceCollection.AddCustomNClientScoped<IBasicClientWithMetadata>(
                host: api.Urls.First(),
                implementationFactory: builder => builder
                    .UsingRestApi()
                    .UsingHttpTransport()
                    .UsingJsonSerializer()
                    .Build());

            serviceCollection.ServiceRegistered<IBasicClientWithMetadata>(ServiceLifetime.Scoped).Should().BeTrue();
        }
        
        [Test]
        public void AddCustomNClientTransient_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var serviceCollection = new ServiceCollection();
            
            serviceCollection.AddCustomNClientTransient<IBasicClientWithMetadata>(
                host: api.Urls.First(),
                implementationFactory: builder => builder
                    .UsingRestApi()
                    .UsingHttpTransport()
                    .UsingJsonSerializer()
                    .Build());

            serviceCollection.ServiceRegistered<IBasicClientWithMetadata>(ServiceLifetime.Transient).Should().BeTrue();
        }
        
        [Test]
        public void AddCustomNClientSingleton_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var serviceCollection = new ServiceCollection();
            
            serviceCollection.AddCustomNClientSingleton<IBasicClientWithMetadata>(
                host: api.Urls.First(),
                implementationFactory: builder => builder
                    .UsingRestApi()
                    .UsingHttpTransport()
                    .UsingJsonSerializer()
                    .Build());

            serviceCollection.ServiceRegistered<IBasicClientWithMetadata>(ServiceLifetime.Singleton).Should().BeTrue();
        }
        
        [Test]
        public void AddCustomNClientScoped_WithHostProvider_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var serviceCollection = new ServiceCollection().AddSingleton(api.Urls.First().ToUri());
            
            serviceCollection.AddCustomNClientScoped(implementationFactory: (serviceProvider, builder) => builder
                .For<IBasicClientWithMetadata>(host: serviceProvider.GetRequiredService<Uri>())
                .UsingRestApi()
                .UsingHttpTransport()
                .UsingJsonSerializer()
                .Build());

            serviceCollection.ServiceRegistered<IBasicClientWithMetadata>(ServiceLifetime.Scoped).Should().BeTrue();
        }
        
        [Test]
        public void AddCustomNClientTransient_WithHostProvider_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var serviceCollection = new ServiceCollection().AddSingleton(api.Urls.First().ToUri());
            
            serviceCollection.AddCustomNClientTransient(implementationFactory: (serviceProvider, builder) => builder
                .For<IBasicClientWithMetadata>(host: serviceProvider.GetRequiredService<Uri>())
                .UsingRestApi()
                .UsingHttpTransport()
                .UsingJsonSerializer()
                .Build());

            serviceCollection.ServiceRegistered<IBasicClientWithMetadata>(ServiceLifetime.Transient).Should().BeTrue();
        }
        
        [Test]
        public void AddCustomNClientSingleton_WithHostProvider_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var serviceCollection = new ServiceCollection().AddSingleton(api.Urls.First().ToUri());
            
            serviceCollection.AddCustomNClientSingleton(implementationFactory: (serviceProvider, builder) => builder
                .For<IBasicClientWithMetadata>(host: serviceProvider.GetRequiredService<Uri>())
                .UsingRestApi()
                .UsingHttpTransport()
                .UsingJsonSerializer()
                .Build());

            serviceCollection.ServiceRegistered<IBasicClientWithMetadata>(ServiceLifetime.Singleton).Should().BeTrue();
        }
        
        [Test]
        public void AddCustomNClientScoped_ImplementationFactory_UseImplementationFactory()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var host = api.Urls.First();
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddCustomNClientScoped(implementationFactory: builder =>
                builder.For<IBasicClientWithMetadata>(host)
                    .UsingRestApi()
                    .UsingHttpTransport()
                    .UsingJsonSerializer()
                    .WithoutResponseValidation()
                    .WithResponseValidation(
                        isSuccess: _ => false,
                        onFailure: _ => throw new InvalidOperationException())
                    .Build());

            serviceCollection.ServiceRegistered<IBasicClientWithMetadata>(ServiceLifetime.Scoped).Should().BeTrue();
        }
        
        [Test]
        public void AddCustomNClientTransient_ImplementationFactory_UseImplementationFactory()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var host = api.Urls.First();
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddCustomNClientTransient(implementationFactory: builder =>
                builder.For<IBasicClientWithMetadata>(host)
                    .UsingRestApi()
                    .UsingHttpTransport()
                    .UsingJsonSerializer()
                    .WithoutResponseValidation()
                    .WithResponseValidation(
                        isSuccess: _ => false,
                        onFailure: _ => throw new InvalidOperationException())
                    .Build());

            serviceCollection.ServiceRegistered<IBasicClientWithMetadata>(ServiceLifetime.Transient).Should().BeTrue();
        }
        
        [Test]
        public void AddCustomNClientSingleton_ImplementationFactory_UseImplementationFactory()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var host = api.Urls.First();
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddCustomNClientSingleton(implementationFactory: builder =>
                builder.For<IBasicClientWithMetadata>(host)
                    .UsingRestApi()
                    .UsingHttpTransport()
                    .UsingJsonSerializer()
                    .WithoutResponseValidation()
                    .WithResponseValidation(
                        isSuccess: _ => false,
                        onFailure: _ => throw new InvalidOperationException())
                    .Build());

            serviceCollection.ServiceRegistered<IBasicClientWithMetadata>(ServiceLifetime.Singleton).Should().BeTrue();
        }
    }
}
