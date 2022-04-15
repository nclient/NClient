using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;
using NClient.Tests.ClientTests.Helpers;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class ResultClientTest : ClientTestBase<IResultClientWithMetadata>
    {
        [Test]
        public async Task BasicClient_GetIResultWithIntAsync_IResultWithInt()
        {
            const int id = 1;
            using var api = ResultApiMockFactory.MockGetIntMethod(id);

            var result = await NClientGallery.Clients.GetRest().For<IResultClientWithMetadata>(host: api.Urls.First()).Build()
                .GetIResultWithIntAsync(id);

            using var assertionScope = new AssertionScope();
            result.Value.Should().Be(id);
            result.Error.Should().Be(null);
        }
        
        [Test]
        public async Task BasicClient_GetIResultWithNotFoundIntAsync_IResultWithError()
        {
            const int id = 1;
            var expectedError = new Error { Message = "Error" };
            using var api = ResultApiMockFactory.MockGetNotFoundIntMethod(id);

            var result = await NClientGallery.Clients.GetRest().For<IResultClientWithMetadata>(host: api.Urls.First()).Build()
                .GetIResultWithIntAsync(id);

            using var assertionScope = new AssertionScope();
            result.Value.Should().Be(null);
            result.Error.Should().BeEquivalentTo(expectedError);
        }
        
        [Test]
        public async Task BasicClient_GetIResultWithEntityAsync_IResultWithEntity()
        {
            const int id = 1;
            var expectedEntity = new BasicEntity { Id = id, Value = 2 };
            using var api = ResultApiMockFactory.MockGetEntityMethod(id);

            var result = await NClientGallery.Clients.GetRest().For<IResultClientWithMetadata>(host: api.Urls.First()).Build()
                .GetIResultWithEntityAsync(id);

            using var assertionScope = new AssertionScope();
            result.Value.Should().BeEquivalentTo(expectedEntity);
            result.Error.Should().Be(null);
        }
        
        [Test]
        public async Task BasicClient_GetIResultWithNotFoundEntityAsync_IResultWithError()
        {
            const int id = 1;
            var expectedError = new Error { Message = "Error" };
            using var api = ResultApiMockFactory.MockGetNotFoundEntityMethod(id);

            var result = await NClientGallery.Clients.GetRest().For<IResultClientWithMetadata>(host: api.Urls.First()).Build()
                .GetResultWithEntityAsync(id);

            using var assertionScope = new AssertionScope();
            result.Value.Should().Be(null);
            result.Error.Should().BeEquivalentTo(expectedError);
        }
        
        [Test]
        public async Task BasicClient_GetResultWithIntAsync_ResultWithInt()
        {
            const int id = 1;
            using var api = ResultApiMockFactory.MockGetIntMethod(id);

            var result = await NClientGallery.Clients.GetRest().For<IResultClientWithMetadata>(host: api.Urls.First()).Build()
                .GetResultWithIntAsync(id);

            using var assertionScope = new AssertionScope();
            result.Value.Should().Be(id);
            result.Error.Should().Be(null);
        }
        
        [Test]
        public async Task BasicClient_GetResultWithNotFoundIntAsync_ResultWithError()
        {
            const int id = 1;
            var expectedError = new Error { Message = "Error" };
            using var api = ResultApiMockFactory.MockGetNotFoundIntMethod(id);

            var result = await NClientGallery.Clients.GetRest().For<IResultClientWithMetadata>(host: api.Urls.First()).Build()
                .GetIResultWithIntAsync(id);

            using var assertionScope = new AssertionScope();
            result.Value.Should().Be(null);
            result.Error.Should().BeEquivalentTo(expectedError);
        }
        
        [Test]
        public async Task BasicClient_GetResultWithEntityAsync_ResultWithEntity()
        {
            const int id = 1;
            var expectedEntity = new BasicEntity { Id = id, Value = 2 };
            using var api = ResultApiMockFactory.MockGetEntityMethod(id);

            var result = await NClientGallery.Clients.GetRest().For<IResultClientWithMetadata>(host: api.Urls.First()).Build()
                .GetResultWithEntityAsync(id);

            using var assertionScope = new AssertionScope();
            result.Value.Should().BeEquivalentTo(expectedEntity);
            result.Error.Should().Be(null);
        }
        
        [Test]
        public async Task BasicClient_GetResultWithNotFoundEntityAsync_ResultWithError()
        {
            const int id = 1;
            var expectedError = new Error { Message = "Error" };
            using var api = ResultApiMockFactory.MockGetNotFoundEntityMethod(id);

            var result = await NClientGallery.Clients.GetRest().For<IResultClientWithMetadata>(host: api.Urls.First()).Build()
                .GetResultWithEntityAsync(id);

            using var assertionScope = new AssertionScope();
            result.Value.Should().Be(null);
            result.Error.Should().BeEquivalentTo(expectedError);
        }
    }
}
