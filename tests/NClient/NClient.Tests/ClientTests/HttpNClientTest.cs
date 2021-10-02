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
using NClient.Testing.Common.Entities;
using NClient.Tests.Clients;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class HttpNClientTest
    {
        private static readonly HttpRequest HttpRequestStub = new(Guid.Empty, new Uri("http://localhost:5000"), HttpMethod.Get);
        private static readonly HttpResponse HttpResponseStub = new(HttpRequestStub);

        private IReturnClientWithMetadata _returnClient = null!;
        private ReturnApiMockFactory _returnApiMockFactory = null!;

        [SetUp]
        public void Setup()
        {
            _returnApiMockFactory = new ReturnApiMockFactory(port: 5013);
            _returnClient = new NClientBuilder()
                .For<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString())
                .Build();
        }

        [Test]
        public async Task GetAsync_ServiceReturnsEntity_HttpResponseWithValue()
        {
            const int id = 1;
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockGetAsyncMethod(id, entity);

            var result = await _returnClient.AsHttp().GetHttpResponse(client => client.GetAsync(id));

            result.Should().BeEquivalentTo(new HttpResponse<BasicEntity>(HttpResponseStub, HttpRequestStub, entity)
            {
                StatusCode = HttpStatusCode.OK,
                Content = new HttpResponseContent(
                    Encoding.UTF8.GetBytes("{\"Id\":1,\"Value\":2}"),
                    new HttpResponseContentHeaderContainer(new[]
                    {
                        new HttpHeader(HttpKnownHeaderNames.ContentType, "application/json"),
                        new HttpHeader(HttpKnownHeaderNames.ContentLength, "18")
                    })),
                ProtocolVersion = new Version("1.1"),
                StatusDescription = "OK"
            }, ExcludeInessentialFields);
        }

        [Test]
        public async Task GetAsync_ServiceReturnsEntity_HttpResponseWithValueWithoutError()
        {
            const int id = 1;
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockGetAsyncMethod(id, entity);

            var result = await _returnClient.AsHttp()
                .GetHttpResponse<BasicEntity, Error>(client => client.GetAsync(id));

            result.Should().BeEquivalentTo(new HttpResponseWithError<BasicEntity, Error>(HttpResponseStub, HttpRequestStub, entity, error: null)
            {
                StatusCode = HttpStatusCode.OK,
                Content = new HttpResponseContent(
                    Encoding.UTF8.GetBytes("{\"Id\":1,\"Value\":2}"),
                    new HttpResponseContentHeaderContainer(new[]
                    {
                        new HttpHeader(HttpKnownHeaderNames.ContentType, "application/json"),
                        new HttpHeader(HttpKnownHeaderNames.ContentLength, "18")
                    })),
                ProtocolVersion = new Version("1.1"),
                StatusDescription = "OK"
            }, ExcludeInessentialFields);
        }

        [Test]
        public void Get_ServiceReturnsEntity_HttpResponseWithValue()
        {
            const int id = 1;
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockGetMethod(id, entity);

            var result = _returnClient.AsHttp().GetHttpResponse(client => client.Get(id));

            result.Should().BeEquivalentTo(new HttpResponse<BasicEntity>(HttpResponseStub, HttpRequestStub, entity)
            {
                StatusCode = HttpStatusCode.OK,
                Content = new HttpResponseContent(
                    Encoding.UTF8.GetBytes("{\"Id\":1,\"Value\":2}"),
                    new HttpResponseContentHeaderContainer(new[]
                    {
                        new HttpHeader(HttpKnownHeaderNames.ContentType, "application/json"),
                        new HttpHeader(HttpKnownHeaderNames.ContentLength, "18")
                    })),
                ProtocolVersion = new Version("1.1"),
                StatusDescription = "OK"
            }, ExcludeInessentialFields);
        }

        [Test]
        public void Get_ServiceReturnsEntity_HttpResponseWithValueWithoutError()
        {
            const int id = 1;
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockGetMethod(id, entity);

            var result = _returnClient.AsHttp().GetHttpResponse<BasicEntity, Error>(client => client.Get(id));

            result.Should().BeEquivalentTo(new HttpResponseWithError<BasicEntity, Error>(HttpResponseStub, HttpRequestStub, entity, error: null)
            {
                StatusCode = HttpStatusCode.OK,
                Content = new HttpResponseContent(
                    Encoding.UTF8.GetBytes("{\"Id\":1,\"Value\":2}"),
                    new HttpResponseContentHeaderContainer(new[]
                    {
                        new HttpHeader(HttpKnownHeaderNames.ContentType, "application/json"),
                        new HttpHeader(HttpKnownHeaderNames.ContentLength, "18")
                    })),
                ProtocolVersion = new Version("1.1"),
                StatusDescription = "OK"
            }, ExcludeInessentialFields);
        }

        [Test]
        public async Task PostAsync_ServiceReturnsOk_HttpResponseWithoutValue()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockPostAsyncMethod(entity);

            var httpResponse = await _returnClient.AsHttp().GetHttpResponse(client => client.PostAsync(entity));

            httpResponse.Should().BeEquivalentTo(new HttpResponse(HttpRequestStub)
            {
                StatusCode = HttpStatusCode.OK,
                Content = new HttpResponseContent(
                    Encoding.UTF8.GetBytes(""),
                    new HttpResponseContentHeaderContainer(new[]
                    {
                        new HttpHeader(HttpKnownHeaderNames.ContentType, "application/json"),
                        new HttpHeader(HttpKnownHeaderNames.ContentLength, "0")
                    })),
                ProtocolVersion = new Version("1.1"),
                StatusDescription = "OK"
            }, ExcludeInessentialFields);
        }

        [Test]
        public async Task PostAsync_ServiceReturnsOk_HttpResponseWithoutValueWithoutError()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockPostAsyncMethod(entity);

            var httpResponse = await _returnClient.AsHttp().GetHttpResponse<Error>(client => client.PostAsync(entity));

            httpResponse.Should().BeEquivalentTo(new HttpResponseWithError<Error>(httpResponse, httpResponse.Request, error: null)
            {
                StatusCode = HttpStatusCode.OK,
                Content = new HttpResponseContent(
                    Encoding.UTF8.GetBytes(""),
                    new HttpResponseContentHeaderContainer(new[]
                    {
                        new HttpHeader(HttpKnownHeaderNames.ContentType, "application/json"),
                        new HttpHeader(HttpKnownHeaderNames.ContentLength, "0")
                    })),
                ProtocolVersion = new Version("1.1"),
                StatusDescription = "OK"
            }, ExcludeInessentialFields);
        }

        [Test]
        public void Post_ServiceReturnsOk_HttpResponseWithoutValue()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockPostMethod(entity);

            var httpResponse = _returnClient.AsHttp().GetHttpResponse(client => client.Post(entity));

            httpResponse.Should().BeEquivalentTo(new HttpResponse(HttpRequestStub)
            {
                StatusCode = HttpStatusCode.OK,
                Content = new HttpResponseContent(
                    Encoding.UTF8.GetBytes(""),
                    new HttpResponseContentHeaderContainer(new[]
                    {
                        new HttpHeader(HttpKnownHeaderNames.ContentType, "application/json"),
                        new HttpHeader(HttpKnownHeaderNames.ContentLength, "0")
                    })),
                ProtocolVersion = new Version("1.1"),
                StatusDescription = "OK"
            }, ExcludeInessentialFields);
        }

        [Test]
        public void Post_ServiceReturnsOk_HttpResponseWithoutValueWithoutError()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockPostMethod(entity);

            var httpResponse = _returnClient.AsHttp().GetHttpResponse<Error>(client => client.Post(entity));

            httpResponse.Should().BeEquivalentTo(new HttpResponseWithError<Error>(httpResponse, httpResponse.Request, error: null)
            {
                StatusCode = HttpStatusCode.OK,
                Content = new HttpResponseContent(
                    Encoding.UTF8.GetBytes(""),
                    new HttpResponseContentHeaderContainer(new[]
                    {
                        new HttpHeader(HttpKnownHeaderNames.ContentType, "application/json"),
                        new HttpHeader(HttpKnownHeaderNames.ContentLength, "0")
                    })),
                ProtocolVersion = new Version("1.1"),
                StatusDescription = "OK"
            }, ExcludeInessentialFields);
        }

        [Test]
        public void Post_ServiceReturnsInternalServerError_HttpResponseWithInternalServerError()
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

        private EquivalencyAssertionOptions<HttpResponseWithError<BasicEntity, Error>> ExcludeInessentialFields(
            EquivalencyAssertionOptions<HttpResponseWithError<BasicEntity, Error>> opts)
        {
            return opts
                .Excluding(x => x.Request)
                .Excluding(x => x.Headers)
                .Excluding(x => x.ResponseUri);
        }

        private EquivalencyAssertionOptions<HttpResponseWithError<Error>> ExcludeInessentialFields(
            EquivalencyAssertionOptions<HttpResponseWithError<Error>> opts)
        {
            return opts
                .Excluding(x => x.Request)
                .Excluding(x => x.Headers)
                .Excluding(x => x.ResponseUri);
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
