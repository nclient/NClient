using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NClient.Common.Helpers;
using NClient.Providers.Serialization;
using NClient.Providers.Serialization.SystemTextJson;
using NClient.Testing.Common.Entities;
using NClient.Testing.Common.Helpers;
using NUnit.Framework;
using WireMock.Matchers;
using WireMock.Server;

namespace NClient.Providers.Transport.SystemNetHttp.Tests
{
    [Parallelizable]
    public class SystemNetHttpClientTest
    {
        private const string Path = "/api/method";
        private static readonly Guid RequestId = Guid.Parse("55df3bb2-a254-4beb-87a8-70e18b74d995");
        private static readonly BasicEntity Data = new() { Id = 1, Value = 2 };
        private static readonly Mock<ILogger> LoggerMock = new();
        private static readonly ISerializer Serializer = new SystemTextJsonSerializerProvider().Create(LoggerMock.Object);
        private static readonly Metadata AcceptHeader = new("Accept", "application/json");
        private static readonly Metadata ServerHeader = new("Server", "Kestrel");
        private static readonly Metadata EmptyContentLengthMetadata = new("Content-Length", "0");
        private static readonly Metadata ContentEncodingHeader = new("Content-Encoding", "UTF-8");
        private static readonly Metadata ContentTypeHeader = new("Content-Type", "application/json");
        
        public static readonly IEnumerable ValidTestCases = new[]
        {
            ExecutionHeadRequestTestCase(),
            ExecutionGetRequestTestCase(),
            ExecutionPostRequestTestCase()
        };
        
        [TestCaseSource(nameof(ValidTestCases))]
        public async Task Test(Func<Uri, IRequest> requestFactory, Func<Uri, IResponse> expectedResponseFactory, Lazy<IWireMockServer> serverFactory)
        {
            using var server = serverFactory.Value;
            var toolset = new Toolset(Serializer, LoggerMock.Object);
            var transport = new SystemNetHttpTransportProvider().Create(toolset);
            var transportRequestBuilder = new SystemNetHttpTransportRequestBuilderProvider().Create(toolset);
            var responseBuilder = new SystemNetHttpResponseBuilderProvider().Create(toolset);
            var request = requestFactory.Invoke(new Uri(server.Urls.First()));
            var expectedResponse = expectedResponseFactory.Invoke(new Uri(server.Urls.First()));

            var httpRequestMessage = await transportRequestBuilder.BuildAsync(request, CancellationToken.None);
            var httpResponseMessage = await transport.ExecuteAsync(httpRequestMessage, CancellationToken.None);
            var response = await responseBuilder.BuildAsync(request, new ResponseContext<HttpRequestMessage, 
                HttpResponseMessage>(httpRequestMessage, httpResponseMessage), allocateMemoryForContent: true, CancellationToken.None);
            
            response.Should().BeEquivalentTo(expectedResponse, x => x
                .Excluding(r => r.Metadatas)
                .Excluding(r => r.Content.Stream)
                .Excluding(r => r.Request.Content!.Stream));
            
            (await response.Content.Stream.ReadToEndAsync(response.Content.Encoding))
                .Should().BeEquivalentTo(await expectedResponse.Content.Stream.ReadToEndAsync(expectedResponse.Content.Encoding));
            response.Metadatas.Where(x => x.Key != HttpKnownHeaderNames.Date && x.Key != HttpKnownHeaderNames.TransferEncoding)
                .Should().BeEquivalentTo(expectedResponse.Metadatas, x => x.WithoutStrictOrdering());
        }

        private static TestCaseData ExecutionHeadRequestTestCase()
        {
            const RequestType method = RequestType.Check;
            var requestFactory = new Func<Uri, IRequest>(uri =>
            {
                var resource = new UriBuilder(uri) { Path = Path }.Uri;
                var request = new Request(RequestId, resource, method)
                {
                    Content = null
                };
                request.AddMetadata(AcceptHeader.Name, AcceptHeader.Value);
                return request;
            });

            var responseFactory = new Func<Uri, IResponse>(uri =>
            {
                var resource = new UriBuilder(uri) { Path = Path }.Uri;
                var finalRequest = new Request(RequestId, resource, method)
                {
                    Content = null
                };
                finalRequest.AddMetadata(AcceptHeader.Name, AcceptHeader.Value);

                return new Response(finalRequest)
                {
                    Content = new Content(headerContainer: new MetadataContainer(new[]
                    {
                        EmptyContentLengthMetadata
                    })),
                    StatusCode = (int) HttpStatusCode.OK,
                    StatusDescription = "OK",
                    Resource = resource,
                    Metadatas = new MetadataContainer(new[]
                    {
                        ServerHeader
                    }),
                    ErrorMessage = null,
                    ErrorException = null,
                    ProtocolVersion = new Version(1, 1),
                    IsSuccessful = true
                };
            });

            var serverFactory = new Lazy<IWireMockServer>(() =>
            {
                var server = WireMockServer.Start();
                server.Given(WireMock.RequestBuilders.Request.Create()
                        .WithPath(Path)
                        .WithHeader(AcceptHeader.Name, AcceptHeader.Value)
                        .UsingHead())
                    .RespondWith(WireMock.ResponseBuilders.Response.Create()
                        .WithStatusCode(200)
                        .WithHeader(ServerHeader.Name, ServerHeader.Value));
                return server;
            });

            return new TestCaseData(requestFactory, responseFactory, serverFactory)
                .SetName(nameof(ExecutionHeadRequestTestCase));
        }
        
