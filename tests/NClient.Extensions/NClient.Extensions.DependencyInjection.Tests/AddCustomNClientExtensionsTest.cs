using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NClient.Exceptions;
using NClient.Extensions.DependencyInjection.Tests.Helpers;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Helpers;
using NUnit.Framework;

namespace NClient.Extensions.DependencyInjection.Tests
{
    [Parallelizable]
    public class AddCustomNClientExtensionsTest
    {
        [Test]
        public async Task AddCustomNClient_WithoutHttpClientAndLogging_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var serviceCollection = new ServiceCollection();
            
            serviceCollection.AddCustomNClient<IBasicClientWithMetadata>(
                host: api.Urls.First().ToUri(),
                implementationFactory: builder => builder
                    .UsingRestApi()
                    .UsingHttpTransport()
                    .UsingJsonSerializer()
                    .Build());

            var client = serviceCollection.BuildServiceProvider().GetService<IBasicClientWithMetadata>();
            client.Should().NotBeNull();
            (await client!.GetAsync(id)).Should().Be(id);
        }
        
        [Test]
        public async Task AddCustomNClient_WithHostProvider_UseHostFromServices()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var serviceCollection = new ServiceCollection().AddSingleton(api.Urls.First().ToUri());
            
            serviceCollection.AddCustomNClient(implementationFactory: (serviceProvider, builder) => builder
                .For<IBasicClientWithMetadata>(host: serviceProvider.GetRequiredService<Uri>())
                .UsingRestApi()
                .UsingHttpTransport()
                .UsingJsonSerializer()
                .Build());

            var client = serviceCollection.BuildServiceProvider().GetService<IBasicClientWithMetadata>();
            client.Should().NotBeNull();
            (await client!.GetAsync(id)).Should().Be(id);
        }

        [Test]
        public async Task AddCustomNClient_WithLogging_UseLoggerFromServices()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var loggerMock = LoggerMockFactory.Create<IBasicClientWithMetadata>();
            var loggerFactoryMock = LoggerFactoryMockFactory.Create(loggerMock);
            var serviceCollection = new ServiceCollection().AddSingleton(loggerFactoryMock.Object);

            serviceCollection.AddCustomNClient<IBasicClientWithMetadata>(
                host: api.Urls.First().ToUri(),
                implementationFactory: (serviceProvider, builder) => builder
                    .UsingRestApi()
                    .UsingHttpTransport()
                    .UsingJsonSerializer()
                    .WithLogging(serviceProvider.GetRequiredService<ILoggerFactory>())
                    .Build());

            var client = serviceCollection.BuildServiceProvider().GetService<IBasicClientWithMetadata>();
            client.Should().NotBeNull();
            (await client!.GetAsync(id)).Should().Be(id);
            loggerFactoryMock.VerifyCreateLogger<IBasicClientWithMetadata>(Times.Once());
            loggerMock.VerifyLog(Times.AtLeast(1));
        }
        
        #if !NETFRAMEWORK
        [Test]
        public async Task AddCustomNClient_WithControllerJsonSerializerOptions_DoNotUseJsonOptions()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var constIntJsonConverter = new ConstIntJsonConverter();
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddControllers().AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.Converters.Add(constIntJsonConverter);
            });

            serviceCollection.AddCustomNClient<IBasicClientWithMetadata>(
                host: api.Urls.First().ToUri(),
                implementationFactory: builder => builder
                    .UsingRestApi()
                    .UsingHttpTransport()
                    .UsingJsonSerializer()
                    .Build());

            var client = serviceCollection.BuildServiceProvider().GetService<IBasicClientWithMetadata>();
            client.Should().NotBeNull();
            (await client!.GetAsync(id)).Should().Be(id);
        }
        #endif
        
        [Test]
        public async Task AddCustomNClient_WithJsonSerializerOptions_UseOptionsFormServices()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var constIntJsonConverter = new ConstIntJsonConverter();
            var serviceCollection = new ServiceCollection().Configure<JsonSerializerOptions>(x =>
            {
                x.Converters.Add(constIntJsonConverter);
            });

            serviceCollection.AddCustomNClient<IBasicClientWithMetadata>(
                host: api.Urls.First().ToUri(),
                implementationFactory: (serviceProvider, builder) => builder
                    .UsingRestApi()
                    .UsingHttpTransport()
                    .UsingSystemTextJsonSerialization(serviceProvider.GetRequiredService<IOptions<JsonSerializerOptions>>().Value)
                    .Build());

            var client = serviceCollection.BuildServiceProvider().GetService<IBasicClientWithMetadata>();
            client.Should().NotBeNull();
            (await client!.GetAsync(id)).Should().Be(constIntJsonConverter.Value);
        }
        
        [Test]
        public async Task AddCustomNClient_ImplementationFactory_UseImplementationFactory()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var host = api.Urls.First().ToUri();
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddCustomNClient(implementationFactory: builder =>
                builder.For<IBasicClientWithMetadata>(host)
                    .UsingRestApi()
                    .UsingHttpTransport()
                    .UsingJsonSerializer()
                    .WithoutResponseValidation()
                    .WithResponseValidation(
                        isSuccess: _ => false,
                        onFailure: _ => throw new InvalidOperationException())
                    .Build());

            var client = serviceCollection.BuildServiceProvider().GetService<IBasicClientWithMetadata>();
            client.Should().NotBeNull();
            await client.Invoking(x => x!.GetAsync(id))
                .Should()
                .ThrowExactlyAsync<ClientRequestException>()
                .WithInnerException<ClientRequestException, InvalidOperationException>();
        }
    }
}
