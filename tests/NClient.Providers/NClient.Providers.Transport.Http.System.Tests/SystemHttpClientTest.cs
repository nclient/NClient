using System;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NClient.Providers.Serialization;
using NClient.Providers.Serialization.Json.System;
using NClient.Testing.Common.Entities;
using NClient.Testing.Common.Helpers;
using NUnit.Framework;
using WireMock.Matchers;
using WireMock.Server;

namespace NClient.Providers.Transport.Http.System.Tests
{
    [Parallelizable]
    public class SystemHttpClientTest
    {
        private static readonly Uri Host = new("http://localhost:5022");
        private static readonly Uri EndpointUri = new(Host, "api/method");
        private static readonly Guid RequestId = Guid.Parse("55df3bb2-a254-4beb-87a8-70e18b74d995");
        private static readonly BasicEntity Data = new() { Id = 1, Value = 2 };
        private static readonly ISerializer Serializer = new SystemJsonSerializerProvider().Create();
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
        public async Task Test(IRequest request, IResponse expectedResponse, Lazy<IWireMockServer> serverFactory)
        {
            using var server = serverFactory.Value;
            var transport = new SystemHttpTransportProvider().Create(Serializer);
            var transportRequestBuilder = new SystemHttpTransportRequestBuilderProvider().Create(Serializer);
            var responseBuilder = new SystemHttpResponseBuilderProvider().Create(Serializer);

            var httpRequestMessage = await transportRequestBuilder.BuildAsync(request);
            var httpResponseMessage = await transport.ExecuteAsync(httpRequestMessage);
            var response = await responseBuilder.BuildAsync(request, new ResponseContext<HttpRequestMessage, HttpResponseMessage>(httpRequestMessage, httpResponseMessage));
            
            response.Should().BeEquivalentTo(expectedResponse, x => x.Excluding(r => r.Metadatas));
            response.Metadatas.Where(x => x.Key != HttpKnownHeaderNames.Date && x.Key != HttpKnownHeaderNames.TransferEncoding)
                .Should().BeEquivalentTo(expectedResponse.Metadatas, x => x.WithoutStrictOrdering());
        }

        private static TestCaseData ExecutionHeadRequestTestCase()
        {
            const RequestType method = RequestType.Check;
            var request = new Request(RequestId, EndpointUri.ToString(), method)
            {
                Content = null
            };
            request.AddMetadata(AcceptHeader.Name, AcceptHeader.Value);
            
            var finalRequest = new Request(RequestId, EndpointUri.ToString(), method)
            {
                Content = null
            };
            finalRequest.AddMetadata(AcceptHeader.Name, AcceptHeader.Value);
            
            var response = new Response(finalRequest)
            {
                #if NETFRAMEWORK
                Content = new Content(headerContainer: new MetadataContainer(new[]
                {
                    EmptyContentLengthMetadata
                })),
                #else
                Content = new Content(headerContainer: new MetadataContainer(Array.Empty<IMetadata>())),
                #endif
                StatusCode = (int)HttpStatusCode.OK,
                StatusDescription = "OK",
                Endpoint = EndpointUri.ToString(),
                Metadatas = new MetadataContainer(new[]
                {
                    ServerHeader
                }),
                ErrorMessage = null,
                ErrorException = null,
                ProtocolVersion = new Version(1, 1),
                IsSuccessful = true
            };

            var serverFactory = new Lazy<IWireMockServer>(() =>
            {
                var server = WireMockServer.Start(Host.ToString());
                server.Given(WireMock.RequestBuilders.Request.Create()
                        .WithPath(EndpointUri.PathAndQuery)
                        .WithHeader(AcceptHeader.Name, AcceptHeader.Value)
                        .UsingHead())
                    .RespondWith(WireMock.ResponseBuilders.Response.Create()
                        .WithStatusCode(200)
                        .WithHeader(ServerHeader.Name, ServerHeader.Value));
                return server;
            });

            return new TestCaseData(request, response, serverFactory)
                .SetName(nameof(ExecutionHeadRequestTestCase));
        }
        
