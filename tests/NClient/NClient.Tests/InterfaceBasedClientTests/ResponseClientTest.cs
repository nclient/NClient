using System.Threading.Tasks;
using FluentAssertions;
using NClient.Extensions;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Entities;
using NClient.Tests.Clients;
using NUnit.Framework;

namespace NClient.Tests.InterfaceBasedClientTests
{
    [Parallelizable]
    public class ResponseClientTest
    {
        private IResponseClientWithMetadata _responseClient = null!;
        private ResponseApiMockFactory _responseApiMockFactory = null!;

        [SetUp]
        public void Setup()
        {
            _responseApiMockFactory = new ResponseApiMockFactory(port: 5017);

            _responseClient = new NClientBuilder()
                .Use<IResponseClientWithMetadata>(_responseApiMockFactory.ApiUri.ToString())
                .Build();
        }

        [Test]
        public async Task ResponseClient_GetAsync_IntInBody()
        {
            const int id = 1;
            using var api = _responseApiMockFactory.MockGetMethod(id);

            var result = await _responseClient.GetAsync(id);
            result.Should().Be(id);
        }

        [Test]
        public async Task ResponseClient_PostAsync_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _responseApiMockFactory.MockPostMethod(entity);

            await _responseClient
                .Invoking(async x => await x.PostAsync(entity))
                .Should()
                .NotThrowAsync();
        }
    }
}
