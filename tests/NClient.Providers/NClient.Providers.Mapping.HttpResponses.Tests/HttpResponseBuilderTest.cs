using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Language.Flow;
using NClient.Providers.Results.HttpResults;
using NClient.Providers.Serialization;
using NClient.Providers.Transport;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.Providers.Mapping.HttpResponses.Tests
{
    [Parallelizable]
    public class HttpResponseBuilderTest
    {
        private ResponseToHttpResponseMapper _responseToHttpResponseMapper = null!;
        private Mock<ISerializer> _serializerMock = null!;
        private ISetup<ISerializer, object?> _serializerMockSetup = null!;

        [SetUp]
        public void SetUp()
        {
            var loggerMock = new Mock<ILogger>();
            _serializerMock = new Mock<ISerializer>();
            _serializerMockSetup = _serializerMock.Setup(x => x.Deserialize(It.IsAny<string>(), It.IsAny<Type>()));
            _responseToHttpResponseMapper = new ResponseToHttpResponseMapper(new Toolset(_serializerMock.Object, loggerMock.Object));
        }
        
        [Test]
        public async Task Build_SuccessHttpResponse_HttpResponse()
        {
            var httpRequestMessage = new HttpRequestMessage();
            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StreamContent(new MemoryStream()),
                StatusCode = HttpStatusCode.OK
            };
            var responseContext = new ResponseContext<HttpRequestMessage, HttpResponseMessage>(httpRequestMessage, httpResponseMessage);
            var expectedData = new BasicEntity { Id = 1, Value = 2 };
            _serializerMockSetup.Returns(expectedData);

            var actualResult = await _responseToHttpResponseMapper.MapAsync(
                typeof(HttpResponse), responseContext, CancellationToken.None);
            
            actualResult.Should().BeOfType<HttpResponse>();
            ((HttpResponse) actualResult!).StatusCode.Should().Be(httpResponseMessage.StatusCode);
        }
        
        [Test]
        public async Task Build_SuccessHttpResponse_IHttpResponse()
        {
            var httpRequestMessage = new HttpRequestMessage();
            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StreamContent(new MemoryStream()),
                StatusCode = HttpStatusCode.OK
            };
            var responseContext = new ResponseContext<HttpRequestMessage, HttpResponseMessage>(httpRequestMessage, httpResponseMessage);
            var expectedData = new BasicEntity { Id = 1, Value = 2 };
            _serializerMockSetup.Returns(expectedData);

            var actualResult = await _responseToHttpResponseMapper.MapAsync(
                typeof(IHttpResponse), responseContext, CancellationToken.None);
            
            actualResult.Should().BeOfType<HttpResponse>();
            ((HttpResponse) actualResult!).StatusCode.Should().Be(httpResponseMessage.StatusCode);
        }
        
        [Test]
        public async Task Build_FailureHttpResponse_HttpResponse()
        {
            var httpRequestMessage = new HttpRequestMessage();
            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StreamContent(new MemoryStream()),
                StatusCode = HttpStatusCode.NotFound
            };
            var responseContext = new ResponseContext<HttpRequestMessage, HttpResponseMessage>(httpRequestMessage, httpResponseMessage);

            var actualResult = await _responseToHttpResponseMapper.MapAsync(
                typeof(HttpResponse), responseContext, CancellationToken.None);
            
            actualResult.Should().BeOfType<HttpResponse>();
            ((HttpResponse) actualResult!).StatusCode.Should().Be(httpResponseMessage.StatusCode);
        }
        
        [Test]
        public async Task Build_SuccessHttpResponse_HttpResponseWithData()
        {
            var httpRequestMessage = new HttpRequestMessage();
            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StreamContent(new MemoryStream()),
                StatusCode = HttpStatusCode.OK
            };
            var responseContext = new ResponseContext<HttpRequestMessage, HttpResponseMessage>(httpRequestMessage, httpResponseMessage);
            var expectedData = new BasicEntity { Id = 1, Value = 2 };
            _serializerMockSetup.Returns(expectedData);

            var actualResult = await _responseToHttpResponseMapper.MapAsync(
                typeof(HttpResponse<BasicEntity>), responseContext, CancellationToken.None);
            
            actualResult.Should().BeOfType<HttpResponse<BasicEntity>>();
            ((HttpResponse<BasicEntity>) actualResult!).StatusCode.Should().Be(httpResponseMessage.StatusCode);
            ((HttpResponse<BasicEntity>) actualResult).Data.Should().Be(expectedData);
        }

        [Test]
        public async Task Build_SuccessHttpResponse_IHttpResponseWithData()
        {
            var httpRequestMessage = new HttpRequestMessage();
            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StreamContent(new MemoryStream()),
                StatusCode = HttpStatusCode.OK
            };
            var responseContext = new ResponseContext<HttpRequestMessage, HttpResponseMessage>(httpRequestMessage, httpResponseMessage);
            var expectedData = new BasicEntity { Id = 1, Value = 2 };
            _serializerMockSetup.Returns(expectedData);

            var actualResult = await _responseToHttpResponseMapper.MapAsync(
                typeof(IHttpResponse<BasicEntity>), responseContext, CancellationToken.None);
            
            actualResult.Should().BeOfType<HttpResponse<BasicEntity>>();
            ((HttpResponse<BasicEntity>) actualResult!).StatusCode.Should().Be(httpResponseMessage.StatusCode);
            ((HttpResponse<BasicEntity>) actualResult).Data.Should().Be(expectedData);
        }
        
        [Test]
        public async Task Build_FailureHttpResponse_HttpResponseWithData()
        {
            var httpRequestMessage = new HttpRequestMessage();
            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StreamContent(new MemoryStream()),
                StatusCode = HttpStatusCode.NotFound
            };
            var responseContext = new ResponseContext<HttpRequestMessage, HttpResponseMessage>(httpRequestMessage, httpResponseMessage);

            var actualResult = await _responseToHttpResponseMapper.MapAsync(
                typeof(HttpResponse<BasicEntity>), responseContext, CancellationToken.None);
            
            actualResult.Should().BeOfType<HttpResponse<BasicEntity>>();
            ((HttpResponse<BasicEntity>) actualResult!).StatusCode.Should().Be(httpResponseMessage.StatusCode);
            ((HttpResponse<BasicEntity>) actualResult).Data.Should().BeNull();
        }
        
        [Test]
        public async Task Build_SuccessHttpResponse_HttpResponseWithError()
        {
            var httpRequestMessage = new HttpRequestMessage();
            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StreamContent(new MemoryStream()),
                StatusCode = HttpStatusCode.OK
            };
            var responseContext = new ResponseContext<HttpRequestMessage, HttpResponseMessage>(httpRequestMessage, httpResponseMessage);

            var actualResult = await _responseToHttpResponseMapper.MapAsync(
                typeof(HttpResponseWithError<string>), responseContext, CancellationToken.None);
            
            actualResult.Should().BeOfType<HttpResponseWithError<string>>();
            ((HttpResponseWithError<string>) actualResult!).StatusCode.Should().Be(httpResponseMessage.StatusCode);
            ((HttpResponseWithError<string>) actualResult).Error.Should().BeNull();
        }

        [Test]
        public async Task Build_SuccessHttpResponse_IHttpResponseWithError()
        {
            var httpRequestMessage = new HttpRequestMessage();
            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StreamContent(new MemoryStream()),
                StatusCode = HttpStatusCode.OK
            };
            var responseContext = new ResponseContext<HttpRequestMessage, HttpResponseMessage>(httpRequestMessage, httpResponseMessage);

            var actualResult = await _responseToHttpResponseMapper.MapAsync(
                typeof(IHttpResponseWithError<string>), responseContext, CancellationToken.None);
            
            actualResult.Should().BeOfType<HttpResponseWithError<string>>();
            ((HttpResponseWithError<string>) actualResult!).StatusCode.Should().Be(httpResponseMessage.StatusCode);
            ((HttpResponseWithError<string>) actualResult).Error.Should().BeNull();
        }
        
        [Test]
        public async Task Build_FailureHttpResponse_HttpResponseWithError()
        {
            var httpRequestMessage = new HttpRequestMessage();
            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StreamContent(new MemoryStream()),
                StatusCode = HttpStatusCode.NotFound
            };
            var responseContext = new ResponseContext<HttpRequestMessage, HttpResponseMessage>(httpRequestMessage, httpResponseMessage);
            const string expectedError = "error";
            _serializerMockSetup.Returns(expectedError);

            var actualResult = await _responseToHttpResponseMapper.MapAsync(
                typeof(HttpResponseWithError<string>), responseContext, CancellationToken.None);
            
            actualResult.Should().BeOfType<HttpResponseWithError<string>>();
            ((HttpResponseWithError<string>) actualResult!).StatusCode.Should().Be(httpResponseMessage.StatusCode);
            ((HttpResponseWithError<string>) actualResult).Error.Should().Be(expectedError);
        }
        
        [Test]
        public async Task Build_SuccessHttpResponse_HttpResponseWithDataOrError()
        {
            var httpRequestMessage = new HttpRequestMessage();
            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StreamContent(new MemoryStream()),
                StatusCode = HttpStatusCode.OK
            };
            var responseContext = new ResponseContext<HttpRequestMessage, HttpResponseMessage>(httpRequestMessage, httpResponseMessage);
            var expectedData = new BasicEntity { Id = 1, Value = 2 };
            _serializerMockSetup.Returns(expectedData);
            
            var actualResult = await _responseToHttpResponseMapper.MapAsync(
                typeof(HttpResponseWithError<BasicEntity, string>), responseContext, CancellationToken.None);
            
            actualResult.Should().BeOfType<HttpResponseWithError<BasicEntity, string>>();
            ((HttpResponseWithError<BasicEntity, string>) actualResult!).StatusCode.Should().Be(httpResponseMessage.StatusCode);
            ((HttpResponseWithError<BasicEntity, string>) actualResult).Data.Should().Be(expectedData);
            ((HttpResponseWithError<BasicEntity, string>) actualResult).Error.Should().BeNull();
        }

        [Test]
        public async Task Build_SuccessHttpResponse_IHttpResponseWithDataOrError()
        {
            var httpRequestMessage = new HttpRequestMessage();
            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StreamContent(new MemoryStream()),
                StatusCode = HttpStatusCode.OK
            };
            var responseContext = new ResponseContext<HttpRequestMessage, HttpResponseMessage>(httpRequestMessage, httpResponseMessage);
            var expectedData = new BasicEntity { Id = 1, Value = 2 };
            _serializerMockSetup.Returns(expectedData);
            
            var actualResult = await _responseToHttpResponseMapper.MapAsync(
                typeof(IHttpResponseWithError<BasicEntity, string>), responseContext, CancellationToken.None);
            
            actualResult.Should().BeOfType<HttpResponseWithError<BasicEntity, string>>();
            ((HttpResponseWithError<BasicEntity, string>) actualResult!).StatusCode.Should().Be(httpResponseMessage.StatusCode);
            ((HttpResponseWithError<BasicEntity, string>) actualResult).Data.Should().Be(expectedData);
            ((HttpResponseWithError<BasicEntity, string>) actualResult).Error.Should().BeNull();
        }
        
        [Test]
        public async Task Build_FailureHttpResponse_HttpResponseWithDataOrError()
        {
            var httpRequestMessage = new HttpRequestMessage();
            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StreamContent(new MemoryStream()),
                StatusCode = HttpStatusCode.NotFound
            };
            var responseContext = new ResponseContext<HttpRequestMessage, HttpResponseMessage>(httpRequestMessage, httpResponseMessage);
            const string expectedError = "error";
            _serializerMockSetup.Returns(expectedError);

            var actualResult = await _responseToHttpResponseMapper.MapAsync(
                typeof(HttpResponseWithError<BasicEntity, string>), responseContext, CancellationToken.None);
            
            actualResult.Should().BeOfType<HttpResponseWithError<BasicEntity, string>>();
            ((HttpResponseWithError<BasicEntity, string>) actualResult!).StatusCode.Should().Be(httpResponseMessage.StatusCode);
            ((HttpResponseWithError<BasicEntity, string>) actualResult).Error.Should().Be(expectedError);
        }
    }
}
