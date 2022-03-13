using System.Linq;
using FluentAssertions;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class SyncClientTest
    {
        [Test, Order(0)]
        public void SyncClient_Build_NotThrow()
        {
            const string anyHost = "http://localhost:5000";
            
            NClientGallery.Clients.GetRest().For<ISyncClientWithMetadata>(anyHost)
                .Invoking(builder => builder.Build())
                .Should()
                .NotThrow();
        }
        
        [Test]
        public void SyncClient_Get_IntInBody()
        {
            const int id = 1;
            using var api = SyncApiMockFactory.MockGetMethod(id);

            var result = NClientGallery.Clients.GetRest().For<ISyncClientWithMetadata>(host: api.Urls.First()).Build()
                .Get(id);

            result.Should().Be(id);
        }

        [Test]
        public void SyncClient_Post_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = SyncApiMockFactory.MockPostMethod(entity);

            NClientGallery.Clients.GetRest().For<ISyncClientWithMetadata>(host: api.Urls.First()).Build()
                .Invoking(x => x.Post(entity))
                .Should()
                .NotThrow();
        }

        [Test]
        public void SyncClient_Put_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = SyncApiMockFactory.MockPutMethod(entity);

            NClientGallery.Clients.GetRest().For<ISyncClientWithMetadata>(host: api.Urls.First()).Build()
                .Invoking(x => x.Put(entity))
                .Should()
                .NotThrow();
        }

        [Test]
        public void SyncClient_Delete_NotThrow()
        {
            const int id = 1;
            using var api = SyncApiMockFactory.MockDeleteMethod(id);

            NClientGallery.Clients.GetRest().For<ISyncClientWithMetadata>(host: api.Urls.First()).Build()
                .Invoking(x => x.Delete(id))
                .Should()
                .NotThrow();
        }
    }
}
