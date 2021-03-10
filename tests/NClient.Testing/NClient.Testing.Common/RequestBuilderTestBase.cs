using System;
using System.Collections.Generic;
using System.Net.Http;
using Castle.DynamicProxy;
using FluentAssertions;
using NClient.Core.Attributes;
using NClient.Core.Helpers;
using NClient.Core.Interceptors;
using NClient.Core.RequestBuilders;
using NClient.Providers.HttpClient;
using NUnit.Framework;

namespace NClient.Testing.Common
{
    public abstract class RequestBuilderTestBase
    {
        internal RequestBuilder RequestBuilder = null!;
        internal KeepDataInterceptor KeepDataInterceptor = null!;

        protected IAttributeHelper AttributeHelper = null!;
        protected ProxyGenerator ProxyGenerator = null!;

        [SetUp]
        public void SetUp()
        {
            RequestBuilder = new RequestBuilder(
                host: new Uri("http://localhost:5000"),
                new RouteTemplateProvider(AttributeHelper),
                new RouteProvider(AttributeHelper),
                new HttpMethodProvider(AttributeHelper),
                new ParameterProvider(AttributeHelper),
                new ObjectToKeyValueConverter(),
                AttributeHelper);

            ProxyGenerator = new ProxyGenerator();
        }

        [OneTimeSetUp]
        public abstract void OneTimeSetUp();

        internal virtual HttpRequest BuildRequest(IInvocation invocation)
        {
            return RequestBuilder.Build(invocation.Method.DeclaringType, invocation.Method, invocation.Arguments);
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
