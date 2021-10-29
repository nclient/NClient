using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NClient.Standalone.Tests.Clients;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class ReturnClientTest
    {
        [Test]
        public void ResultClient_Get_BasicEntity()
        {
            const int id = 1;
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = ReturnApiMockFactory.MockGetMethod(id, entity);

            var result = NClientGallery.Clients.GetBasic().For<IReturnClientWithMetadata>(api.Urls.First()).Build()
                .Get(id);

            result.Should().BeEquivalentTo(entity);
        }

        [Test]
        public async Task ResultClient_GetAsync_TaskWithBasicEntity()
        {
            const int id = 1;
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = ReturnApiMockFactory.MockGetAsyncMethod(id, entity);

            var result = await NClientGallery.Clients.GetBasic().For<IReturnClientWithMetadata>(api.Urls.First()).Build()
                .GetAsync(id);

            result.Should().BeEquivalentTo(entity);
        }

        [Test]
        public void ResultClient_GetHttpResponse_HttpResponseWithBasicEntity()
        {
            const int id = 1;
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = ReturnApiMockFactory.MockGetAsyncMethod(id, entity);

            var result = NClientGallery.Clients.GetBasic().For<IReturnClientWithMetadata>(api.Urls.First()).Build()
                .GetIHttpResponse(id);

            result.IsSuccessful.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(entity);
        }
        
        [Test]
        public void ResultClient_Post_Void()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = ReturnApiMockFactory.MockPostMethod(entity);

            NClientGallery.Clients.GetBasic().For<IReturnClientWithMetadata>(api.Urls.First()).Build()
                .Post(entity);
        }

        [Test]
        public async Task ResultClient_PostAsync_Task()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = ReturnApiMockFactory.MockPostAsyncMethod(entity);

            await NClientGallery.Clients.GetBasic().For<IReturnClientWithMetadata>(api.Urls.First()).Build()
                .PostAsync(entity);
        }

        [Test]
        public void ResultClient_PostHttpResponse_HttpResponseWithoutValue()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = ReturnApiMockFactory.MockPostMethod(entity);

            var result = NClientGallery.Clients.GetBasic().For<IReturnClientWithMetadata>(api.Urls.First()).Build()
                .PostHttpResponse(entity);

            result.IsSuccessful.Should().BeTrue();
        }
    }
}
