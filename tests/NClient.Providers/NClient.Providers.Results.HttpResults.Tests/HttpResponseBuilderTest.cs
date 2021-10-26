using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using NClient.Providers.Serialization;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.Providers.Results.HttpResults.Tests
{
    [Parallelizable]
    public class HttpResponseBuilderTest
    {
        private HttpResponseBuilder _httpResponseBuilder = null!;
        private Mock<ISerializer> _serializerMock = null!;
        private ISetup<ISerializer, object?> _serializerMockSetup = null!;

        [SetUp]
        public void SetUp()
        {
            _httpResponseBuilder = new HttpResponseBuilder();
            _serializerMock = new Mock<ISerializer>();
            _serializerMockSetup = _serializerMock.Setup(x => x.Deserialize(It.IsAny<string>(), It.IsAny<Type>()));
        }
        
        [Test]
        public async Task Build_SuccessHttpResponse_HttpResponse()
        {
            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StreamContent(new MemoryStream()),
                StatusCode = HttpStatusCode.OK
            };
            var expectedData = new BasicEntity { Id = 1, Value = 2 };
            _serializerMockSetup.Returns(expectedData);

            var actualResult = await _httpResponseBuilder.BuildAsync(typeof(HttpResponse), httpResponseMessage, _serializerMock.Object);
            
            actualResult.Should().BeOfType<HttpResponse>();
            ((HttpResponse)actualResult!).StatusCode.Should().Be(httpResponseMessage.StatusCode);
        }
        
        [Test]
        public async Task Build_SuccessHttpResponse_IHttpResponse()
        {
            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StreamContent(new MemoryStream()),
                StatusCode = HttpStatusCode.OK
            };
            var expectedData = new BasicEntity { Id = 1, Value = 2 };
            _serializerMockSetup.Returns(expectedData);

            var actualResult = await _httpResponseBuilder.BuildAsync(typeof(IHttpResponse), httpResponseMessage, _serializerMock.Object);
            
            actualResult.Should().BeOfType<HttpResponse>();
            ((HttpResponse)actualResult!).StatusCode.Should().Be(httpResponseMessage.StatusCode);
        }
        
        [Test]
        public async Task Build_FailureHttpResponse_HttpResponse()
        {
            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StreamContent(new MemoryStream()),
                StatusCode = HttpStatusCode.NotFound
            };

            var actualResult = await _httpResponseBuilder.BuildAsync(typeof(HttpResponse), httpResponseMessage, _serializerMock.Object);
            
            actualResult.Should().BeOfType<HttpResponse>();
            ((HttpResponse)actualResult!).StatusCode.Should().Be(httpResponseMessage.StatusCode);
        }
        
        [Test]
        public async Task Build_SuccessHttpResponse_HttpResponseWithData()
        {
            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StreamContent(new MemoryStream()),
                StatusCode = HttpStatusCode.OK
            };
            var expectedData = new BasicEntity { Id = 1, Value = 2 };
            _serializerMockSetup.Returns(expectedData);

            var actualResult = await _httpResponseBuilder.BuildAsync(typeof(HttpResponse<BasicEntity>), httpResponseMessage, _serializerMock.Object);
            
            actualResult.Should().BeOfType<HttpResponse<BasicEntity>>();
            ((HttpResponse<BasicEntity>)actualResult!).StatusCode.Should().Be(httpResponseMessage.StatusCode);
            ((HttpResponse<BasicEntity>)actualResult!).Data.Should().Be(expectedData);
        }

        [Test]
        public async Task Build_SuccessHttpResponse_IHttpResponseWithData()
        {
            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StreamContent(new MemoryStream()),
                StatusCode = HttpStatusCode.OK
            };
            var expectedData = new BasicEntity { Id = 1, Value = 2 };
            _serializerMockSetup.Returns(expectedData);

            var actualResult = await _httpResponseBuilder.BuildAsync(typeof(IHttpResponse<BasicEntity>), httpResponseMessage, _serializerMock.Object);
            
            actualResult.Should().BeOfType<HttpResponse<BasicEntity>>();
            ((HttpResponse<BasicEntity>)actualResult!).StatusCode.Should().Be(httpResponseMessage.StatusCode);
            ((HttpResponse<BasicEntity>)actualResult!).Data.Should().Be(expectedData);
        }
        
        [Test]
        public async Task Build_FailureHttpResponse_HttpResponseWithData()
        {
            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StreamContent(new MemoryStream()),
                StatusCode = HttpStatusCode.NotFound
            };

            var actualResult = await _httpResponseBuilder.BuildAsync(typeof(HttpResponse<BasicEntity>), httpResponseMessage, _serializerMock.Object);
            
            actualResult.Should().BeOfType<HttpResponse<BasicEntity>>();
            ((HttpResponse<BasicEntity>)actualResult!).StatusCode.Should().Be(httpResponseMessage.StatusCode);
            ((HttpResponse<BasicEntity>)actualResult!).Data.Should().BeNull();
        }
        
        [Test]
        public async Task Build_SuccessHttpResponse_HttpResponseWithError()
        {
            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StreamContent(new MemoryStream()),
                StatusCode = HttpStatusCode.OK
            };

            var actualResult = await _httpResponseBuilder.BuildAsync(typeof(HttpResponseWithError<string>), httpResponseMessage, _serializerMock.Object);
            
            actualResult.Should().BeOfType<HttpResponseWithError<string>>();
            ((HttpResponseWithError<string>)actualResult!).StatusCode.Should().Be(httpResponseMessage.StatusCode);
            ((HttpResponseWithError<string>)actualResult!).Error.Should().BeNull();
        }

        [Test]
        public async Task Build_SuccessHttpResponse_IHttpResponseWithError()
        {
            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StreamContent(new MemoryStream()),
                StatusCode = HttpStatusCode.OK
            };

            var actualResult = await _httpResponseBuilder.BuildAsync(typeof(IHttpResponseWithError<string>), httpResponseMessage, _serializerMock.Object);
            
            actualResult.Should().BeOfType<HttpResponseWithError<string>>();
            ((HttpResponseWithError<string>)actualResult!).StatusCode.Should().Be(httpResponseMessage.StatusCode);
            ((HttpResponseWithError<string>)actualResult!).Error.Should().BeNull();
        }
        
        [Test]
        public async Task Build_FailureHttpResponse_HttpResponseWithError()
        {
            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StreamContent(new MemoryStream()),
                StatusCode = HttpStatusCode.NotFound
            };
            const string expectedError = "error";
            _serializerMockSetup.Returns(expectedError);

            var actualResult = await _httpResponseBuilder.BuildAsync(typeof(HttpResponseWithError<string>), httpResponseMessage, _serializerMock.Object);
            
            actualResult.Should().BeOfType<HttpResponseWithError<string>>();
            ((HttpResponseWithError<string>)actualResult!).StatusCode.Should().Be(httpResponseMessage.StatusCode);
            ((HttpResponseWithError<string>)actualResult!).Error.Should().Be(expectedError);
        }
        
        [Test]
        public async Task Build_SuccessHttpResponse_HttpResponseWithDataAndError()
        {
            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StreamContent(new MemoryStream()),
                StatusCode = HttpStatusCode.OK
            };
            var expectedData = new BasicEntity { Id = 1, Value = 2 };
            _serializerMockSetup.Returns(expectedData);
            
            var actualResult = await _httpResponseBuilder.BuildAsync(typeof(HttpResponseWithError<BasicEntity, string>), httpResponseMessage, _serializerMock.Object);
            
            actualResult.Should().BeOfType<HttpResponseWithError<BasicEntity, string>>();
            ((HttpResponseWithError<BasicEntity, string>)actualResult!).StatusCode.Should().Be(httpResponseMessage.StatusCode);
            ((HttpResponseWithError<BasicEntity, string>)actualResult!).Data.Should().Be(expectedData);
            ((HttpResponseWithError<BasicEntity, string>)actualResult!).Error.Should().BeNull();
        }

        [Test]
        public async Task Build_SuccessHttpResponse_IHttpResponseWithDataAndError()
        {
            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StreamContent(new MemoryStream()),
                StatusCode = HttpStatusCode.OK
            };
            var expectedData = new BasicEntity { Id = 1, Value = 2 };
            _serializerMockSetup.Returns(expectedData);
            
            var actualResult = await _httpResponseBuilder.BuildAsync(typeof(IHttpResponseWithError<BasicEntity, string>), httpResponseMessage, _serializerMock.Object);
            
            actualResult.Should().BeOfType<HttpResponseWithError<BasicEntity, string>>();
            ((HttpResponseWithError<BasicEntity, string>)actualResult!).StatusCode.Should().Be(httpResponseMessage.StatusCode);
            ((HttpResponseWithError<BasicEntity, string>)actualResult!).Data.Should().Be(expectedData);
            ((HttpResponseWithError<BasicEntity, string>)actualResult!).Error.Should().BeNull();
        }
        
        [Test]
        public async Task Build_FailureHttpResponse_HttpResponseWithDataAndError()
        {
            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StreamContent(new MemoryStream()),
                StatusCode = HttpStatusCode.NotFound
            };
            const string expectedError = "error";
            _serializerMockSetup.Returns(expectedError);

            var actualResult = await _httpResponseBuilder.BuildAsync(typeof(HttpResponseWithError<BasicEntity, string>), httpResponseMessage, _serializerMock.Object);
            
            actualResult.Should().BeOfType<HttpResponseWithError<BasicEntity, string>>();
            ((HttpResponseWithError<BasicEntity, string>)actualResult!).StatusCode.Should().Be(httpResponseMessage.StatusCode);
            ((HttpResponseWithError<BasicEntity, string>)actualResult!).Error.Should().Be(expectedError);
        }
    }
}
