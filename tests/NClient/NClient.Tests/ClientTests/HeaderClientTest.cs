using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;
using NClient.Tests.ClientTests.Helpers;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class HeaderClientTest : ClientTestBase<IHeaderClientWithMetadata>
    {
        [Test]
        public async Task GetWithSingleHeaderAsync_ShouldPassSingleHeader_ThenReturnIntInBody()
        {
            const int id = 1;
            using var api = HeaderApiMockFactory.MockGetMethodWithSingleHeader(id);

            var result = await NClientGallery.Clients.GetRest().For<IHeaderClientWithMetadata>(host: api.Urls.First()).Build()
                .GetWithSingleHeaderAsync(id);

            result.Should().Be(id);
        }

        [Test]
        public async Task GetWithMultipleHeaderValuesAsync_ShouldPassMultipleHeaderValues_ThenReturnIntArrayInBody()
        {
            const int id1 = 1;
            const int id2 = 2;
            using var api = HeaderApiMockFactory.MockGetMethodWithMultipleHeaderValues(id1, id2);

            var result = await NClientGallery.Clients.GetRest().For<IHeaderClientWithMetadata>(host: api.Urls.First()).Build()
                .GetWithMultipleHeaderValuesAsync(id1, id2);

            result.Should().BeEquivalentTo(new[] { id1, id2 });
        }
        
        [Test]
        public async Task GetWithMultipleHeadersAsync_ShouldPassMultipleHeaders_ThenReturnIntArrayInBody()
        {
            const int id1 = 1;
            const int id2 = 2;
            using var api = HeaderApiMockFactory.MockGetMethodWithMultipleHeaders(id1, id2);

            var result = await NClientGallery.Clients.GetRest().For<IHeaderClientWithMetadata>(host: api.Urls.First()).Build()
                .GetWithMultipleHeadersAsync(id1, id2);

            result.Should().BeEquivalentTo(new[] { id1, id2 });
        }

        [Test]
        public async Task PostWithSingleContentHeaderAsync_ShouldPassSingleContentHeader_ThenReturnOk()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            const string expectedHeaderName = "Content-Range";
            const string expectedHeaderValue = "items 1-1/*";
            using var api = HeaderApiMockFactory.MockPostMethodWithHeader(entity, expectedHeaderName, expectedHeaderValue);

            await NClientGallery.Clients.GetRest().For<IHeaderClientWithMetadata>(host: api.Urls.First()).Build()
                .PostWithSingleContentHeaderAsync(entity, expectedHeaderValue);
        }
        
        [Test]
        public async Task PostWithSingleOverridingContentHeaderAsync_ShouldPassSingleContentHeader_ThenReturnOk()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            const string expectedHeaderName = "Content-Type";
            const string expectedHeaderValue = "text/html, application/json";
            const string headerValue = "text/html";
            using var api = HeaderApiMockFactory.MockPostMethodWithHeader(entity, expectedHeaderName, expectedHeaderValue);

            await NClientGallery.Clients.GetRest().For<IHeaderClientWithMetadata>(host: api.Urls.First()).Build()
                .PostWithSingleOverridingContentHeaderAsync(entity, headerValue);
        }

        [Test]
        public async Task GetWithSingleStaticHeaderAsync_ShouldPassSingleHeader_ThenReturnIntInBody()
        {
            const int id = 1;
            using var api = HeaderApiMockFactory.MockGetMethodWithSingleHeader(id);

            var result = await NClientGallery.Clients.GetRest().For<IHeaderClientWithMetadata>(host: api.Urls.First()).Build()
                .GetWithSingleStaticHeaderAsync();
            
            result.Should().Be(id);
        }
        
        [Test]
        public async Task GetWithMultipleStaticHeaderValuesAsync_ShouldPassMultipleHeaderValues_ThenReturnIntArrayInBody()
        {
            const int id1 = 1;
            const int id2 = 2;
            using var api = HeaderApiMockFactory.MockGetMethodWithMultipleHeaderValues(id1, id2);

            var result = await NClientGallery.Clients.GetRest().For<IHeaderClientWithMetadata>(host: api.Urls.First()).Build()
                .GetWithMultipleStaticHeaderValuesAsync();
            
            result.Should().BeEquivalentTo(new[] { id1, id2 });
        }
        
        [Test]
        public async Task GetWithMultipleStaticHeadersAsync_ShouldPassMultipleHeaders_ThenReturnIntArrayInBody()
        {
            const int id1 = 1;
            const int id2 = 2;
            using var api = HeaderApiMockFactory.MockGetMethodWithMultipleHeaders(id1, id2);

            var result = await NClientGallery.Clients.GetRest().For<IHeaderClientWithMetadata>(host: api.Urls.First()).Build()
                .GetWithMultipleStaticHeadersAsync();
            
            result.Should().BeEquivalentTo(new[] { id1, id2 });
        }
        [Test]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public async Task GetWithMultipleStaticAndParamHeadersAsync_ShouldPassMultipleHeaders_ThenReturnIntArrayInBody()
        {
            const int id1_1 = 1;
            const int id2_1 = 2;
            const int id1_2 = 3;
            const int id2_2 = 4;
            using var api = HeaderApiMockFactory.MockGetMethodWithMultipleHeaders(id1_1, id2_1, id1_2, id2_2);

            var result = await NClientGallery.Clients.GetRest().For<IHeaderClientWithMetadata>(host: api.Urls.First()).Build()
                .GetWithMultipleStaticAndParamHeadersAsync(id1_2, id2_2);
            
            result.Should().BeEquivalentTo(new[] { id1_1, id2_1, id1_2, id2_2 });
        }
        
        [Test]
        public async Task PostWithSingleStaticContentHeaderAsync_ShouldPassSingleContentHeader_ThenReturnOk()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            const string expectedHeaderName = "Content-Range";
            const string expectedHeaderValue = "items 1-1/*";
            using var api = HeaderApiMockFactory.MockPostMethodWithHeader(entity, expectedHeaderName, expectedHeaderValue);

            await NClientGallery.Clients.GetRest().For<IHeaderClientWithMetadata>(host: api.Urls.First()).Build()
                .PostWithSingleStaticContentHeaderAsync(entity);
        }
        
        [Test]
        public async Task PostWithSingleStaticOverridingContentHeaderAsync_ShouldPassSingleContentHeader_ThenReturnOk()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            const string expectedHeaderName = "Content-Type";
            const string expectedHeaderValue = "text/html, application/json";
            using var api = HeaderApiMockFactory.MockPostMethodWithHeader(entity, expectedHeaderName, expectedHeaderValue);

            await NClientGallery.Clients.GetRest().For<IHeaderClientWithMetadata>(host: api.Urls.First()).Build()
                .PostWithSingleStaticOverridingContentHeaderAsync(entity);
        }
    }
}