        private static TestCaseData ExecutionGetRequestTestCase()
        {
            const RequestType method = RequestType.Read;
            var request = new Request(RequestId, EndpointUri.ToString(), method)
            {
                Content = null
            };
            request.AddMetadata(AcceptHeader.Name, AcceptHeader.Value);
            
            var finalRequest = new Request(RequestId, EndpointUri.ToString(), method)
            {
                Content = null
            };
            finalRequest.AddMetadata(AcceptHeader.Name, AcceptHeader.Value);
            
            var content = Serializer.Serialize(Data);
            var bytes = Encoding.UTF8.GetBytes(content);
            var response = new Response(finalRequest)
            {
                Content = new Content(bytes, ContentEncodingHeader.Value, new MetadataContainer(new[]
                {
                    ContentTypeHeader,
                    ContentEncodingHeader,
                    new Metadata(EmptyContentLengthMetadata.Name, content.Length.ToString())
                })),
                StatusCode = (int)HttpStatusCode.OK,
                StatusDescription = "OK",
                Endpoint = EndpointUri.ToString(),
                Metadatas = new MetadataContainer(new[]
                {
                    ServerHeader
                }),
                ErrorMessage = null,
                ErrorException = null,
                ProtocolVersion = new Version(1, 1),
                IsSuccessful = true
            };

            var serverFactory = new Lazy<IWireMockServer>(() =>
            {
                var server = WireMockServer.Start(Host.ToString());
                server.Given(WireMock.RequestBuilders.Request.Create()
                        .WithPath(EndpointUri.PathAndQuery)
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

            return new TestCaseData(request, response, serverFactory)
                .SetName(nameof(ExecutionGetRequestTestCase));
        }
        
        private static TestCaseData ExecutionPostRequestTestCase()
        {
            const RequestType method = RequestType.Create;
            var content = Serializer.Serialize(Data);
            
            var request = new Request(RequestId, EndpointUri.ToString(), method)
            {
                Content = new Content(
                    Encoding.UTF8.GetBytes(content),
                    Encoding.UTF8.WebName,
                    new MetadataContainer(new[]
                    {
                        new Metadata(ContentEncodingHeader.Name, Encoding.UTF8.WebName),
                        new Metadata(ContentTypeHeader.Name, ContentTypeHeader.Value),
                        new Metadata(EmptyContentLengthMetadata.Name, content.Length.ToString())
                    }))
            };
            request.AddMetadata(AcceptHeader.Name, AcceptHeader.Value);
            
            var finalRequest = new Request(RequestId, EndpointUri.ToString(), method)
            {
                Content = new Content(
                    Encoding.UTF8.GetBytes(content),
                    Encoding.UTF8.WebName,
                    new MetadataContainer(new[]
                    {
                        new Metadata(ContentEncodingHeader.Name, Encoding.UTF8.WebName),
                        new Metadata(ContentTypeHeader.Name, ContentTypeHeader.Value),
                        new Metadata(EmptyContentLengthMetadata.Name, content.Length.ToString())
                    }))
            };
            finalRequest.AddMetadata(AcceptHeader.Name, AcceptHeader.Value);
            
            var response = new Response(finalRequest)
            {
                Content = new Content(headerContainer: new MetadataContainer(new[]
                {
                    EmptyContentLengthMetadata
                })),
                StatusCode = (int)HttpStatusCode.OK,
                StatusDescription = "OK",
                Endpoint = EndpointUri.ToString(),
                Metadatas = new MetadataContainer(new[]
                {
                    ServerHeader
                }),
                ErrorMessage = null,
                ErrorException = null,
                ProtocolVersion = new Version(1, 1),
                IsSuccessful = true
            };

            var serverFactory = new Lazy<IWireMockServer>(() =>
            {
                var server = WireMockServer.Start(Host.ToString());
                server.Given(WireMock.RequestBuilders.Request.Create()
                        .WithPath(EndpointUri.PathAndQuery)
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

            return new TestCaseData(request, response, serverFactory)
                .SetName(nameof(ExecutionPostRequestTestCase));
        }
    }
}
