﻿using System.Linq;
using System.Net;
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
    public class OverrideClientTest : ClientTestBase<IOverriddenClientWithMetadata>
    {
        [Test]
        public async Task OverriddenClient_GetAsync_SendsGetRequestAndReceivesHttpResponseWithIntContent()
        {
            const int id = 1;
            using var api = OverriddenApiMockFactory.MockGetMethod(id);

            var result = await NClientGallery.Clients.GetRest().For<IOverriddenClientWithMetadata>(host: api.Urls.First()).Build()
                .GetAsync(id);

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int) HttpStatusCode.OK);
            result.Data.Should().Be(1);
        }

        [Test]
        public async Task OverriddenClient_PostAsync_SendsPutRequestAndReceivesHttpResponse()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = OverriddenApiMockFactory.MockPostMethod(entity);

            var result = await NClientGallery.Clients.GetRest().For<IOverriddenClientWithMetadata>(host: api.Urls.First()).Build()
                .PostAsync(entity);

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int) HttpStatusCode.OK);
        }

        [Test]
        public async Task OverriddenClient_PutAsync_SendsPutWithTestHeaderAndNotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = OverriddenApiMockFactory.MockPutMethod(entity);

            await NClientGallery.Clients.GetRest().For<IOverriddenClientWithMetadata>(host: api.Urls.First()).Build()
                .Invoking(async x => await x.PutAsync(entity))
                .Should()
                .NotThrowAsync();
        }

        [Test]
        public async Task OverriddenClient_DeleteAsync_SendsIntInBodyAndReceivesOkString()
        {
            const int id = 1;
            using var api = OverriddenApiMockFactory.MockDeleteMethod(id);

            var result = await NClientGallery.Clients.GetRest().For<IOverriddenClientWithMetadata>(host: api.Urls.First()).Build()
                .DeleteAsync(id);

            result.Should().NotBeNull();
            result.Should().Be("OK");
        }
    }
}
