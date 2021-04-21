using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Castle.DynamicProxy;
using FluentAssertions;
using NClient.Abstractions.HttpClients;
using NClient.Core.Helpers;
using NClient.Core.Interceptors;
using NClient.Core.Mappers;
using NClient.Core.RequestBuilders;
using NUnit.Framework;

namespace NClient.Testing.Common
{
    public abstract class RequestBuilderTestBase
    {
        protected static readonly Guid RequestId = Guid.Parse("5bb86773-9999-483e-aa9a-3cce10e47fb1");

        internal RequestBuilder RequestBuilder = null!;
        internal KeepDataInterceptor KeepDataInterceptor = null!;

        internal IAttributeMapper AttributeMapper = null!;
        protected ProxyGenerator ProxyGenerator = null!;

        [SetUp]
        public void SetUp()
        {
            RequestBuilder = new RequestBuilder(
                host: new Uri("http://localhost:5000"),
                new RouteTemplateProvider(AttributeMapper),
                new RouteProvider(),
                new HttpMethodProvider(AttributeMapper),
                new ParameterProvider(AttributeMapper),
                new ObjectToKeyValueConverter());

            ProxyGenerator = new ProxyGenerator();
        }

        [OneTimeSetUp]
        public abstract void OneTimeSetUp();

        internal virtual HttpRequest BuildRequest(IInvocation invocation)
        {
            return RequestBuilder.Build(RequestId, invocation.Proxy.GetType().GetInterfaces().First(), invocation.Method, invocation.Arguments);
        }

        protected static void AssertHttpRequest(
            HttpRequest actualRequest,
            Uri uri,
            HttpMethod httpMethod,
            IEnumerable<HttpParameter>? parameters = null,
            IEnumerable<HttpHeader>? headers = null,
            object? body = null)
        {
            actualRequest.Uri.Should().Be(uri);
            actualRequest.Method.Should().Be(httpMethod);
            actualRequest.Parameters.Should().BeEquivalentTo(parameters ?? Array.Empty<HttpParameter>(), config => config.WithoutStrictOrdering());
            actualRequest.Headers.Should().BeEquivalentTo(headers ?? Array.Empty<HttpHeader>(), config => config.WithoutStrictOrdering());
            actualRequest.Body.Should().BeEquivalentTo(body);
        }
    }
}
