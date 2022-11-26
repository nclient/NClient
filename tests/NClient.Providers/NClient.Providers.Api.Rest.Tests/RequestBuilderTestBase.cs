using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NClient.Common.Helpers;
using NClient.Core.Helpers.ObjectMemberManagers;
using NClient.Core.Helpers.ObjectToKeyValueConverters;
using NClient.Core.Helpers.ObjectToKeyValueConverters.Factories;
using NClient.Core.Mappers;
using NClient.Invocation;
using NClient.Providers.Api.Rest.Exceptions.Factories;
using NClient.Providers.Api.Rest.Helpers;
using NClient.Providers.Api.Rest.Providers;
using NClient.Providers.Authorization;
using NClient.Providers.Host;
using NClient.Providers.Serialization;
using NClient.Providers.Serialization.SystemTextJson;
using NClient.Providers.Transport;
using NClient.Standalone.ClientProxy.Generation.MethodBuilders;
using NClient.Standalone.ClientProxy.Generation.MethodBuilders.Providers;
using NUnit.Framework;
using StandaloneClientValidationExceptionFactory = NClient.Standalone.Exceptions.Factories.ClientValidationExceptionFactory;
using IStandaloneClientValidationExceptionFactory = NClient.Standalone.Exceptions.Factories.IClientValidationExceptionFactory;

namespace NClient.Providers.Api.Rest.Tests
{
    public abstract class RequestBuilderTestBase
    {
        protected static readonly Guid RequestId = Guid.Parse("5bb86773-9999-483e-aa9a-3cce10e47fb1");
        protected static readonly Uri RequestUri = new("http://localhost:5000");
        protected static readonly Encoding RequestEncoding = Encoding.UTF8;
        
        protected Metadata AcceptMetadata = null!;
        protected Metadata ContentEncodingMetadata = null!;
        protected Metadata ContentTypeMetadata = null!;
        
        protected Metadata GetContentTypeMetadata(string contentType) => new("Content-Type", contentType);
        protected Metadata GetContentLengthMetadata(string contentString) => new("Content-Length", contentString.Length.ToString());
        protected Metadata GetContentDispositionMetadata(string contentDisposition) => new("Content-Disposition", contentDisposition);

        internal MethodBuilder MethodBuilder = null!;
        internal RestRequestBuilder RequestBuilder = null!;
        internal ISerializer Serializer = null!;
        internal IClientArgumentExceptionFactory ClientArgumentExceptionFactory = null!;
        internal IClientValidationExceptionFactory RestClientValidationExceptionFactory = null!;
        internal IStandaloneClientValidationExceptionFactory ClientValidationExceptionFactory = null!;
        internal IObjectToKeyValueConverterExceptionFactory ObjectToKeyValueConverterExceptionFactory = null!;
        internal IAuthorization Authorization = null!;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var objectMemberManager = new ObjectMemberManager(new ObjectMemberManagerExceptionFactory());

            var loggerMock = new Mock<ILogger>();
            Serializer = new SystemTextJsonSerializerProvider().Create(loggerMock.Object);
            var toolset = new Toolset(Serializer, loggerMock.Object);
            ClientArgumentExceptionFactory = new ClientArgumentExceptionFactory();
            RestClientValidationExceptionFactory = new ClientValidationExceptionFactory();
            ClientValidationExceptionFactory = new StandaloneClientValidationExceptionFactory();
            ObjectToKeyValueConverterExceptionFactory = new ObjectToKeyValueConverterExceptionFactory();
            RequestBuilder = new RestRequestBuilder(
                new RouteTemplateProvider(RestClientValidationExceptionFactory),
                new RouteProvider(objectMemberManager, ClientArgumentExceptionFactory, RestClientValidationExceptionFactory),
                new RequestTypeProvider(RestClientValidationExceptionFactory),
                new FormUrlEncoder(),
                new ObjectToKeyValueConverter(objectMemberManager, ObjectToKeyValueConverterExceptionFactory),
                RestClientValidationExceptionFactory,
                toolset);

