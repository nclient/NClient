using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NClient.Exceptions;
using NClient.Extensions.DependencyInjection.Tests.Helpers;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NUnit.Framework;

namespace NClient.Extensions.DependencyInjection.Tests
{
    [Parallelizable]
    public class AddRestNClientFactoryExtensionsTest
    {
        [Test]
        public async Task AddRestNClientFactory_WithoutHttpClientAndLogging_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var serviceCollection = new ServiceCollection();
            
            serviceCollection.AddRestNClientFactory(factoryName: nameof(IBasicClientWithMetadata));

            var clientFactory = serviceCollection.BuildServiceProvider().GetService<INClientFactory>();
            clientFactory.Should().NotBeNull();
            var client = clientFactory!.Create<IBasicClientWithMetadata>(host: api.Urls.First());
            client.Should().NotBeNull();
            (await client.GetAsync(id)).Should().Be(id);
        }
        
        [Test]
        public async Task AddRestNClientFactory_WithHostProvider_UseHostFromServices()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var serviceCollection = new ServiceCollection().AddSingleton(nameof(IBasicClientWithMetadata));
            
            serviceCollection.AddRestNClientFactory(implementationFactory: (serviceProvider, builder) => builder
                .For(factoryName: serviceProvider.GetRequiredService<string>())
                .Build());

            var clientFactory = serviceCollection.BuildServiceProvider().GetService<INClientFactory>();
            clientFactory.Should().NotBeNull();
            var client = clientFactory!.Create<IBasicClientWithMetadata>(host: api.Urls.First());
            client.Should().NotBeNull();
            (await client.GetAsync(id)).Should().Be(id);
        }

        [Test]
        public async Task AddRestNClientFactory_WithLogging_UseLoggerFromServices()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var loggerMock = LoggerMockFactory.Create<IBasicClientWithMetadata>();
            var loggerFactoryMock = LoggerFactoryMockFactory.Create(loggerMock);
            var serviceCollection = new ServiceCollection().AddSingleton(loggerFactoryMock.Object);

            serviceCollection.AddRestNClientFactory(factoryName: nameof(IBasicClientWithMetadata));

            var clientFactory = serviceCollection.BuildServiceProvider().GetService<INClientFactory>();
            clientFactory.Should().NotBeNull();
            var client = clientFactory!.Create<IBasicClientWithMetadata>(host: api.Urls.First());
            (await client.GetAsync(id)).Should().Be(id);
            loggerFactoryMock.VerifyCreateLogger<IBasicClientWithMetadata>(Times.Once());
            loggerMock.VerifyLog(Times.AtLeast(1));
        }
        
        #if !NETFRAMEWORK
        [Test]
        public async Task AddRestNClientFactory_WithControllerJsonSerializerOptions_DoNotUseJsonOptions()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var constIntJsonConverter = new ConstIntJsonConverter();
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddControllers().AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.Converters.Add(constIntJsonConverter);
            });

            serviceCollection.AddRestNClientFactory(factoryName: nameof(IBasicClientWithMetadata));

            var clientFactory = serviceCollection.BuildServiceProvider().GetService<INClientFactory>();
            clientFactory.Should().NotBeNull();
            var client = clientFactory!.Create<IBasicClientWithMetadata>(host: api.Urls.First());
            client.Should().NotBeNull();
            (await client.GetAsync(id)).Should().Be(id);
        }
        #endif
        
        [Test]
        public async Task AddRestNClientFactory_WithJsonSerializerOptions_UseOptionsFormServices()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var constIntJsonConverter = new ConstIntJsonConverter();
            var serviceCollection = new ServiceCollection().Configure<JsonSerializerOptions>(x =>
            {
                x.Converters.Add(constIntJsonConverter);
            });

            serviceCollection.AddRestNClientFactory(factoryName: nameof(IBasicClientWithMetadata));

            var clientFactory = serviceCollection.BuildServiceProvider().GetService<INClientFactory>();
            clientFactory.Should().NotBeNull();
            var client = clientFactory!.Create<IBasicClientWithMetadata>(host: api.Urls.First());
            (await client.GetAsync(id)).Should().Be(constIntJsonConverter.Value);
        }
        
        [Test]
        public async Task AddRestNClientFactory_ImplementationFactory_UseImplementationFactory()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddRestNClientFactory(implementationFactory: builder =>
                builder.For(factoryName: nameof(IBasicClientWithMetadata))
                    .WithoutResponseValidation()
                    .WithResponseValidation(
                        isSuccess: _ => false,
                        onFailure: _ => throw new InvalidOperationException())
                    .Build());

            var clientFactory = serviceCollection.BuildServiceProvider().GetService<INClientFactory>();
            clientFactory.Should().NotBeNull();
            var client = clientFactory!.Create<IBasicClientWithMetadata>(host: api.Urls.First());
            client.Should().NotBeNull();
            await client.Invoking(x => x!.GetAsync(id))
                .Should()
                .ThrowExactlyAsync<ClientRequestException>()
                .WithInnerException<ClientRequestException, InvalidOperationException>();
        }
        
        [Test]
        public async Task AddRestNClientFactory_OptionsWithResponseValidation_UseOptions()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var serviceCollection = new ServiceCollection().Configure<NClientFactoryBuilderOptions<HttpRequestMessage, HttpResponseMessage>>(x =>
            {
                x.BuilderActions.Add(builder => builder
                    .WithoutResponseValidation()
                    .WithResponseValidation(
                        isSuccess: _ => false,
                        onFailure: _ => throw new InvalidOperationException()));
            });

            serviceCollection.AddRestNClientFactory(factoryName: nameof(IBasicClientWithMetadata));

            var clientFactory = serviceCollection.BuildServiceProvider().GetService<INClientFactory>();
            clientFactory.Should().NotBeNull();
            var client = clientFactory!.Create<IBasicClientWithMetadata>(host: api.Urls.First());
            client.Should().NotBeNull();
            await client.Invoking(x => x!.GetAsync(id))
                .Should()
                .ThrowExactlyAsync<ClientRequestException>()
                .WithInnerException<ClientRequestException, InvalidOperationException>();
        }
    }
}
