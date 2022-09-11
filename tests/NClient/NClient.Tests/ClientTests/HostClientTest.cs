using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NClient.Exceptions;
using NClient.Providers;
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
                .For<IHostClient>()
                .WithHost(doubleApiUri)
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
                .For<IHostClient>()
                .WithHost(new Uri(doubleApiUri))
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
                .For<IHostClient>()
                .WithHost(new Host(new Uri(doubleApiUri)))
                .Build()
                .GetAsync(id);

            result.Should().Be(2 * id);
        }
        
        [Test]
        public void WithoutAnyHostOption_ShouldThrow()
        {
            var client = NClientGallery.Clients.GetRest()
                .For<IHostClient>();

            client.Invoking(x => x.Build())
                .Should()
                .ThrowExactly<ClientBuildException>();
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
                .For<IHostClient>()
                .WithHost(hostMock.Object)
                .Build();

            var doubleResult = await client.GetAsync(id);
            var squareResult = await client.GetAsync(id);

            doubleResult.Should().Be(id * 2);
            squareResult.Should().Be(id * id);
        }
    }
}
