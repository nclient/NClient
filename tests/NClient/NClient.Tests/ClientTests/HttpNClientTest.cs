using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Equivalency;
using FluentAssertions.Execution;
using NClient.Providers.Transport;
using NClient.Standalone.Tests.Clients;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Entities;
using NClient.Testing.Common.Helpers;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class HttpNClientTest
    {
        private static readonly Request RequestStub = new(Guid.Empty, endpoint: "http://localhost:5000", RequestType.Read);
        private static readonly Response ResponseStub = new(RequestStub);

        [Test]
        public async Task GetAsync_ServiceReturnsEntity_HttpResponseWithValue()
        {
            const int id = 1;
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = ReturnApiMockFactory.MockGetAsyncMethod(id, entity);

            var result = await NClientGallery.Clients.GetRest().For<IReturnClientWithMetadata>(api.Urls.First()).Build()
                .AsTransport().GetTransportResponse(client => client.GetAsync(id));

            result.Should().BeEquivalentTo(new Response<BasicEntity>(ResponseStub, RequestStub, entity, stringContent: "{\"Id\":1,\"Value\":2}")
            {
                StatusCode = (int) HttpStatusCode.OK,
                Content = new Content(
                    new MemoryStream(Encoding.UTF8.GetBytes("{\"Id\":1,\"Value\":2}")),
                    Encoding.UTF8.WebName,
                    new MetadataContainer(new[]
                    {
                        new Metadata(HttpKnownHeaderNames.ContentEncoding, "utf-8"),
                        new Metadata(HttpKnownHeaderNames.ContentType, "application/json"),
                        new Metadata(HttpKnownHeaderNames.ContentLength, "18")
                    })),
                ProtocolVersion = new Version("1.1"),
                StatusDescription = "OK",
                IsSuccessful = true
            }, ExcludeInessentialFields);
        }

        [Test]
        public async Task GetAsync_ServiceReturnsEntity_HttpResponseWithValueWithoutError()
        {
            const int id = 1;
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = ReturnApiMockFactory.MockGetAsyncMethod(id, entity);

            var result = await NClientGallery.Clients.GetRest().For<IReturnClientWithMetadata>(api.Urls.First()).Build()
                .AsTransport().GetTransportResponse<BasicEntity, Error>(client => client.GetAsync(id));

            result.Should().BeEquivalentTo(new ResponseWithError<BasicEntity, Error>(ResponseStub, RequestStub, entity, error: null, stringContent: "{\"Id\":1,\"Value\":2}")
            {
                StatusCode = (int) HttpStatusCode.OK,
                Content = new Content(
                    new MemoryStream(Encoding.UTF8.GetBytes("{\"Id\":1,\"Value\":2}")),
                    Encoding.UTF8.WebName,
                    new MetadataContainer(new[]
                    {
                        new Metadata(HttpKnownHeaderNames.ContentEncoding, "utf-8"),
                        new Metadata(HttpKnownHeaderNames.ContentType, "application/json"),
                        new Metadata(HttpKnownHeaderNames.ContentLength, "18")
                    })),
                ProtocolVersion = new Version("1.1"),
                StatusDescription = "OK",
                IsSuccessful = true
            }, ExcludeInessentialFields);
        }

        [Test]
        public void Get_ServiceReturnsEntity_HttpResponseWithValue()
        {
            const int id = 1;
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = ReturnApiMockFactory.MockGetMethod(id, entity);

            var result = NClientGallery.Clients.GetRest().For<IReturnClientWithMetadata>(api.Urls.First()).Build()
                .AsTransport().GetTransportResponse(client => client.Get(id));

            result.Should().BeEquivalentTo(new Response<BasicEntity>(ResponseStub, RequestStub, entity, stringContent: "{\"Id\":1,\"Value\":2}")
            {
                StatusCode = (int) HttpStatusCode.OK,
                Content = new Content(
                    new MemoryStream(Encoding.UTF8.GetBytes("{\"Id\":1,\"Value\":2}")),
                    Encoding.UTF8.WebName,
                    new MetadataContainer(new[]
                    {
                        new Metadata(HttpKnownHeaderNames.ContentEncoding, "utf-8"),
                        new Metadata(HttpKnownHeaderNames.ContentType, "application/json"),
                        new Metadata(HttpKnownHeaderNames.ContentLength, "18")
                    })),
                ProtocolVersion = new Version("1.1"),
                StatusDescription = "OK",
                IsSuccessful = true
            }, ExcludeInessentialFields);
        }

        [Test]
        public void Get_ServiceReturnsEntity_HttpResponseWithValueWithoutError()
        {
            const int id = 1;
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = ReturnApiMockFactory.MockGetMethod(id, entity);

            var result = NClientGallery.Clients.GetRest().For<IReturnClientWithMetadata>(api.Urls.First()).Build()
                .AsTransport().GetTransportResponse<BasicEntity, Error>(client => client.Get(id));

            result.Should().BeEquivalentTo(new ResponseWithError<BasicEntity, Error>(ResponseStub, RequestStub, entity, error: null, stringContent: "{\"Id\":1,\"Value\":2}")
            {
                StatusCode = (int) HttpStatusCode.OK,
                Content = new Content(
                    new MemoryStream(Encoding.UTF8.GetBytes("{\"Id\":1,\"Value\":2}")),
                    Encoding.UTF8.WebName,
                    new MetadataContainer(new[]
                    {
                        new Metadata(HttpKnownHeaderNames.ContentEncoding, "utf-8"),
                        new Metadata(HttpKnownHeaderNames.ContentType, "application/json"),
                        new Metadata(HttpKnownHeaderNames.ContentLength, "18")
                    })),
                ProtocolVersion = new Version("1.1"),
                StatusDescription = "OK",
                IsSuccessful = true
            }, ExcludeInessentialFields);
        }

        [Test]
        public async Task PostAsync_ServiceReturnsOk_HttpResponseWithoutValue()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = ReturnApiMockFactory.MockPostAsyncMethod(entity);

            var httpResponse = await NClientGallery.Clients.GetRest().For<IReturnClientWithMetadata>(api.Urls.First()).Build()
                .AsTransport().GetTransportResponse(client => client.PostAsync(entity));

            httpResponse.Should().BeEquivalentTo(new Response(RequestStub)
            {
                StatusCode = (int) HttpStatusCode.OK,
                Content = new Content(
                    streamContent: new MemoryStream(Encoding.UTF8.GetBytes("")),
                    headerContainer: new MetadataContainer(new[]
                    {
                        new Metadata(HttpKnownHeaderNames.ContentType, "application/json"),
                        new Metadata(HttpKnownHeaderNames.ContentLength, "0")
                    })),
                ProtocolVersion = new Version("1.1"),
                StatusDescription = "OK",
                IsSuccessful = true
            }, ExcludeInessentialFields);
        }

        [Test]
        public async Task PostAsync_ServiceReturnsOk_HttpResponseWithoutValueWithoutError()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = ReturnApiMockFactory.MockPostAsyncMethod(entity);

            var httpResponse = await NClientGallery.Clients.GetRest().For<IReturnClientWithMetadata>(api.Urls.First()).Build()
                .AsTransport().GetTransportResponse<Error>(client => client.PostAsync(entity));

            httpResponse.Should().BeEquivalentTo(new ResponseWithError<Error>(httpResponse, httpResponse.Request, error: null, string.Empty)
            {
                StatusCode = (int) HttpStatusCode.OK,
                Content = new Content(
                    streamContent: new MemoryStream(Encoding.UTF8.GetBytes("")),
                    headerContainer: new MetadataContainer(new[]
                    {
                        new Metadata(HttpKnownHeaderNames.ContentType, "application/json"),
                        new Metadata(HttpKnownHeaderNames.ContentLength, "0")
                    })),
                ProtocolVersion = new Version("1.1"),
                StatusDescription = "OK",
                IsSuccessful = true
            }, ExcludeInessentialFields);
        }

        [Test]
        public void Post_ServiceReturnsOk_HttpResponseWithoutValue()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = ReturnApiMockFactory.MockPostMethod(entity);

            var httpResponse = NClientGallery.Clients.GetRest().For<IReturnClientWithMetadata>(api.Urls.First()).Build()
                .AsTransport().GetTransportResponse(client => client.Post(entity));

            httpResponse.Should().BeEquivalentTo(new Response(RequestStub)
            {
                StatusCode = (int) HttpStatusCode.OK,
                Content = new Content(
                    streamContent: new MemoryStream(Encoding.UTF8.GetBytes("")),
                    headerContainer: new MetadataContainer(new[]
                    {
                        new Metadata(HttpKnownHeaderNames.ContentType, "application/json"),
                        new Metadata(HttpKnownHeaderNames.ContentLength, "0")
                    })),
                ProtocolVersion = new Version("1.1"),
                StatusDescription = "OK",
                IsSuccessful = true
            }, ExcludeInessentialFields);
        }

        [Test]
        public void Post_ServiceReturnsOk_HttpResponseWithoutValueWithoutError()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = ReturnApiMockFactory.MockPostMethod(entity);

            var httpResponse = NClientGallery.Clients.GetRest().For<IReturnClientWithMetadata>(api.Urls.First()).Build()
                .AsTransport().GetTransportResponse<Error>(client => client.Post(entity));

            httpResponse.Should().BeEquivalentTo(new ResponseWithError<Error>(httpResponse, httpResponse.Request, error: null, string.Empty)
            {
                StatusCode = (int) HttpStatusCode.OK,
                Content = new Content(
                    streamContent: new MemoryStream(Encoding.UTF8.GetBytes("")),
                    headerContainer: new MetadataContainer(new[]
                    {
                        new Metadata(HttpKnownHeaderNames.ContentType, "application/json"),
                        new Metadata(HttpKnownHeaderNames.ContentLength, "0")
                    })),
                ProtocolVersion = new Version("1.1"),
                StatusDescription = "OK",
                IsSuccessful = true
            }, ExcludeInessentialFields);
        }

        [Test]
        public void Post_ServiceReturnsInternalServerError_HttpResponseWithInternalServerError()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = ReturnApiMockFactory.MockInternalServerError();

            var httpResponse = NClientGallery.Clients.GetRest().For<IReturnClientWithMetadata>(api.Urls.First()).Build()
                .AsTransport().GetTransportResponse(client => client.Post(entity));

            using var assertionScope = new AssertionScope();
            httpResponse.StatusCode.Should().Be((int) HttpStatusCode.InternalServerError);
            httpResponse.IsSuccessful.Should().BeFalse();
            httpResponse.ErrorMessage.Should().NotBeNull();
            httpResponse.ErrorException.Should().NotBeNull();
        }

        private EquivalencyAssertionOptions<ResponseWithError<BasicEntity, Error>> ExcludeInessentialFields(
            EquivalencyAssertionOptions<ResponseWithError<BasicEntity, Error>> opts)
        {
            return opts
                .Excluding(x => x.Request)
                .Excluding(x => x.Metadatas)
                .Excluding(x => x.Content.StreamContent)
                .Excluding(x => x.Request.Content!.StreamContent)
                .Excluding(x => x.Endpoint);
        }

        private EquivalencyAssertionOptions<ResponseWithError<Error>> ExcludeInessentialFields(
            EquivalencyAssertionOptions<ResponseWithError<Error>> opts)
        {
            return opts
                .Excluding(x => x.Request)
                .Excluding(x => x.Metadatas)
                .Excluding(x => x.Content.StreamContent)
                .Excluding(x => x.Request.Content!.StreamContent)
                .Excluding(x => x.Endpoint);
        }

        private EquivalencyAssertionOptions<Response<BasicEntity>> ExcludeInessentialFields(
            EquivalencyAssertionOptions<Response<BasicEntity>> opts)
        {
            return opts
                .Excluding(x => x.Request)
                .Excluding(x => x.Metadatas)
                .Excluding(x => x.Content.StreamContent)
                .Excluding(x => x.Request.Content!.StreamContent)
                .Excluding(x => x.Endpoint);
        }

        private EquivalencyAssertionOptions<Response> ExcludeInessentialFields(
            EquivalencyAssertionOptions<Response> opts)
        {
            return opts
                .Excluding(x => x.Request)
                .Excluding(x => x.Metadatas)
                .Excluding(x => x.Content.StreamContent)
                .Excluding(x => x.Request.Content!.StreamContent)
                .Excluding(x => x.Endpoint);
        }
    }
}
