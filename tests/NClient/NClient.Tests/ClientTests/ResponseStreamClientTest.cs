using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using NClient.Common.Helpers;
using NClient.Providers.Transport;
using NClient.Standalone.Tests.Clients;
using NClient.Testing.Common.Apis;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class ResponseStreamClientTest
    {
        #if NETFRAMEWORK
        private const string TransportStreamTypeName = "ReadOnlyStream";
        #else
        private const string TransportStreamTypeName = "ChunkedEncodingReadStream";
        #endif
        private const string MemoryStreamTypeName = nameof(MemoryStream);

        [Test]
        public async Task GetResponseAsync_ResponseWithSuccessCode_ChunkedEncodingReadStream()
        {
            const int id = 1;
            using var api = ResponseStreamApiMockFactory.MockSuccessGetMethod(id);

            var response = await NClientGallery.Clients.GetRest().For<IResponseStreamClientWithMetaData>(api.Urls.First()).Build()
                .GetResponseAsync();

            await AssertSuccessResponseAsync(response, TransportStreamTypeName, id.ToString());
        }
        
        [Test]
        public async Task GetResponseAsync_ResponseWithFailureCode_ChunkedEncodingReadStream()
        {
            const HttpStatusCode errorCode = HttpStatusCode.NotFound;
            using var api = ResponseStreamApiMockFactory.MockFailureGetMethod(HttpStatusCode.NotFound);

            var response = await NClientGallery.Clients.GetRest().For<IResponseStreamClientWithMetaData>(api.Urls.First()).Build()
                .GetResponseAsync();

            await AssertFailureResponseAsync(response, TransportStreamTypeName, errorCode);
        }
        
        [Test]
        public async Task GetResponseWithDataAsync_ResponseWithSuccessCode_MemoryStream()
        {
            const int id = 1;
            using var api = ResponseStreamApiMockFactory.MockSuccessGetMethod(id);

            var response = await NClientGallery.Clients.GetRest().For<IResponseStreamClientWithMetaData>(api.Urls.First()).Build()
                .GetResponseWithDataAsync();

            await AssertSuccessResponseAsync(response, MemoryStreamTypeName, id.ToString());
        }
        
        [Test]
        public async Task GetResponseWithDataAsync_ResponseWithFailureCode_MemoryStream()
        {
            const HttpStatusCode errorCode = HttpStatusCode.NotFound;
            using var api = ResponseStreamApiMockFactory.MockFailureGetMethod(errorCode);

            var response = await NClientGallery.Clients.GetRest().For<IResponseStreamClientWithMetaData>(api.Urls.First()).Build()
                .GetResponseWithDataAsync();

            await AssertFailureResponseAsync(response, MemoryStreamTypeName, errorCode);
        }
        
        [Test]
        public async Task GetResponseWithErrorAsync_ResponseWithSuccessCode_MemoryStream()
        {
            const int id = 1;
            using var api = ResponseStreamApiMockFactory.MockSuccessGetMethod(id);

            var response = await NClientGallery.Clients.GetRest().For<IResponseStreamClientWithMetaData>(api.Urls.First()).Build()
                .GetResponseWithErrorAsync();

            await AssertSuccessResponseAsync(response, MemoryStreamTypeName, id.ToString());
        }
        
        [Test]
        public async Task GetResponseWithErrorAsync_ResponseWithFailureCode_MemoryStream()
        {
            const HttpStatusCode errorCode = HttpStatusCode.NotFound;
            using var api = ResponseStreamApiMockFactory.MockFailureGetMethod(errorCode);

            var response = await NClientGallery.Clients.GetRest().For<IResponseStreamClientWithMetaData>(api.Urls.First()).Build()
                .GetResponseWithErrorAsync();

            await AssertFailureResponseAsync(response, MemoryStreamTypeName, errorCode);
        }
        
        [Test]
        public async Task GetResponseWithDataOrErrorAsync_ResponseWithSuccessCode_MemoryStream()
        {
            const int id = 1;
            using var api = ResponseStreamApiMockFactory.MockSuccessGetMethod(id);

            var response = await NClientGallery.Clients.GetRest().For<IResponseStreamClientWithMetaData>(api.Urls.First()).Build()
                .GetResponseWithDataOrErrorAsync();

            await AssertSuccessResponseAsync(response, MemoryStreamTypeName, id.ToString());
        }
        
        [Test]
        public async Task GetResponseWithDataOrErrorAsync_ResponseWithFailureCode_MemoryStream()
        {
            const HttpStatusCode errorCode = HttpStatusCode.NotFound;
            using var api = ResponseStreamApiMockFactory.MockFailureGetMethod(errorCode);

            var response = await NClientGallery.Clients.GetRest().For<IResponseStreamClientWithMetaData>(api.Urls.First()).Build()
                .GetResponseWithDataOrErrorAsync();

            await AssertFailureResponseAsync(response, MemoryStreamTypeName, errorCode);
        }

        private static async Task AssertSuccessResponseAsync(IResponse response, string streamTypeName, string stringContent)
        {
            using var assertionScope = new AssertionScope();
            response.StatusCode.Should().Be((int) HttpStatusCode.OK);
            response.Content.Stream.GetType().Name.Should().Be(streamTypeName);
            response.Content.Encoding.Should().Be(Encoding.UTF8);
            if (streamTypeName == MemoryStreamTypeName)
                response.Content.Stream.Position.Should().Be(0);
            (await response.Content.Stream.ReadToEndAsync(response.Content.Encoding)).Should().Be(stringContent);
        }
        
        private static async Task AssertFailureResponseAsync(IResponse response, string streamTypeName, HttpStatusCode errorCode)
        {
            using var assertionScope = new AssertionScope();
            response.StatusCode.Should().Be((int) errorCode);
            response.Content.Stream.GetType().Name.Should().Be(streamTypeName);
            response.Content.Encoding.Should().Be(Encoding.UTF8);
            if (streamTypeName == MemoryStreamTypeName)
                response.Content.Stream.Position.Should().Be(0);
            (await response.Content.Stream.ReadToEndAsync(response.Content.Encoding)).Should().Be($"\"{errorCode.ToString()}\"");
        }
    }
}
