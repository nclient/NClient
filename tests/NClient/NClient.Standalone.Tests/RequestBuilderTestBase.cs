using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentAssertions;
using NClient.Core.Helpers.ObjectMemberManagers;
using NClient.Core.Mappers;
using NClient.Providers.Serialization;
using NClient.Providers.Serialization.Json.System;
using NClient.Providers.Transport;
using NClient.Standalone.ClientProxy.Interceptors.MethodBuilders;
using NClient.Standalone.ClientProxy.Interceptors.MethodBuilders.Models;
using NClient.Standalone.ClientProxy.Interceptors.MethodBuilders.Providers;
using NClient.Standalone.ClientProxy.Interceptors.RequestBuilders;
using NClient.Standalone.Exceptions.Factories;
using NClient.Standalone.Helpers.ObjectToKeyValueConverters;
using NUnit.Framework;

namespace NClient.Standalone.Tests
{
    public abstract class RequestBuilderTestBase
    {
        protected static readonly Guid RequestId = Guid.Parse("5bb86773-9999-483e-aa9a-3cce10e47fb1");

        internal MethodBuilder MethodBuilder = null!;
        internal RequestBuilder RequestBuilder = null!;
        internal ISerializer Serializer = null!;
        internal IClientArgumentExceptionFactory ClientArgumentExceptionFactory = null!;
        internal IClientValidationExceptionFactory ClientValidationExceptionFactory = null!;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var objectMemberManager = new ObjectMemberManager(new ClientObjectMemberManagerExceptionFactory());

            Serializer = new SystemJsonSerializerProvider().Create();
            ClientArgumentExceptionFactory = new ClientArgumentExceptionFactory();
            ClientValidationExceptionFactory = new ClientValidationExceptionFactory();
            RequestBuilder = new RequestBuilder(
                Serializer,
                new RouteTemplateProvider(ClientValidationExceptionFactory),
                new RouteProvider(objectMemberManager, ClientArgumentExceptionFactory, ClientValidationExceptionFactory),
                new RequestTypeProvider(ClientValidationExceptionFactory),
                new ObjectToKeyValueConverter(objectMemberManager, ClientValidationExceptionFactory),
                ClientValidationExceptionFactory);

            var attributeMapper = new AttributeMapper();
            MethodBuilder = new MethodBuilder(
                new MethodAttributeProvider(attributeMapper, ClientValidationExceptionFactory),
                new UseVersionAttributeProvider(attributeMapper, ClientValidationExceptionFactory),
                new PathAttributeProvider(attributeMapper, ClientValidationExceptionFactory),
                new HeaderAttributeProvider(ClientValidationExceptionFactory),
                new MethodParamBuilder(new ParamAttributeProvider(attributeMapper, ClientValidationExceptionFactory)));
        }

        protected static MethodInfo GetMethodInfo<T>()
        {
            return typeof(T).GetMethods().First();
        }

        internal Method BuildMethod<T>()
        {
            return MethodBuilder.Build(typeof(T), GetMethodInfo<T>());
        }

        internal IRequest BuildRequest(Method method, params object[] arguments)
        {
            return BuildRequest(host: "http://localhost:5000", method, arguments);
        }

        internal IRequest BuildRequest(string host, Method method, params object[] arguments)
        {
            return RequestBuilder.Build(RequestId, resourceRoot: host, method, arguments);
        }

        protected void AssertHttpRequest(
            IRequest actualRequest,
            Uri uri,
            RequestType requestType,
            IEnumerable<IParameter>? parameters = null,
            IEnumerable<IMetadata>? metadatas = null,
            object? body = null)
        {
            var contentBytes = Encoding.UTF8.GetBytes(Serializer.Serialize(body));
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
            
            if (body is null)
            {
                actualRequest.Content.Should().BeNull();   
            }
            else
            {
                actualRequest.Content.Should().NotBeNull();
                actualRequest.Content!.Bytes.Should().BeEquivalentTo(contentBytes);
                actualRequest.Content!.Encoding.Should().BeEquivalentTo(Encoding.UTF8);
                actualRequest.Content!.Metadatas.SelectMany(x => x.Value).Should().BeEquivalentTo(new[]
                {
                    new Metadata("Content-Encoding", Encoding.UTF8.WebName),
                    new Metadata("Content-Type", Serializer.ContentType),
                    new Metadata("Content-Length", contentBytes.Length.ToString())
                }, x => x.WithStrictOrdering());
            }
        }
    }
}
