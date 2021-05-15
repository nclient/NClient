using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using NClient.Core.Exceptions;
using NClient.Extensions;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Entities;
using NClient.Tests.Clients;
using NUnit.Framework;

namespace NClient.Tests.InterfaceBasedClientTests
{
    [Parallelizable]
    public class ResponseClientTest
    {
        private IResponseClientWithMetadata _responseClient = null!;
        private ResponseApiMockFactory _responseApiMockFactory = null!;

        [SetUp]
        public void Setup()
        {
            _responseApiMockFactory = new ResponseApiMockFactory(port: 5017);

            _responseClient = new NClientBuilder()
                .Use<IResponseClientWithMetadata>(_responseApiMockFactory.ApiUri.ToString())
                .Build();
        }

        [Test]
        public async Task ResponseClient_GetAsync_IntInBody()
        {
            const int id = 1;
            using var api = _responseApiMockFactory.MockGetMethod(id);

            var result = await _responseClient.GetAsync(id);
            
            result.Should().Be(id);
        }
        
        [Test]
        public async Task ResponseClient_GetAsyncWithBadRequest_ThrowHttpRequestNClientException()
        {
            const int id = 1;
            using var api = _responseApiMockFactory.MockGetMethodWithBadRequest(id);

            await _responseClient
                .Invoking(async x => await x.GetAsync(id))
                .Should()
                .ThrowAsync<HttpRequestNClientException>();
        }
        
        [Test]
        public async Task ResponseClient_GetAsyncToNotWorkingService_ThrowHttpRequestNClientException()
        {
            const int id = 1;
            using var api = _responseApiMockFactory.MockInternalServerError();

            await _responseClient
                .Invoking(async x => await x.GetAsync(id))
                .Should()
                .ThrowAsync<HttpRequestNClientException>();
        }
        
        [Test]
        public async Task ResponseClient_GetResponseAsync_IntInBody()
        {
            const int id = 1;
            using var api = _responseApiMockFactory.MockGetMethod(id);

            var result = await _responseClient.GetResponseAsync(id);
            
            result.Should().NotBeNull();
            using var assertionScope = new AssertionScope();
            result.Value.Should().Be(id);
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        [Test]
        public async Task ResponseClient_GetResponseAsyncWithBadRequest_ResponseWithBadRequestStatus()
        {
            const int id = 1;
            using var api = _responseApiMockFactory.MockGetMethodWithBadRequest(id);

            var result = await _responseClient.GetResponseAsync(id);
            
            result.Should().NotBeNull();
            using var assertionScope = new AssertionScope();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        
        [Test]
        public async Task ResponseClient_GetResponseAsyncToNotWorkingService_ResponseWithInternalServerErrorStatus()
        {
            const int id = 1;
            using var api = _responseApiMockFactory.MockInternalServerError();

            var result = await _responseClient.GetResponseAsync(id);
            
            result.Should().NotBeNull();
            using var assertionScope = new AssertionScope();
            result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Test]
        public async Task ResponseClient_PostAsync_NotThrow()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _responseApiMockFactory.MockPostMethod(entity);

            await _responseClient
                .Invoking(async x => await x.PostAsync(entity))
                .Should()
                .NotThrowAsync();
        }
        
        [Test]
        public async Task ResponseClient_PostAsyncWithBadRequest_ThrowHttpRequestNClientException()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _responseApiMockFactory.MockPostMethodWithBadRequest(entity);

            await _responseClient
                .Invoking(async x => await x.PostAsync(entity))
                .Should()
                .ThrowAsync<HttpRequestNClientException>();
        }
        
        [Test]
        public async Task ResponseClient_PostAsyncToNotWorkingService_ThrowHttpRequestNClientException()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _responseApiMockFactory.MockInternalServerError();

            await _responseClient
                .Invoking(async x => await x.PostAsync(entity))
                .Should()
                .ThrowAsync<HttpRequestNClientException>();
        }
        
        [Test]
        public async Task ResponseClient_PostResponseAsync_IntInBody()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _responseApiMockFactory.MockPostMethod(entity);

            var result = await _responseClient.PostResponseAsync(entity);
            
            result.Should().NotBeNull();
            using var assertionScope = new AssertionScope();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        [Test]
        public async Task ResponseClient_PostResponseAsyncWithBadRequest_ResponseWithBadRequestStatus()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _responseApiMockFactory.MockPostMethodWithBadRequest(entity);

            var result = await _responseClient.PostResponseAsync(entity);
            
            result.Should().NotBeNull();
            using var assertionScope = new AssertionScope();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        
        [Test]
        public async Task ResponseClient_PostResponseAsyncToNotWorkingService_ResponseWithInternalServerErrorStatus()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _responseApiMockFactory.MockInternalServerError();

            var result = await _responseClient.PostResponseAsync(entity);
            
            result.Should().NotBeNull();
            using var assertionScope = new AssertionScope();
            result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }
    }
}
