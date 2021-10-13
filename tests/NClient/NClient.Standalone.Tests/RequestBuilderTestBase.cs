﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using FluentAssertions;
using NClient.Abstractions.HttpClients;
using NClient.Core.Helpers.ObjectMemberManagers;
using NClient.Core.Mappers;
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
        internal IClientArgumentExceptionFactory ClientArgumentExceptionFactory = null!;
        internal IClientValidationExceptionFactory ClientValidationExceptionFactory = null!;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var objectMemberManager = new ObjectMemberManager(new ClientObjectMemberManagerExceptionFactory());

            ClientArgumentExceptionFactory = new ClientArgumentExceptionFactory();
            ClientValidationExceptionFactory = new ClientValidationExceptionFactory();
            RequestBuilder = new RequestBuilder(
                new RouteTemplateProvider(ClientValidationExceptionFactory),
                new RouteProvider(objectMemberManager, ClientArgumentExceptionFactory, ClientValidationExceptionFactory),
                new HttpMethodProvider(ClientValidationExceptionFactory),
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

        internal IHttpRequest BuildRequest(Method method, params object[] arguments)
        {
            return BuildRequest(host: "http://localhost:5000", method, arguments);
        }

        internal IHttpRequest BuildRequest(string host, Method method, params object[] arguments)
        {
            return RequestBuilder.Build(RequestId, host: new Uri(host), method, arguments);
        }

        protected static void AssertHttpRequest(
            IHttpRequest actualRequest,
            Uri uri,
            HttpMethod httpMethod,
            IEnumerable<IHttpParameter>? parameters = null,
            IEnumerable<IHttpHeader>? headers = null,
            object? body = null)
        {
            actualRequest.Resource.Should().Be(uri);
            actualRequest.Method.Should().Be(httpMethod);
            actualRequest.Parameters.Should().BeEquivalentTo(parameters ?? Array.Empty<IHttpParameter>(), config => config.WithoutStrictOrdering());
            actualRequest.Headers.Should().BeEquivalentTo(headers ?? Array.Empty<IHttpHeader>(), config => config.WithoutStrictOrdering());
            actualRequest.Data.Should().BeEquivalentTo(body);
        }
    }
}
