using FluentAssertions;
using NClient.AspNetProxy.Extensions;
using NClient.AspNetProxy.Tests.Controllers;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;
using NUnit.Framework;
#pragma warning disable 618

namespace NClient.AspNetProxy.Tests.ClientTests
{
    [Parallelizable]
    public class SyncControllerTest
    {
        private ISyncClient _syncClient = null!;
        private SyncApiMockFactory _syncApiMockFactory = null!;

        [SetUp]
        public void Setup()
        {
            _syncApiMockFactory = new SyncApiMockFactory(port: 5006);
            _syncClient = new NClientControllerProvider()
                .Use<ISyncClient, SyncController>(_syncApiMockFactory.ApiUri.ToString())
                .Build();
        }

        [Test]
        public void SyncClient_Get_IntInBody()
        {
            const int id = 1;
            using var api = _syncApiMockFactory.MockGetMethod(id);

            var result = _syncClient.Get(id);
            result.Should().Be(1);
        }

        [Test]
        public void SyncClient_Post_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _syncApiMockFactory.MockPostMethod(entity);

            _syncClient
                .Invoking(x => x.Post(entity))
                .Should()
                .NotThrow();
        }

        [Test]
        public void SyncClient_Put_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _syncApiMockFactory.MockPutMethod(entity);

            _syncClient
                .Invoking(x => x.Put(entity))
                .Should()
                .NotThrow();
        }

        [Test]
        public void SyncClient_Delete_NotThrow()
        {
            const int id = 1;
            using var api = _syncApiMockFactory.MockDeleteMethod(id);

            _syncClient
                .Invoking(x => x.Delete(id))
                .Should()
                .NotThrow();
        }
    }
}
