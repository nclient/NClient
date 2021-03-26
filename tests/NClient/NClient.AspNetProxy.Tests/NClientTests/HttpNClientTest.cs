using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Equivalency;
using NClient.Abstractions.HttpClients;
using NClient.AspNetProxy.Extensions;
using NClient.AspNetProxy.Tests.Controllers;
using NClient.Core.Extensions;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;
using NUnit.Framework;
#pragma warning disable 618

namespace NClient.AspNetProxy.Tests.NClientTests
{
    [Parallelizable]
    public class HttpNClientTest
    {
        private IReturnClient _returnClient = null!;
        private ReturnApiMockFactory _returnApiMockFactory = null!;

        [SetUp]
        public void Setup()
        {
            _returnApiMockFactory = new ReturnApiMockFactory(port: 5015);
            _returnClient = new NClientControllerBuilder()
                .Use<IReturnClient, ReturnController>(_returnApiMockFactory.ApiUri.ToString())
                .Build();
        }

        [Test]
        public async Task GetHttpResponse_GetAsync_HttpResponseWithValue()
        {
            const int id = 1;
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockGetAsyncMethod(id, entity);

            var result = await _returnClient.AsHttp().GetHttpResponse(client => client.GetAsync(id));
            result.Should().BeEquivalentTo(new HttpResponse<BasicEntity>(entity)
            {
                StatusCode = HttpStatusCode.OK,
                Content = "{\"Id\":1,\"Value\":2}",
                ContentLength = -1,
                ContentType = "application/json",
                ProtocolVersion = new Version("1.1"),
                Server = "Kestrel",
                StatusDescription = "OK",
            }, ExcludeInessentialFields);
        }

        [Test]
        public void GetHttpResponse_Get_HttpResponseWithValue()
        {
            const int id = 1;
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockGetMethod(id, entity);

            var result = _returnClient.AsHttp().GetHttpResponse(client => client.Get(id));
            result.Should().BeEquivalentTo(new HttpResponse<BasicEntity>(entity)
            {
                StatusCode = HttpStatusCode.OK,
                Content = "{\"Id\":1,\"Value\":2}",
                ContentLength = -1,
                ContentType = "application/json",
                ProtocolVersion = new Version("1.1"),
                Server = "Kestrel",
                StatusDescription = "OK",
            }, ExcludeInessentialFields);
        }

        [Test]
        public async Task GetHttpResponse_PostAsync_HttpResponseWithoutValue()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockPostAsyncMethod(entity);

            var httpResponse = await _returnClient.AsHttp().GetHttpResponse(client => client.PostAsync(entity));
            httpResponse.Should().BeEquivalentTo(new HttpResponse
            {
                StatusCode = HttpStatusCode.OK,
                ContentLength = 0,
                ContentType = null,
                ProtocolVersion = new Version("1.1"),
                Server = "Kestrel",
                StatusDescription = "OK",
            }, ExcludeInessentialFields);
        }

        [Test]
        public void GetHttpResponse_Post_HttpResponseWithoutValue()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockPostMethod(entity);

            var httpResponse = _returnClient.AsHttp().GetHttpResponse(client => client.Post(entity));
            httpResponse.Should().BeEquivalentTo(new HttpResponse
            {
                StatusCode = HttpStatusCode.OK,
                ContentLength = 0,
                ContentType = null,
                ProtocolVersion = new Version("1.1"),
                Server = "Kestrel",
                StatusDescription = "OK",
            }, ExcludeInessentialFields);
        }

        [Test]
        public void GetHttpResponse_ServiceNotExists_InternalServerError()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };

            var httpResponse = _returnClient.AsHttp().GetHttpResponse(client => client.Post(entity));
            httpResponse.StatusCode.Should().Be((HttpStatusCode)0);
        }

        private EquivalencyAssertionOptions<HttpResponse<BasicEntity>> ExcludeInessentialFields(
            EquivalencyAssertionOptions<HttpResponse<BasicEntity>> opts)
        {
            return opts
                .Excluding(x => x.Headers)
                .Excluding(x => x.RawBytes)
                .Excluding(x => x.ResponseUri);
        }

        private EquivalencyAssertionOptions<HttpResponse> ExcludeInessentialFields(
            EquivalencyAssertionOptions<HttpResponse> opts)
        {
            return opts
                .Excluding(x => x.Headers)
                .Excluding(x => x.RawBytes)
                .Excluding(x => x.ResponseUri);
        }
    }
}
