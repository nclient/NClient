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
using NClient.Providers.Results.HttpMessages;
using NClient.Providers.Serialization.Json.System;
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
        private static readonly BasicEntity Data = new() { Id = 1, Value = 2 };
        private static readonly ISerializer Serializer = new SystemJsonSerializerProvider().Create();
        private static readonly HttpHeader AcceptHeader = new("Accept", "application/json");
        private static readonly HttpHeader ServerHeader = new("Server", "Kestrel");
        private static readonly HttpHeader EmptyContentLengthHeader = new("Content-Length", "0");
        private static readonly HttpHeader ContentEncodingHeader = new("Content-Encoding", "UTF-8");
        private static readonly HttpHeader ContentTypeHeader = new("Content-Type", "application/json; charset=utf-8");
        
        public static readonly IEnumerable ValidTestCases = new[]
        {
            ExecutionHeadRequestTestCase(),
            ExecutionGetRequestTestCase(),
            ExecutionPostRequestTestCase()
        };
        
        [TestCaseSource(nameof(ValidTestCases))]
        public async Task Test(IHttpRequest httpRequest, IHttpResponse expectedResponse, Lazy<IWireMockServer> serverFactory)
        {
            using var server = serverFactory.Value;
            var httpClient = new SystemHttpClientProvider().Create(Serializer);
            var httpMessageBuilder = new SystemHttpMessageBuilderProvider().Create(Serializer);

            var request = await httpMessageBuilder.BuildRequestAsync(httpRequest);
            var response = await httpClient.ExecuteAsync(request);
            var httpResponse = await httpMessageBuilder.BuildResponseAsync(httpRequest, response);
            
            httpResponse.Should().BeEquivalentTo(expectedResponse, x => x.Excluding(r => r.Headers));
            httpResponse.Headers.Where(x => x.Key != HttpKnownHeaderNames.Date && x.Key != HttpKnownHeaderNames.TransferEncoding)
                .Should().BeEquivalentTo(expectedResponse.Headers, x => x.WithoutStrictOrdering());
        }

        private static TestCaseData ExecutionHeadRequestTestCase()
        {
            var method = HttpMethod.Head;
            var request = new HttpRequest(RequestId, Resource, method)
            {
                Data = null,
                Content = null
            };
            
            var finalRequest = new HttpRequest(RequestId, Resource, method)
            {
                Data = null,
                Content = null
            };
            finalRequest.AddHeader(AcceptHeader.Name, AcceptHeader.Value);
            
            var response = new HttpResponse(finalRequest)
            {
                Content = new HttpResponseContent(headerContainer: new HttpResponseContentHeaderContainer(new[]
                {
                    EmptyContentLengthHeader
                })),
                StatusCode = HttpStatusCode.OK,
                StatusDescription = "OK",
                ResponseUri = Resource,
                Headers = new HttpResponseHeaderContainer(new[]
                {
                    ServerHeader
                }),
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

            return new TestCaseData(request, response, serverFactory)
                .SetName(nameof(ExecutionHeadRequestTestCase));
        }
        
        private static TestCaseData ExecutionGetRequestTestCase()
        {
            var method = HttpMethod.Get;
            var request = new HttpRequest(RequestId, Resource, method)
            {
                Data = null,
                Content = null
            };
            
            var finalRequest = new HttpRequest(RequestId, Resource, method)
            {
                Data = null,
                Content = null
            };
            finalRequest.AddHeader(AcceptHeader.Name, AcceptHeader.Value);
            
            var content = Serializer.Serialize(Data);
            var bytes = Encoding.UTF8.GetBytes(content);
            var response = new HttpResponse(finalRequest)
            {
                Content = new HttpResponseContent(bytes, new HttpResponseContentHeaderContainer(new[]
                {
                    ContentTypeHeader,
                    new HttpHeader(EmptyContentLengthHeader.Name, content.Length.ToString())
                })),
                StatusCode = HttpStatusCode.OK,
                StatusDescription = "OK",
                ResponseUri = Resource,
                Headers = new HttpResponseHeaderContainer(new[]
                {
                    ServerHeader
                }),
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
                        .WithBodyAsJson(Data));
                return server;
            });

            return new TestCaseData(request, response, serverFactory)
                .SetName(nameof(ExecutionGetRequestTestCase));
        }
        
        private static TestCaseData ExecutionPostRequestTestCase()
        {
            var method = HttpMethod.Post;
            var content = Serializer.Serialize(Data);
            
            var request = new HttpRequest(RequestId, Resource, method)
            {
                Data = Data,
                Content = null
            };
            
            var finalRequest = new HttpRequest(RequestId, Resource, method)
            {
                Data = Data,
                Content = content
            };
            finalRequest.AddHeader(AcceptHeader.Name, AcceptHeader.Value);
            finalRequest.AddHeader(ContentTypeHeader.Name, ContentTypeHeader.Value);
            finalRequest.AddHeader(EmptyContentLengthHeader.Name, content.Length.ToString());
            
            var response = new HttpResponse(finalRequest)
            {
                Content = new HttpResponseContent(headerContainer: new HttpResponseContentHeaderContainer(new[]
                {
                    ContentTypeHeader,
                    ContentEncodingHeader,
                    EmptyContentLengthHeader
                })),
                StatusCode = HttpStatusCode.OK,
                StatusDescription = "OK",
                ResponseUri = Resource,
                Headers = new HttpResponseHeaderContainer(new[]
                {
                    ServerHeader
                }),
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
                        .WithBody(new JsonMatcher(Data))
                        .UsingPost())
                    .RespondWith(Response.Create()
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