        private static TestCaseData ExecutionGetRequestTestCase()
        {
            const RequestType method = RequestType.Read;
            var content = Serializer.Serialize(Data);
            
            var requestFactory = new Func<Uri, IRequest>(uri =>
            {
                var resource = new UriBuilder(uri) { Path = Path }.Uri;
                var request = new Request(RequestId, resource, method)
                {
                    Content = null
                };
                request.AddMetadata(AcceptHeader.Name, AcceptHeader.Value);
                return request;
            });

            var responseFactory = new Func<Uri, IResponse>(uri =>
            {
                var resource = new UriBuilder(uri) { Path = Path }.Uri;
                var finalRequest = new Request(RequestId, resource, method)
                {
                    Content = null
                };
                finalRequest.AddMetadata(AcceptHeader.Name, AcceptHeader.Value);
                
                var bytes = Encoding.UTF8.GetBytes(content);
                return new Response(finalRequest)
                {
                    Content = new Content(new MemoryStream(bytes), ContentEncodingHeader.Value, new MetadataContainer(new[]
                    {
                        ContentTypeHeader,
                        ContentEncodingHeader,
                        new Metadata(EmptyContentLengthMetadata.Name, content.Length.ToString())
                    })),
                    StatusCode = (int) HttpStatusCode.OK,
                    StatusDescription = "OK",
                    Resource = resource,
                    Metadatas = new MetadataContainer(new[]
                    {
                        ServerHeader
                    }),
                    ErrorMessage = null,
                    ErrorException = null,
                    ProtocolVersion = new Version(1, 1),
                    IsSuccessful = true
                };
            });

            var serverFactory = new Lazy<IWireMockServer>(() =>
            {
                var server = WireMockServer.Start();
                server.Given(WireMock.RequestBuilders.Request.Create()
                        .WithPath(Path)
                        .WithHeader(AcceptHeader.Name, AcceptHeader.Value)
                        .UsingGet())
                    .RespondWith(WireMock.ResponseBuilders.Response.Create()
                        .WithStatusCode(200)
                        .WithHeader(ContentEncodingHeader.Name, ContentEncodingHeader.Value)
                        .WithHeader(ContentTypeHeader.Name, ContentTypeHeader.Value)
                        .WithHeader(EmptyContentLengthMetadata.Name, content.Length.ToString())
                        .WithHeader(ServerHeader.Name, ServerHeader.Value)
                        .WithBodyAsJson(Data));
                return server;
            });

            return new TestCaseData(requestFactory, responseFactory, serverFactory)
                .SetName(nameof(ExecutionGetRequestTestCase));
        }
        
        private static TestCaseData ExecutionPostRequestTestCase()
        {
            const RequestType method = RequestType.Create;
            var content = Serializer.Serialize(Data);

            var requestFactory = new Func<Uri, IRequest>(uri =>
            {
                var resource = new UriBuilder(uri) { Path = Path }.Uri;
                var request = new Request(RequestId, resource, method)
                {
                    Content = new Content(
                        new MemoryStream(Encoding.UTF8.GetBytes(content)),
                        Encoding.UTF8.WebName,
                        new MetadataContainer(new[]
                        {
                            new Metadata(ContentEncodingHeader.Name, Encoding.UTF8.WebName),
                            new Metadata(ContentTypeHeader.Name, ContentTypeHeader.Value),
                            new Metadata(EmptyContentLengthMetadata.Name, content.Length.ToString())
                        }))
                };
                request.AddMetadata(AcceptHeader.Name, AcceptHeader.Value);
                return request;
            });

            var responseFactory = new Func<Uri, IResponse>(uri =>
            {
                var resource = new UriBuilder(uri) { Path = Path }.Uri;
                var finalRequest = new Request(RequestId, resource, method)
                {
                    Content = new Content(
                        new MemoryStream(Encoding.UTF8.GetBytes(content)),
                        Encoding.UTF8.WebName,
                        new MetadataContainer(new[]
                        {
                            new Metadata(ContentEncodingHeader.Name, Encoding.UTF8.WebName),
                            new Metadata(ContentTypeHeader.Name, ContentTypeHeader.Value),
                            new Metadata(EmptyContentLengthMetadata.Name, content.Length.ToString())
                        }))
                };
                finalRequest.AddMetadata(AcceptHeader.Name, AcceptHeader.Value);

                return new Response(finalRequest)
                {
                    Content = new Content(headerContainer: new MetadataContainer(new[]
                    {
                        EmptyContentLengthMetadata
                    })),
                    StatusCode = (int) HttpStatusCode.OK,
                    StatusDescription = "OK",
                    Resource = resource,
                    Metadatas = new MetadataContainer(new[]
                    {
                        ServerHeader
                    }),
                    ErrorMessage = null,
                    ErrorException = null,
                    ProtocolVersion = new Version(1, 1),
                    IsSuccessful = true
                };
            });

            var serverFactory = new Lazy<IWireMockServer>(() =>
            {
                var server = WireMockServer.Start();
                server.Given(WireMock.RequestBuilders.Request.Create()
                        .WithPath(Path)
                        .WithHeader(AcceptHeader.Name, AcceptHeader.Value)
                        .WithHeader(ContentTypeHeader.Name, ContentTypeHeader.Value)
                        .WithBody(new JsonMatcher(Data))
                        .UsingPost())
                    .RespondWith(WireMock.ResponseBuilders.Response.Create()
                        .WithStatusCode(200)
                        .WithHeader(EmptyContentLengthMetadata.Name, EmptyContentLengthMetadata.Value)
                        .WithHeader(ServerHeader.Name, ServerHeader.Value));
                return server;
            });

            return new TestCaseData(requestFactory, responseFactory, serverFactory)
                .SetName(nameof(ExecutionPostRequestTestCase));
        }
    }
}
