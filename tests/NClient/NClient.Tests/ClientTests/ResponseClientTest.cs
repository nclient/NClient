using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using NClient.Exceptions;
using NClient.Standalone.Tests.Clients;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class ResponseClientTest
    {
        private static readonly HttpError BadRequestError = new() { Code = HttpStatusCode.BadRequest, Message = "Error" };
        private static readonly HttpError InternalServerError = new() { Code = HttpStatusCode.InternalServerError, Message = "Error" };

        [Test]
        public async Task GetAsync_ServiceReturnsInt_IntInBody()
        {
            const int id = 1;
            using var api = ResponseApiMockFactory.MockGetMethod(id);

            var result = await NClientGallery.Clients.GetBasic().For<IResponseClientWithMetadata>(api.Urls.First()).Build()
                .GetAsync(id);

            result.Should().Be(id);
        }

        [Test]
        public async Task GetAsync_ServiceReturnsBadRequest_ThrowClientRequestException()
        {
            const int id = 1;
            using var api = ResponseApiMockFactory.MockGetMethodWithBadRequest(id);

            (await NClientGallery.Clients.GetBasic().For<IResponseClientWithMetadata>(api.Urls.First()).Build()
                    .Invoking(async x => await x.GetAsync(id))
                    .Should()
                    .ThrowAsync<ClientRequestException>())
                .Where(x => x.IsHttpError);
        }

        [Test]
        public async Task GetAsync_ServiceReturnsBadRequestWithError_ThrowClientRequestException()
        {
            const int id = 1;
            using var api = ResponseApiMockFactory.MockGetMethodWithBadRequestAndError(id);

            (await NClientGallery.Clients.GetBasic().For<IResponseClientWithMetadata>(api.Urls.First()).Build()
                    .Invoking(async x => await x.GetAsync(id))
                    .Should()
                    .ThrowAsync<ClientRequestException>())
                .Where(x => x.IsHttpError);
        }

        [Test]
        public async Task GetAsync_NotWorkingService_ThrowClientRequestException()
        {
            const int id = 1;
            using var api = ResponseApiMockFactory.MockInternalServerError();

            (await NClientGallery.Clients.GetBasic().For<IResponseClientWithMetadata>(api.Urls.First()).Build()
                    .Invoking(async x => await x.GetAsync(id))
                    .Should()
                    .ThrowAsync<ClientRequestException>())
                .Where(x => x.IsHttpError);
        }

        [Test]
        public async Task GetResponseAsync_ServiceReturnsInt_IntInBody()
        {
            const int id = 1;
            using var api = ResponseApiMockFactory.MockGetMethod(id);

            var result = await NClientGallery.Clients.GetBasic().For<IResponseClientWithMetadata>(api.Urls.First()).Build()
                .GetResponseAsync(id);

            result.Should().NotBeNull();
            using var assertionScope = new AssertionScope();
            result.Data.Should().Be(id);
            result.StatusCode.Should().Be((int) HttpStatusCode.OK);
        }

        [Test]
        public async Task GetResponseAsync_ServiceReturnsBadRequest_ResponseWithBadRequestStatus()
        {
            const int id = 1;
            using var api = ResponseApiMockFactory.MockGetMethodWithBadRequest(id);

            var result = await NClientGallery.Clients.GetBasic().For<IResponseClientWithMetadata>(api.Urls.First()).Build()
                .GetResponseAsync(id);

            result.Should().NotBeNull();
            using var assertionScope = new AssertionScope();
            result.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task GetResponseAsync_ServiceReturnsBadRequestWithError_ResponseWithBadRequestStatusAndContent()
        {
            const int id = 1;
            using var api = ResponseApiMockFactory.MockGetMethodWithBadRequestAndError(id);

            var result = await NClientGallery.Clients.GetBasic().For<IResponseClientWithMetadata>(api.Urls.First()).Build()
                .GetResponseAsync(id);

            result.Should().NotBeNull();
            using var assertionScope = new AssertionScope();
            result.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);
            result.Content.ToString().Should().Be(JsonSerializer.Serialize(BadRequestError));
        }

        [Test]
        public async Task GetResponseAsync_NotWorkingService_ResponseWithInternalServerStatus()
        {
            const int id = 1;
            using var api = ResponseApiMockFactory.MockInternalServerError();

            var result = await NClientGallery.Clients.GetBasic().For<IResponseClientWithMetadata>(api.Urls.First()).Build()
                .GetResponseAsync(id);

            result.Should().NotBeNull();
            using var assertionScope = new AssertionScope();
            result.StatusCode.Should().Be((int) HttpStatusCode.InternalServerError);
        }

        [Test]
        public async Task GetResponseWithErrorAsync_ServiceReturnsInt_IntInBody()
        {
            const int id = 1;
            using var api = ResponseApiMockFactory.MockGetMethod(id);

            var result = await NClientGallery.Clients.GetBasic().For<IResponseClientWithMetadata>(api.Urls.First()).Build()
                .GetResponseWithErrorAsync(id);

            result.Should().NotBeNull();
            using var assertionScope = new AssertionScope();
            result.Data.Should().Be(id);
            result.StatusCode.Should().Be((int) HttpStatusCode.OK);
            result.Error.Should().BeNull();
        }

        [Test]
        public async Task GetResponseWithErrorAsync_ServiceReturnsBadRequest_ThrowClientRequestException()
        {
            const int id = 1;
            using var api = ResponseApiMockFactory.MockGetMethodWithBadRequest(id);

            (await NClientGallery.Clients.GetBasic().For<IResponseClientWithMetadata>(api.Urls.First()).Build()
                    .Invoking(async x => await x.GetResponseWithErrorAsync(id))
                    .Should()
                    .ThrowAsync<ClientRequestException>())
                .Where(x => x.IsHttpError == false);
        }

        [Test]
        public async Task GetResponseWithErrorAsync_ServiceReturnsBadRequestWithError_ResponseWithBadRequestStatusAndError()
        {
            const int id = 1;
            using var api = ResponseApiMockFactory.MockGetMethodWithBadRequestAndError(id);

            var result = await NClientGallery.Clients.GetBasic().For<IResponseClientWithMetadata>(api.Urls.First()).Build()
                .GetResponseWithErrorAsync(id);

            result.Should().NotBeNull();
            using var assertionScope = new AssertionScope();
            result.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);
            result.Error.Should().BeEquivalentTo(BadRequestError);
        }

        [Test]
        public async Task GetResponseWithErrorAsync_NotWorkingService_ResponseWithInternalServerStatusAndError()
        {
            const int id = 1;
            using var api = ResponseApiMockFactory.MockInternalServerError();

            var result = await NClientGallery.Clients.GetBasic().For<IResponseClientWithMetadata>(api.Urls.First()).Build()
                .GetResponseWithErrorAsync(id);

            result.Should().NotBeNull();
            using var assertionScope = new AssertionScope();
            result.StatusCode.Should().Be((int) HttpStatusCode.InternalServerError);
            result.Error.Should().BeEquivalentTo(InternalServerError);
        }

        [Test]
        public async Task PostAsync_ServiceReturnsOk_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = ResponseApiMockFactory.MockPostMethod(entity);

            await NClientGallery.Clients.GetBasic().For<IResponseClientWithMetadata>(api.Urls.First()).Build()
                .Invoking(async x => await x.PostAsync(entity))
                .Should()
                .NotThrowAsync();
        }

        [Test]
        public async Task PostAsync_ServiceReturnsBadRequest_ThrowClientRequestException()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = ResponseApiMockFactory.MockPostMethodWithBadRequest(entity);

            (await NClientGallery.Clients.GetBasic().For<IResponseClientWithMetadata>(api.Urls.First()).Build()
                    .Invoking(async x => await x.PostAsync(entity))
                    .Should()
                    .ThrowAsync<ClientRequestException>())
                .Where(x => x.IsHttpError);
        }

        [Test]
        public async Task PostAsync_ServiceReturnsBadRequestWithError_ThrowClientRequestException()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = ResponseApiMockFactory.MockPostMethodWithBadRequestAndError(entity);

            (await NClientGallery.Clients.GetBasic().For<IResponseClientWithMetadata>(api.Urls.First()).Build()
                    .Invoking(async x => await x.PostAsync(entity))
                    .Should()
                    .ThrowAsync<ClientRequestException>())
                .Where(x => x.IsHttpError);
        }

        [Test]
        public async Task PostAsync_NotWorkingService_ThrowClientRequestException()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = ResponseApiMockFactory.MockInternalServerError();

            (await NClientGallery.Clients.GetBasic().For<IResponseClientWithMetadata>(api.Urls.First()).Build()
                    .Invoking(async x => await x.PostAsync(entity))
                    .Should()
                    .ThrowAsync<ClientRequestException>())
                .Where(x => x.IsHttpError);
        }

        [Test]
        public async Task PostResponseAsync_ServiceReturnsOk_IntInBody()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = ResponseApiMockFactory.MockPostMethod(entity);

            var result = await NClientGallery.Clients.GetBasic().For<IResponseClientWithMetadata>(api.Urls.First()).Build()
                .PostResponseAsync(entity);

            result.Should().NotBeNull();
            using var assertionScope = new AssertionScope();
            result.StatusCode.Should().Be((int) HttpStatusCode.OK);
        }

        [Test]
        public async Task PostResponseAsync_ServiceReturnsBadRequest_ResponseWithBadRequestStatus()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = ResponseApiMockFactory.MockPostMethodWithBadRequest(entity);

            var result = await NClientGallery.Clients.GetBasic().For<IResponseClientWithMetadata>(api.Urls.First()).Build()
                .PostResponseAsync(entity);

            result.Should().NotBeNull();
            using var assertionScope = new AssertionScope();
            result.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task PostResponseAsync_ServiceReturnsBadRequestWithError_ResponseWithBadRequestStatusAndContent()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = ResponseApiMockFactory.MockPostMethodWithBadRequestAndError(entity);

            var result = await NClientGallery.Clients.GetBasic().For<IResponseClientWithMetadata>(api.Urls.First()).Build()
                .PostResponseAsync(entity);

            result.Should().NotBeNull();
            using var assertionScope = new AssertionScope();
            result.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);
            result.Content.ToString().Should().BeEquivalentTo(JsonSerializer.Serialize(BadRequestError));
        }

        [Test]
        public async Task PostResponseAsync_NotWorkingService_ResponseWithInternalServerStatus()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = ResponseApiMockFactory.MockInternalServerError();

            var result = await NClientGallery.Clients.GetBasic().For<IResponseClientWithMetadata>(api.Urls.First()).Build()
                .PostResponseAsync(entity);

            result.Should().NotBeNull();
            using var assertionScope = new AssertionScope();
            result.StatusCode.Should().Be((int) HttpStatusCode.InternalServerError);
        }

        [Test]
        public async Task PostResponseWithErrorAsync_ServiceReturnsOk_IntInBody()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = ResponseApiMockFactory.MockPostMethod(entity);

            var result = await NClientGallery.Clients.GetBasic().For<IResponseClientWithMetadata>(api.Urls.First()).Build()
                .PostResponseWithErrorAsync(entity);

            result.Should().NotBeNull();
            using var assertionScope = new AssertionScope();
            result.StatusCode.Should().Be((int) HttpStatusCode.OK);
            result.Error.Should().BeNull();
        }

        [Test]
        public async Task PostResponseWithErrorAsync_ServiceReturnsBadRequest_ThrowClientRequestException()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = ResponseApiMockFactory.MockPostMethodWithBadRequest(entity);

            (await NClientGallery.Clients.GetBasic().For<IResponseClientWithMetadata>(api.Urls.First()).Build()
                    .Invoking(async x => await x.PostResponseWithErrorAsync(entity))
                    .Should()
                    .ThrowAsync<ClientRequestException>())
                .Where(x => x.IsHttpError == false);
        }

        [Test]
        public async Task PostResponseWithErrorAsync_ServiceReturnsBadRequestWithError_ResponseWithBadRequestStatusAndError()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = ResponseApiMockFactory.MockPostMethodWithBadRequestAndError(entity);

            var result = await NClientGallery.Clients.GetBasic().For<IResponseClientWithMetadata>(api.Urls.First()).Build()
                .PostResponseWithErrorAsync(entity);

            result.Should().NotBeNull();
            using var assertionScope = new AssertionScope();
            result.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);
            result.Error.Should().BeEquivalentTo(BadRequestError);
        }

        [Test]
        public async Task PostResponseWithErrorAsync_NotWorkingService_ResponseWithInternalServerStatusAndError()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = ResponseApiMockFactory.MockInternalServerError();

            var result = await NClientGallery.Clients.GetBasic().For<IResponseClientWithMetadata>(api.Urls.First()).Build()
                .PostResponseWithErrorAsync(entity);

            result.Should().NotBeNull();
            using var assertionScope = new AssertionScope();
            result.StatusCode.Should().Be((int) HttpStatusCode.InternalServerError);
            result.Error.Should().BeEquivalentTo(InternalServerError);
        }
    }
}
