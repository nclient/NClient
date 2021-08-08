using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Equivalency;
using FluentAssertions.Execution;
using NClient.Abstractions.HttpClients;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NClient.Testing.Common.Entities;
using NClient.Tests.Controllers;
using NUnit.Framework;

#pragma warning disable 618

namespace NClient.Tests.ControllerBasedClientTests
{
    [Parallelizable]
    public class HttpNClientTest
    {
        private static readonly HttpRequest HttpRequestStub = new(Guid.Empty, new Uri("http://localhost:5000"), HttpMethod.Get);
        private static readonly HttpResponse HttpResponseStub = new(HttpRequestStub);

        private IReturnClient _returnClient = null!;
        private ReturnApiMockFactory _returnApiMockFactory = null!;

        [SetUp]
        public void Setup()
        {
            _returnApiMockFactory = new ReturnApiMockFactory(port: 5015);
            _returnClient = new NClientBuilder()
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
            result.Should().BeEquivalentTo(new HttpResponse<BasicEntity>(HttpResponseStub, HttpRequestStub, entity)
            {
                StatusCode = HttpStatusCode.OK,
                RawBytes = Encoding.UTF8.GetBytes("{\"Id\":1,\"Value\":2}"),
                ContentLength = 18,
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
            result.Should().BeEquivalentTo(new HttpResponse<BasicEntity>(HttpResponseStub, HttpRequestStub, entity)
            {
                StatusCode = HttpStatusCode.OK,
                RawBytes = Encoding.UTF8.GetBytes("{\"Id\":1,\"Value\":2}"),
                ContentLength = 18,
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
            httpResponse.Should().BeEquivalentTo(new HttpResponse(HttpRequestStub)
            {
                StatusCode = HttpStatusCode.OK,
                ContentLength = 0,
                RawBytes = Encoding.UTF8.GetBytes(""),
                ContentType = "application/json",
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
            httpResponse.Should().BeEquivalentTo(new HttpResponse(HttpRequestStub)
            {
                StatusCode = HttpStatusCode.OK,
                ContentLength = 0,
                RawBytes = Encoding.UTF8.GetBytes(""),
                ContentType = "application/json",
                ProtocolVersion = new Version("1.1"),
                Server = "Kestrel",
                StatusDescription = "OK",
            }, ExcludeInessentialFields);
        }

        [Test]
        public void GetHttpResponse_ServiceNotExists_InternalServerError()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockInternalServerError();

            var httpResponse = _returnClient.AsHttp().GetHttpResponse(client => client.Post(entity));

            using var assertionScope = new AssertionScope();
            httpResponse.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            httpResponse.IsSuccessful.Should().BeFalse();
            httpResponse.ErrorMessage.Should().NotBeNull();
            httpResponse.ErrorException.Should().NotBeNull();
        }

        private EquivalencyAssertionOptions<HttpResponse<BasicEntity>> ExcludeInessentialFields(
            EquivalencyAssertionOptions<HttpResponse<BasicEntity>> opts)
        {
            return opts
                .Excluding(x => x.Request)
                .Excluding(x => x.Headers)
                .Excluding(x => x.ResponseUri);
        }

        private EquivalencyAssertionOptions<HttpResponse> ExcludeInessentialFields(
            EquivalencyAssertionOptions<HttpResponse> opts)
        {
            return opts
                .Excluding(x => x.Request)
                .Excluding(x => x.Headers)
                .Excluding(x => x.ResponseUri);
        }
    }
}
