using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using NClient.Standalone.Tests.Clients;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Entities;
using NClient.Testing.Common.Helpers;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class ResultClientTest
    {
        private IResultClientWithMetadata _resultClient = null!;
        private ResultApiMockFactory _resultApiMockFactory = null!;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _resultApiMockFactory = new ResultApiMockFactory(PortsPool.Get());
        }
        
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            PortsPool.Put(_resultApiMockFactory.ApiUri.Port);
        }
        
        [SetUp]
        public void Setup()
        {
            _resultClient = NClientGallery.Clients
                .GetBasic()
                .For<IResultClientWithMetadata>(_resultApiMockFactory.ApiUri.ToString())
                .Build();
        }

        [Test]
        public async Task BasicClient_GetIResultWithIntAsync_IResultWithInt()
        {
            const int id = 1;
            using var api = _resultApiMockFactory.MockGetIntMethod(id);

            var result = await _resultClient.GetIResultWithIntAsync(id);

            using var assertionScope = new AssertionScope();
            result.Value.Should().Be(id);
            result.Error.Should().Be(null);
        }
        
        [Test]
        public async Task BasicClient_GetIResultWithNotFoundIntAsync_IResultWithError()
        {
            const int id = 1;
            var expectedError = new Error { Message = "Error" };
            using var api = _resultApiMockFactory.MockGetNotFoundIntMethod(id);

            var result = await _resultClient.GetIResultWithIntAsync(id);

            using var assertionScope = new AssertionScope();
            result.Value.Should().Be(null);
            result.Error.Should().BeEquivalentTo(expectedError);
        }
        
        [Test]
        public async Task BasicClient_GetIResultWithEntityAsync_IResultWithEntity()
        {
            const int id = 1;
            var expectedEntity = new BasicEntity { Id = id, Value = 2 };
            using var api = _resultApiMockFactory.MockGetEntityMethod(id);

            var result = await _resultClient.GetIResultWithEntityAsync(id);

            using var assertionScope = new AssertionScope();
            result.Value.Should().BeEquivalentTo(expectedEntity);
            result.Error.Should().Be(null);
        }
        
        [Test]
        public async Task BasicClient_GetIResultWithNotFoundEntityAsync_IResultWithError()
        {
            const int id = 1;
            var expectedError = new Error { Message = "Error" };
            using var api = _resultApiMockFactory.MockGetNotFoundEntityMethod(id);

            var result = await _resultClient.GetResultWithEntityAsync(id);

            using var assertionScope = new AssertionScope();
            result.Value.Should().Be(null);
            result.Error.Should().BeEquivalentTo(expectedError);
        }
        
        [Test]
        public async Task BasicClient_GetResultWithIntAsync_ResultWithInt()
        {
            const int id = 1;
            using var api = _resultApiMockFactory.MockGetIntMethod(id);

            var result = await _resultClient.GetResultWithIntAsync(id);

            using var assertionScope = new AssertionScope();
            result.Value.Should().Be(id);
            result.Error.Should().Be(null);
        }
        
        [Test]
        public async Task BasicClient_GetResultWithNotFoundIntAsync_ResultWithError()
        {
            const int id = 1;
            var expectedError = new Error { Message = "Error" };
            using var api = _resultApiMockFactory.MockGetNotFoundIntMethod(id);

            var result = await _resultClient.GetIResultWithIntAsync(id);

            using var assertionScope = new AssertionScope();
            result.Value.Should().Be(null);
            result.Error.Should().BeEquivalentTo(expectedError);
        }
        
        [Test]
        public async Task BasicClient_GetResultWithEntityAsync_ResultWithEntity()
        {
            const int id = 1;
            var expectedEntity = new BasicEntity { Id = id, Value = 2 };
            using var api = _resultApiMockFactory.MockGetEntityMethod(id);

            var result = await _resultClient.GetResultWithEntityAsync(id);

            using var assertionScope = new AssertionScope();
            result.Value.Should().BeEquivalentTo(expectedEntity);
            result.Error.Should().Be(null);
        }
        
        [Test]
        public async Task BasicClient_GetResultWithNotFoundEntityAsync_ResultWithError()
        {
            const int id = 1;
            var expectedError = new Error { Message = "Error" };
            using var api = _resultApiMockFactory.MockGetNotFoundEntityMethod(id);

            var result = await _resultClient.GetResultWithEntityAsync(id);

            using var assertionScope = new AssertionScope();
            result.Value.Should().Be(null);
            result.Error.Should().BeEquivalentTo(expectedError);
        }
    }
}
