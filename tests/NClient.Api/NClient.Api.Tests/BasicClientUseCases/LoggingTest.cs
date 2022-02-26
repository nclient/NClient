using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.Api.Tests.Stubs;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Helpers;
using NUnit.Framework;

namespace NClient.Api.Tests.BasicClientUseCases
{
    [Parallelizable]
    public class LoggingTest
    {
        [Test]
        public async Task NClientBuilder_WithGenericLogger_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var logger = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider()
                .GetRequiredService<ILogger<IBasicClient>>();
            var client = NClientGallery.Clients.GetRest().For<IBasicClientWithMetadata>(api.Urls.First().ToUri())
                .WithLogging(logger)
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
        
        [Test]
        public async Task NClientBuilder_WithLoggerFactory_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var loggerFactory = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider()
                .GetRequiredService<ILoggerFactory>();
            var client = NClientGallery.Clients.GetRest().For<IBasicClientWithMetadata>(api.Urls.First().ToUri())
                .WithLogging(loggerFactory)
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }

        [Test]
        public async Task NClientBuilder_WithCustomLogger_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var customLogger = new CustomLogger();
            var client = NClientGallery.Clients.GetRest().For<IBasicClientWithMetadata>(api.Urls.First().ToUri())
                .WithLogging(customLogger)
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
        
        [Test]
        public async Task NClientBuilder_WithMultipleCustomLoggers_NotThrow()
        {
            const int id = 1;
            using var api = BasicApiMockFactory.MockGetMethod(id);
            var customLogger = new CustomLogger();
            var client = NClientGallery.Clients.GetRest().For<IBasicClientWithMetadata>(api.Urls.First().ToUri())
                .WithLogging(customLogger)
                .WithLogging(customLogger)
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
    }
}
