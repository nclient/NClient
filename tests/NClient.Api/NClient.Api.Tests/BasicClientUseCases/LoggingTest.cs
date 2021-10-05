using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.Abstractions.Builders;
using NClient.Api.Tests.Helpers;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NClient.Tests.Clients;
using NUnit.Framework;

namespace NClient.Api.Tests.BasicClientUseCases
{
    [Parallelizable]
    public class LoggingTest
    {
        private INClientOptionalBuilder<IBasicClientWithMetadata, HttpRequestMessage, HttpResponseMessage> _optionalBuilder = null!;
        private BasicApiMockFactory _api = null!;

        [SetUp]
        public void SetUp()
        {
            _api = new BasicApiMockFactory(5025);
            _optionalBuilder = NClientGallery.NativeClients.GetBasic().For<IBasicClientWithMetadata>(_api.ApiUri.ToString());
        }
        
        [Test]
        public async Task NClientBuilder_WithGenericLogger_NotThrow()
        {
            const int id = 1;
            using var api = _api.MockGetMethod(id);
            var logger = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider()
                .GetRequiredService<ILogger<IBasicClient>>();
            var client = _optionalBuilder
                .WithLogging(logger)
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
        
        [Test]
        public async Task NClientBuilder_WithLoggerFactory_NotThrow()
        {
            const int id = 1;
            using var api = _api.MockGetMethod(id);
            var loggerFactory = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider()
                .GetRequiredService<ILoggerFactory>();
            var client = _optionalBuilder
                .WithLogging(loggerFactory)
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }

        [Test]
        public async Task NClientBuilder_WithCustomLogger_NotThrow()
        {
            const int id = 1;
            using var api = _api.MockGetMethod(id);
            var customLogger = new CustomLogger();
            var client = _optionalBuilder
                .WithLogging(customLogger)
                .Build();
            
            var response = await client.GetAsync(id);

            response.Should().Be(id);
        }
    }
}
