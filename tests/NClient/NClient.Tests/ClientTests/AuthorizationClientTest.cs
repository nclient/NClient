using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NClient.Exceptions;
using NClient.Providers.Authorization;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NClient.Tests.ClientTests.Helpers;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class AuthorizationClientTest : ClientTestBase<IAuthorizationClientWithMetadata>
    {
        private static readonly Token Token = new(scheme: "Bearer", value: "AbCdEf123456");
        
        [Test]
        public async Task BearerToken_WithSchemeAndTokenValue_NotThrow()
        {
            const int id = 1;
            using var api = AuthorizationApiMockFactory.MockGetMethodWithAuth(id, Token);

            var result = await NClientGallery.Clients.GetRest()
                .For<IAuthorizationClientWithMetadata>(host: api.Urls.First())
                .WithTokenAuthorization(Token.Scheme, Token.Value)
                .Build()
                .GetAsync(id);

            result.Should().Be(id);
        }
        
        [Test]
        public async Task BearerToken_WithToken_NotThrow()
        {
            const int id = 1;
            using var api = AuthorizationApiMockFactory.MockGetMethodWithAuth(id, Token);

            var result = await NClientGallery.Clients.GetRest()
                .For<IAuthorizationClientWithMetadata>(host: api.Urls.First())
                .WithTokenAuthorization(Token)
                .Build()
                .GetAsync(id);

            result.Should().Be(id);
        }
        
        [Test]
        public async Task BearerToken_WithTokenForUri_NotThrow()
        {
            const int id = 1;
            using var api = AuthorizationApiMockFactory.MockGetMethodWithAuth(id, Token);
            var host = api.Urls.First();
            var tokensMock = new Mock<ITokens>();
            tokensMock
                .Setup(x => x.TryGetToken(It.IsAny<Uri>()))
                .Returns<Uri>(x => x == new Uri(host) ? Token : null);
            
            var result = await NClientGallery.Clients.GetRest()
                .For<IAuthorizationClientWithMetadata>(host)
                .WithTokenAuthorization(tokensMock.Object)
                .Build()
                .GetAsync(id);

            result.Should().Be(id);
        }
        
        [Test]
        public async Task BearerToken_WithTokenForUnknownUri_ThrowClientRequestException()
        {
            const int id = 1;
            using var api = AuthorizationApiMockFactory.MockGetMethodWithAuth(id, Token);
            var tokensMock = new Mock<ITokens>();
            tokensMock
                .Setup(x => x.TryGetToken(It.IsAny<Uri>()))
                .Returns<Uri>(null);
            
            var client = NClientGallery.Clients.GetRest()
                .For<IAuthorizationClientWithMetadata>(host: api.Urls.First())
                .WithTokenAuthorization(tokensMock.Object)
                .Build();

            await client.Invoking(x => x.GetAsync(id))
                .Should()
                .ThrowExactlyAsync<ClientRequestException>();
        }
    }
}
