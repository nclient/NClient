﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentAssertions;
using NClient.Core.Helpers.ObjectMemberManagers;
using NClient.Core.Helpers.ObjectToKeyValueConverters;
using NClient.Core.Helpers.ObjectToKeyValueConverters.Factories;
using NClient.Core.Mappers;
using NClient.Invocation;
using NClient.Providers.Api.Rest.Exceptions.Factories;
using NClient.Providers.Api.Rest.Providers;
using NClient.Providers.Serialization;
using NClient.Providers.Serialization.Json.System;
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

        internal MethodBuilder MethodBuilder = null!;
        internal RestRequestBuilder RequestBuilder = null!;
        internal ISerializer Serializer = null!;
        internal IClientArgumentExceptionFactory ClientArgumentExceptionFactory = null!;
        internal IClientValidationExceptionFactory RestClientValidationExceptionFactory = null!;
        internal IStandaloneClientValidationExceptionFactory ClientValidationExceptionFactory = null!;
        internal IObjectToKeyValueConverterExceptionFactory ObjectToKeyValueConverterExceptionFactory = null!;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var objectMemberManager = new ObjectMemberManager(new ObjectMemberManagerExceptionFactory());

            Serializer = new SystemJsonSerializerProvider().Create(logger: null);
            var toolset = new Toolset(Serializer, logger: null);
            ClientArgumentExceptionFactory = new ClientArgumentExceptionFactory();
            RestClientValidationExceptionFactory = new ClientValidationExceptionFactory();
            ClientValidationExceptionFactory = new StandaloneClientValidationExceptionFactory();
            ObjectToKeyValueConverterExceptionFactory = new ObjectToKeyValueConverterExceptionFactory();
            RequestBuilder = new RestRequestBuilder(
                new RouteTemplateProvider(RestClientValidationExceptionFactory),
                new RouteProvider(objectMemberManager, ClientArgumentExceptionFactory, RestClientValidationExceptionFactory),
                new RequestTypeProvider(RestClientValidationExceptionFactory),
                new ObjectToKeyValueConverter(objectMemberManager, ObjectToKeyValueConverterExceptionFactory),
                RestClientValidationExceptionFactory,
                toolset);

            var attributeMapper = new AttributeMapper();
            MethodBuilder = new MethodBuilder(
                new OperationAttributeProvider(attributeMapper, ClientValidationExceptionFactory),
                new UseVersionAttributeProvider(attributeMapper, ClientValidationExceptionFactory),
                new PathAttributeProvider(attributeMapper, ClientValidationExceptionFactory),
                new MetadataAttributeProvider(ClientValidationExceptionFactory),
                new MethodParamBuilder(new ParamAttributeProvider(attributeMapper, ClientValidationExceptionFactory)));
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
            return BuildRequest(host: "http://localhost:5000", method, arguments);
        }

        internal IRequest BuildRequest(string host, IMethod method, params object[] arguments)
        {
            return RequestBuilder
                .BuildAsync(RequestId, resource: host, new MethodInvocation(method, arguments))
                .GetAwaiter()
                .GetResult();
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
            
            actualRequest.Endpoint.Should().Be(uri.ToString());
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
