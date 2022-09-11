using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NClient.Exceptions;
using NClient.Providers.Host;
using NClient.Standalone.Client.Host;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NClient.Tests.ClientTests.Helpers;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class HostClientTest : ClientTestBase<IHostClient>
    {
        [Test]
        public async Task HostStringParameter_NotThrow()
        {
            const int id = 1;
            using var doubleApiServer = HostApiMockFactory.MockGetMethodDoubleServer(id);

            var result = await NClientGallery.Clients.GetRest()
                .For<IHostClient>(host: doubleApiServer.Urls.First())
                .Build()
                .GetAsync(id);

            result.Should().Be(2 * id);
        }
        
        [Test]
        public async Task WithStringParameter_NotThrow()
        {
            const int id = 1;
            using var doubleApiServer = HostApiMockFactory.MockGetMethodDoubleServer(id);

            var doubleApiUri = doubleApiServer.Urls.First();
            
            var result = await NClientGallery.Clients.GetRest()
                .For<IHostClient>(doubleApiUri)
                .Build()
                .GetAsync(id);

            result.Should().Be(2 * id);
        }
        
        [Test]
        public async Task WithUriParameter_NotThrow()
        {
            const int id = 1;
            using var doubleApiServer = HostApiMockFactory.MockGetMethodDoubleServer(id);

            var doubleApiUri = doubleApiServer.Urls.First();
            
            var result = await NClientGallery.Clients.GetRest()
                .For<IHostClient>(new Uri(doubleApiUri))
                .Build()
                .GetAsync(id);

            result.Should().Be(2 * id);
        }
        
        [Test]
        public async Task WithHostParameter_NotThrow()
        {
            const int id = 1;
            using var doubleApiServer = HostApiMockFactory.MockGetMethodDoubleServer(id);

            var doubleApiUri = doubleApiServer.Urls.First();
            
            var result = await NClientGallery.Clients.GetRest()
                .For<IHostClient>(new Host(new Uri(doubleApiUri)))
                .Build()
                .GetAsync(id);

            result.Should().Be(2 * id);
        }
        
        [Test]
        public async Task CustomClient_WithHostParameter_NotThrow()
        {
            const int id = 1;
            using var doubleApiServer = HostApiMockFactory.MockGetMethodDoubleServer(id);

            var doubleApiUri = doubleApiServer.Urls.First();
            
            var result = await NClientGallery.Clients.GetCustom()
                .For<IHostClient>(new Host(new Uri(doubleApiUri)))
                .UsingRestApi()
                .UsingSystemNetHttpTransport()
                .UsingJsonSerializer()
                .WithAdvancedResponseValidation(x => x
                    .ForTransport().UseSystemNetHttpResponseValidation())
                .Build()
                .GetAsync(id);

            result.Should().Be(2 * id);
        }
        
        [Test]
        public async Task WithEmptyUri_ShouldThrow()
        {
            const int id = 1;
            
            var hostMock = new Mock<IHost>();
            hostMock.Setup(m => m.TryGetUriAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(default(Uri));
            
            var client = NClientGallery.Clients.GetRest()
                .For<IHostClient>(hostMock.Object)
                .Build();

            await client.Invoking(x => x.GetAsync(id))
                .Should()
                .ThrowExactlyAsync<ClientValidationException>();
        }
        
        [Test]
        public async Task ChangeHostAtRuntime_NotThrow()
        {
            const int id = 1;
            using var doubleApiServer = HostApiMockFactory.MockGetMethodDoubleServer(id);
            using var squareApiServer = HostApiMockFactory.MockGetMethodSquareServer(id);

            var doubleApiUri = doubleApiServer.Urls.First();
            var squareApiUri = squareApiServer.Urls.First();

            var hostMock = new Mock<IHost>();
            hostMock.SetupSequence(m => m.TryGetUriAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Uri(doubleApiUri))
                .ReturnsAsync(new Uri(squareApiUri));

            var client = NClientGallery.Clients.GetRest()
                .For<IHostClient>(hostMock.Object)
                .Build();

            var doubleResult = await client.GetAsync(id);
            var squareResult = await client.GetAsync(id);

            doubleResult.Should().Be(id * 2);
            squareResult.Should().Be(id * id);
        }
    }
}
