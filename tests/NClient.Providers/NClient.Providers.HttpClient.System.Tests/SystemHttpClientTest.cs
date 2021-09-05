using System;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;
using NClient.Providers.Serialization.System;
using NClient.Testing.Common.Entities;
using NUnit.Framework;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace NClient.Providers.HttpClient.System.Tests
{
    public class SystemHttpClientTest
    {
        private static readonly Uri Host = new("http://localhost:5022");
        private static readonly Uri Resource = new(Host, "api/method");
        private static readonly Guid RequestId = Guid.Parse("55df3bb2-a254-4beb-87a8-70e18b74d995");
        private static readonly BasicEntity Body = new() { Id = 1, Value = 2 };
        private static readonly ISerializer Serializer = new SystemSerializerProvider().Create();
        private static readonly HttpHeader AcceptHeader = new("Accept", "application/json");
        private static readonly HttpHeader ServerHeader = new("Server", "Kestrel");
        private static readonly HttpHeader EmptyContentLengthHeader = new("Content-Length", "0");
        private static readonly HttpHeader ContentEncodingHeader = new("Content-Encoding", "UTF-8");
        private static readonly HttpHeader ContentTypeHeader = new("Content-Type", "application/json; charset=utf-8");
        
        public static readonly IEnumerable ValidTestCases = new[]
        {
            BuildHeadRequestTestCase(),
            BuildGetRequestTestCase(),
            BuildPostRequestTestCase()
        };
        
        [TestCaseSource(nameof(ValidTestCases))]
        public async Task Test(HttpRequest request, HttpResponse expectedResponse, Lazy<IWireMockServer> serverFactory)
        {
            using var server = serverFactory.Value;
            var httpClient = new SystemHttpClientProvider().Create(Serializer);

            var response = await httpClient.ExecuteAsync(request);
            
            response.Should().BeEquivalentTo(expectedResponse, x => x.Excluding(r => r.Headers));
            response.Headers.Where(x => x.Name != "Date" && x.Name != "Transfer-Encoding")
                .Should().BeEquivalentTo(expectedResponse.Headers, x => x.WithoutStrictOrdering());
        }

        private static TestCaseData BuildHeadRequestTestCase()
        {
            var method = HttpMethod.Head;
            var request = new HttpRequest(RequestId, Resource, method)
            {
                Body = null,
                Content = null
            };
            
            var finalRequest = new HttpRequest(RequestId, Resource, method)
            {
                Body = null,
                Content = null
            };
            finalRequest.AddHeader(AcceptHeader.Name, AcceptHeader.Value);
            
            var response = new HttpResponse(finalRequest)
            {
                ContentType = null,
                ContentLength = int.Parse(EmptyContentLengthHeader.Value),
                ContentEncoding = null,
                RawBytes = Array.Empty<byte>(),
                StatusCode = HttpStatusCode.OK,
                StatusDescription = "OK",
                ResponseUri = Resource,
                Server = ServerHeader.Value,
                Headers = new[] { ServerHeader, EmptyContentLengthHeader },
                ErrorMessage = null,
                ErrorException = null,
                ProtocolVersion = new Version(1, 1)
            };

            var serverFactory = new Lazy<IWireMockServer>(() =>
            {
                var server = WireMockServer.Start(Host.ToString());
                server.Given(Request.Create()
                        .WithPath(Resource.PathAndQuery)
                        .WithHeader(AcceptHeader.Name, AcceptHeader.Value)
                        .UsingHead())
                    .RespondWith(Response.Create()
                        .WithStatusCode(200)
                        .WithHeader(ServerHeader.Name, ServerHeader.Value));
                return server;
            });

            return new TestCaseData(request, response, serverFactory);
        }
        
        private static TestCaseData BuildGetRequestTestCase()
        {
            var method = HttpMethod.Get;
            var request = new HttpRequest(RequestId, Resource, method)
            {
                Body = null,
                Content = null
            };
            
            var finalRequest = new HttpRequest(RequestId, Resource, method)
            {
                Body = null,
                Content = null
            };
            finalRequest.AddHeader(AcceptHeader.Name, AcceptHeader.Value);
            
            var content = Serializer.Serialize(Body);
            var rawBytes = Encoding.UTF8.GetBytes(content);
            var response = new HttpResponse(finalRequest)
            {
                ContentType = AcceptHeader.Value,
                ContentLength = content.Length,
                ContentEncoding = null,
                RawBytes = rawBytes,
                StatusCode = HttpStatusCode.OK,
                StatusDescription = "OK",
                ResponseUri = Resource,
                Server = ServerHeader.Value,
                Headers = new[]
                {
                    ServerHeader,
                    ContentTypeHeader,
                    new HttpHeader(EmptyContentLengthHeader.Name, content.Length.ToString())
                },
                ErrorMessage = null,
                ErrorException = null,
                ProtocolVersion = new Version(1, 1)
            };

            var serverFactory = new Lazy<IWireMockServer>(() =>
            {
                var server = WireMockServer.Start(Host.ToString());
                server.Given(Request.Create()
                        .WithPath(Resource.PathAndQuery)
                        .WithHeader(AcceptHeader.Name, AcceptHeader.Value)
                        .UsingGet())
                    .RespondWith(Response.Create()
                        .WithStatusCode(200)
                        .WithHeader(ContentTypeHeader.Name, ContentTypeHeader.Value)
                        .WithHeader(EmptyContentLengthHeader.Name, content.Length.ToString())
                        .WithHeader(ServerHeader.Name, ServerHeader.Value)
                        .WithBodyAsJson(Body));
                return server;
            });

            return new TestCaseData(request, response, serverFactory);
        }
        
        private static TestCaseData BuildPostRequestTestCase()
        {
            var method = HttpMethod.Post;
            var content = Serializer.Serialize(Body);
            
            var request = new HttpRequest(RequestId, Resource, method)
            {
                Body = Body,
                Content = null
            };
            
            var finalRequest = new HttpRequest(RequestId, Resource, method)
            {
                Body = Body,
                Content = content
            };
            finalRequest.AddHeader(AcceptHeader.Name, AcceptHeader.Value);
            finalRequest.AddHeader(ContentTypeHeader.Name, ContentTypeHeader.Value);
            finalRequest.AddHeader(EmptyContentLengthHeader.Name, content.Length.ToString());
            
            var response = new HttpResponse(finalRequest)
            {
                ContentType = AcceptHeader.Value,
                ContentLength = int.Parse(EmptyContentLengthHeader.Value),
                ContentEncoding = ContentEncodingHeader.Value,
                RawBytes = Array.Empty<byte>(),
                StatusCode = HttpStatusCode.OK,
                StatusDescription = "OK",
                ResponseUri = Resource,
                Server = ServerHeader.Value,
                Headers = new[]
                {
                    ServerHeader,
                    ContentTypeHeader,
                    ContentEncodingHeader,
                    EmptyContentLengthHeader
                },
                ErrorMessage = null,
                ErrorException = null,
                ProtocolVersion = new Version(1, 1)
            };

            var serverFactory = new Lazy<IWireMockServer>(() =>
            {
                var server = WireMockServer.Start(Host.ToString());
                server.Given(Request.Create()
                        .WithPath(Resource.PathAndQuery)
                        .WithHeader(AcceptHeader.Name, AcceptHeader.Value)
                        .WithHeader(ContentTypeHeader.Name, ContentTypeHeader.Value)
                        .WithBody(new JsonMatcher(Body))
                        .UsingPost())
                    .RespondWith(Response.Create()
                        .WithStatusCode(200)
                        .WithHeader(ContentTypeHeader.Name, ContentTypeHeader.Value)
                        .WithHeader(ContentEncodingHeader.Name, ContentEncodingHeader.Value)
                        .WithHeader(EmptyContentLengthHeader.Name, content.Length.ToString())
                        .WithHeader(ServerHeader.Name, ServerHeader.Value));
                return server;
            });

            return new TestCaseData(request, response, serverFactory);
        }
    }
}
