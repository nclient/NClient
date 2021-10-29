﻿using System.Threading.Tasks;
using FluentAssertions;
using NClient.Standalone.Tests.Clients;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Entities;
using NClient.Testing.Common.Helpers;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class RestClientTest
    {
        private IRestClientWithMetadata _restClient = null!;
        private RestApiMockFactory _restApiMockFactory = null!;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _restApiMockFactory = new RestApiMockFactory(PortsPool.Get());
        }
        
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            PortsPool.Put(_restApiMockFactory.ApiUri.Port);
        }
        
        [SetUp]
        public void Setup()
        {
            _restClient = NClientGallery.Clients
                .GetBasic()
                .For<IRestClientWithMetadata>(_restApiMockFactory.ApiUri.ToString())
                .Build();
        }

        [Test]
        public async Task RestClient_GetAsync_IntInBody()
        {
            const int id = 1;
            using var api = _restApiMockFactory.MockIntGetMethod(id);

            var result = await _restClient.GetAsync(id);

            result.Should().Be(id);
        }

        [Test]
        public async Task RestClient_GetAsync_StringInBody()
        {
            const string id = "1";
            using var api = _restApiMockFactory.MockStringGetMethod(id);

            var result = await _restClient.GetAsync(id);

            result.Should().Be(id);
        }

        [Test]
        public async Task RestClient_PostAsync_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _restApiMockFactory.MockPostMethod(entity);

            await _restClient
                .Invoking(async x => await x.PostAsync(entity))
                .Should()
                .NotThrowAsync();
        }

        [Test]
        public async Task RestClient_PutAsync_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _restApiMockFactory.MockPutMethod(entity);

            await _restClient
                .Invoking(async x => await x.PutAsync(entity))
                .Should()
                .NotThrowAsync();
        }

        [Test]
        public async Task RestClient_DeleteAsync_NotThrow()
        {
            const int id = 1;
            using var api = _restApiMockFactory.MockDeleteMethod(id);

            await _restClient
                .Invoking(async x => await x.DeleteAsync(id))
                .Should()
                .NotThrowAsync();
        }
    }
}
