using System;
using System.Collections;
using System.Linq;
using System.Net;
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
    public class SystemHttpClientTest
    {
        private static readonly Uri Host = new("http://localhost:5022");
        private static readonly Uri Resource = new(Host, "api/method");
        private static readonly Guid RequestId = Guid.Parse("55df3bb2-a254-4beb-87a8-70e18b74d995");
        private static readonly BasicEntity Data = new() { Id = 1, Value = 2 };
        private static readonly ISerializer Serializer = new SystemJsonSerializerProvider().Create();
        private static readonly Header AcceptHeader = new("Accept", "application/json");
        private static readonly Header ServerHeader = new("Server", "Kestrel");
        private static readonly Header EmptyContentLengthHeader = new("Content-Length", "0");
        private static readonly Header ContentEncodingHeader = new("Content-Encoding", "UTF-8");
        private static readonly Header ContentTypeHeader = new("Content-Type", "application/json; charset=utf-8");
        
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
            var httpClient = new SystemHttpTransportProvider().Create(Serializer);
            var httpMessageBuilder = new SystemHttpTransportMessageBuilderProvider().Create(Serializer);

            var httpRequestMessage = await httpMessageBuilder.BuildTransportRequestAsync(request);
            var httpResponseMessage = await httpClient.ExecuteAsync(httpRequestMessage);
            var response = await httpMessageBuilder.BuildResponseAsync(request, httpRequestMessage, httpResponseMessage);
            
            response.Should().BeEquivalentTo(expectedResponse, x => x.Excluding(r => r.Headers));
            response.Headers.Where(x => x.Key != HttpKnownHeaderNames.Date && x.Key != HttpKnownHeaderNames.TransferEncoding)
                .Should().BeEquivalentTo(expectedResponse.Headers, x => x.WithoutStrictOrdering());
        }

        private static TestCaseData ExecutionHeadRequestTestCase()
        {
            const RequestType method = RequestType.Head;
            var request = new Request(RequestId, Resource, method)
            {
                Data = null,
                Content = null
            };
            
            var finalRequest = new Request(RequestId, Resource, method)
            {
                Data = null,
                Content = null
            };
            finalRequest.AddHeader(AcceptHeader.Name, AcceptHeader.Value);
            
            var response = new Response(finalRequest)
            {
                Content = new Content(headerContainer: new ContentHeaderContainer(new[]
                {
                    EmptyContentLengthHeader
                })),
                StatusCode = (int)HttpStatusCode.OK,
                StatusDescription = "OK",
                ResponseUri = Resource,
                Headers = new HeaderContainer(new[]
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
                        .WithPath(Resource.PathAndQuery)
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
            var request = new Request(RequestId, Resource, method)
            {
                Data = null,
                Content = null
            };
            
            var finalRequest = new Request(RequestId, Resource, method)
            {
                Data = null,
                Content = null
            };
            finalRequest.AddHeader(AcceptHeader.Name, AcceptHeader.Value);
            
            var content = Serializer.Serialize(Data);
            var bytes = Encoding.UTF8.GetBytes(content);
            var response = new Response(finalRequest)
            {
                Content = new Content(bytes, new ContentHeaderContainer(new[]
                {
                    ContentTypeHeader,
                    new Header(EmptyContentLengthHeader.Name, content.Length.ToString())
                })),
                StatusCode = (int)HttpStatusCode.OK,
                StatusDescription = "OK",
                ResponseUri = Resource,
                Headers = new HeaderContainer(new[]
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
                        .WithPath(Resource.PathAndQuery)
                        .WithHeader(AcceptHeader.Name, AcceptHeader.Value)
                        .UsingGet())
                    .RespondWith(WireMock.ResponseBuilders.Response.Create()
                        .WithStatusCode(200)
                        .WithHeader(ContentTypeHeader.Name, ContentTypeHeader.Value)
                        .WithHeader(EmptyContentLengthHeader.Name, content.Length.ToString())
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
            
            var request = new Request(RequestId, Resource, method)
            {
                Data = Data,
                Content = null
            };
            
            var finalRequest = new Request(RequestId, Resource, method)
            {
                Data = Data,
                Content = content
            };
            finalRequest.AddHeader(AcceptHeader.Name, AcceptHeader.Value);
            finalRequest.AddHeader(ContentTypeHeader.Name, ContentTypeHeader.Value);
            finalRequest.AddHeader(EmptyContentLengthHeader.Name, content.Length.ToString());
            
            var response = new Response(finalRequest)
            {
                Content = new Content(headerContainer: new ContentHeaderContainer(new[]
                {
                    ContentTypeHeader,
                    ContentEncodingHeader,
                    EmptyContentLengthHeader
                })),
                StatusCode = (int)HttpStatusCode.OK,
                StatusDescription = "OK",
                ResponseUri = Resource,
                Headers = new HeaderContainer(new[]
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
                        .WithPath(Resource.PathAndQuery)
                        .WithHeader(AcceptHeader.Name, AcceptHeader.Value)
                        .WithHeader(ContentTypeHeader.Name, ContentTypeHeader.Value)
                        .WithBody(new JsonMatcher(Data))
                        .UsingPost())
                    .RespondWith(WireMock.ResponseBuilders.Response.Create()
                        .WithStatusCode(200)
                        .WithHeader(ContentTypeHeader.Name, ContentTypeHeader.Value)
                        .WithHeader(ContentEncodingHeader.Name, ContentEncodingHeader.Value)
                        .WithHeader(EmptyContentLengthHeader.Name, content.Length.ToString())
                        .WithHeader(ServerHeader.Name, ServerHeader.Value));
                return server;
            });

            return new TestCaseData(request, response, serverFactory)
                .SetName(nameof(ExecutionPostRequestTestCase));
        }
    }
}