            var attributeMapper = new AttributeMapper();
            MethodBuilder = new MethodBuilder(
                new OperationAttributeProvider(attributeMapper, ClientValidationExceptionFactory),
                new UseVersionAttributeProvider(attributeMapper, ClientValidationExceptionFactory),
                new PathAttributeProvider(attributeMapper, ClientValidationExceptionFactory),
                new MetadataAttributeProvider(ClientValidationExceptionFactory),
                new TimeoutAttributeProvider(attributeMapper, ClientValidationExceptionFactory),
                new MethodParamBuilder(new ParamAttributeProvider(attributeMapper, ClientValidationExceptionFactory)));

            var authorizationMock = new Mock<IAuthorization>();
            authorizationMock
                .Setup(x => x.TryGetAccessTokensAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IAccessTokens?>(null));
            Authorization = authorizationMock.Object;

            AcceptMetadata = new Metadata(name: "Accept", Serializer.ContentType);
            ContentEncodingMetadata = new Metadata("Content-Encoding", RequestEncoding.WebName);
            ContentTypeMetadata = new Metadata("Content-Type", Serializer.ContentType);
        }

        protected static MethodInfo GetMethodInfo<T>()
        {
            return typeof(T).GetMethods().First();
        }

        internal IMethod BuildMethod<T>()
        {
            return MethodBuilder.Build(typeof(T), GetMethodInfo<T>(), GetMethodInfo<T>().ReturnType);
        }

        internal IRequest BuildRequest(IMethod method, params object[] arguments)
        {
            var hostMock = new Mock<IHost>();
            hostMock
                .Setup(x => x.TryGetUriAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<Uri?>(RequestUri));
            return BuildRequest(host: hostMock.Object, method, arguments);
        }

        internal IRequest BuildRequest(IHost host, IMethod method, params object[] arguments)
        {
            return RequestBuilder
                .BuildAsync(RequestId, host, Authorization, new MethodInvocation(method, arguments), CancellationToken.None)
                .GetAwaiter()
                .GetResult();
        }

        protected async Task AssertHttpRequestAsync(
            IRequest actualRequest,
            Uri uri,
            RequestType requestType,
            IEnumerable<IParameter>? parameters = null,
            IEnumerable<IMetadata>? metadatas = null,
            object? contentObject = null,
            string? contentString = null,
            string? contentType = null)
        {
            var expectedContentJson = contentString ?? Serializer.Serialize(contentObject);
            var acceptHeader = new Metadata("Accept", Serializer.ContentType);
            
            actualRequest.Resource.Should().Be(uri.ToString());
            actualRequest.Type.Should().Be(requestType);
            actualRequest.Parameters.Should().BeEquivalentTo(parameters ?? Array.Empty<IParameter>(), config => config.WithoutStrictOrdering());
            actualRequest.Metadatas.SelectMany(x => x.Value).Should().BeEquivalentTo(metadatas?.Concat(new[]
            {
                acceptHeader
            }) ?? new[]
            {
                acceptHeader
            }, config => config.WithoutStrictOrdering());
            
            if (contentObject is null && contentString is null)
            {
                actualRequest.Content.Should().BeNull();   
            }
            else
            {
                actualRequest.Content.Should().NotBeNull();
                var actualContentJson = await actualRequest.Content!.Stream.ReadToEndAsync(actualRequest.Content.Encoding);
                actualContentJson.Should().BeEquivalentTo(expectedContentJson);
                
                if (actualRequest.Content?.Encoding is { } encoding)
                    actualRequest.Content.Encoding.Should().BeEquivalentTo(encoding);
                
                if (actualRequest.Content!.Metadatas.GetValueOrDefault("Content-Encoding").SingleOrDefault() is Metadata contentEncoding)
                    contentEncoding.Should()
                        .BeEquivalentTo(new Metadata("Content-Encoding", Encoding.UTF8.WebName));
                
                actualRequest.Content!.Metadatas.GetValueOrDefault("Content-Type").Single().Should()
                    .BeEquivalentTo(new Metadata("Content-Type", contentType ?? Serializer.ContentType));
                
                if (actualRequest.Content!.Metadatas.GetValueOrDefault("Content-Length").SingleOrDefault() is Metadata contentLength)
                    contentLength.Should().BeEquivalentTo(new Metadata("Content-Length", expectedContentJson.Length.ToString()));
            }
        }
    }
}
