using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Castle.DynamicProxy;
using FluentAssertions;
using NClient.Abstractions.HttpClients;
using NClient.Core.Helpers;
using NClient.Core.Helpers.ObjectMemberManagers;
using NClient.Core.Helpers.ObjectToKeyValueConverters;
using NClient.Core.Interceptors;
using NClient.Core.Mappers;
using NClient.Core.MethodBuilders;
using NClient.Core.MethodBuilders.Models;
using NClient.Core.MethodBuilders.Providers;
using NClient.Core.RequestBuilders;
using NUnit.Framework;

namespace NClient.Testing.Common
{
    public abstract class RequestBuilderTestBase
    {
        protected static readonly Guid RequestId = Guid.Parse("5bb86773-9999-483e-aa9a-3cce10e47fb1");

        internal MethodBuilder MethodBuilder = null!;
        internal RequestBuilder RequestBuilder = null!;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var objectMemberManager = new ObjectMemberManager();

            RequestBuilder = new RequestBuilder(
                host: new Uri("http://localhost:5000"),
                new RouteTemplateProvider(),
                new RouteProvider(objectMemberManager),
                new HttpMethodProvider(),
                new ObjectToKeyValueConverter(objectMemberManager));

            var attributeMapper = new AttributeMapper();
            MethodBuilder = new MethodBuilder(
                new MethodAttributeProvider(attributeMapper),
                new PathAttributeProvider(attributeMapper),
                new MethodParamBuilder(
                    new ParamAttributeProvider(attributeMapper)));
        }

        protected static MethodInfo GetMethodInfo<T>()
        {
            return typeof(T).GetMethods().First();
        }

        internal Method BuildMethod<T>()
        {
            return MethodBuilder.Build(typeof(T), GetMethodInfo<T>());
        }

        internal HttpRequest BuildRequest(Method method, params object[] arguments)
        {
            return RequestBuilder.Build(RequestId, method, arguments);
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
