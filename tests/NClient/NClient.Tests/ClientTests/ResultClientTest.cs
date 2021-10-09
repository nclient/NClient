using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using NClient.Standalone.Tests.Clients;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class ResultClientTest
    {
        private IResultClientWithMetadata _resultClient = null!;
        private ResultApiMockFactory _resultApiMockFactory = null!;

        [SetUp]
        public void Setup()
        {
            _resultApiMockFactory = new ResultApiMockFactory(port: 5031);

            _resultClient = NClientGallery.NativeClients
                .GetBasic()
                .For<IResultClientWithMetadata>(_resultApiMockFactory.ApiUri.ToString())
                .Build();
        }

        [Test]
        public async Task BasicClient_GetIntAsync_ResultWithInt()
        {
            const int id = 1;
            using var api = _resultApiMockFactory.MockGetIntMethod(id);

            var result = await _resultClient.GetIntAsync(id);

            using var assertionScope = new AssertionScope();
            result.Value.Should().Be(id);
            result.Error.Should().Be(null);
        }
        
        [Test]
        public async Task BasicClient_GetNotFoundIntAsync_ResultWithError()
        {
            const int id = 1;
            var expectedError = new Error { Message = "Error" };
            using var api = _resultApiMockFactory.MockGetNotFoundIntMethod(id);

            var result = await _resultClient.GetIntAsync(id);

            using var assertionScope = new AssertionScope();
            result.Value.Should().Be(null);
            result.Error.Should().BeEquivalentTo(expectedError);
        }
        
        [Test]
        public async Task BasicClient_GetEntityAsync_ResultWithEntity()
        {
            const int id = 1;
            var expectedEntity = new BasicEntity { Id = id, Value = 2 };
            using var api = _resultApiMockFactory.MockGetEntityMethod(id);

            var result = await _resultClient.GetEntityAsync(id);

            using var assertionScope = new AssertionScope();
            result.Value.Should().BeEquivalentTo(expectedEntity);
            result.Error.Should().Be(null);
        }
        
        [Test]
        public async Task BasicClient_GetNotFoundEntityAsync_ResultWithError()
        {
            const int id = 1;
            var expectedError = new Error { Message = "Error" };
            using var api = _resultApiMockFactory.MockGetNotFoundEntityMethod(id);

            var result = await _resultClient.GetEntityAsync(id);

            using var assertionScope = new AssertionScope();
            result.Value.Should().Be(null);
            result.Error.Should().BeEquivalentTo(expectedError);
        }
    }
}
