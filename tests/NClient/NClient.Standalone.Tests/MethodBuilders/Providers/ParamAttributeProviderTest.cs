using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using NClient.Abstractions.Exceptions;
using NClient.Annotations.Parameters;
using NClient.Core.Exceptions.Factories;
using NClient.Core.Mappers;
using NClient.Core.MethodBuilders.Providers;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.Standalone.Tests.MethodBuilders.Providers
{
    [Parallelizable]
    public class ParamAttributeProviderTest
    {
        private interface IImplicitQuery { void Method(int id); }
        private interface IQuery { void Method([QueryParam] int id); }
        private interface IQueryBool { void Method([QueryParam] bool id); }
        private interface IQueryString { void Method([QueryParam] string id); }
        private interface IQueryCustomObject { void Method([QueryParam] BasicEntity entity); }
        private interface IQueryWithName { void Method([QueryParam(Name = "name")] int id); }

        private interface IImplicitBody { void Method(BasicEntity entity); }
        private interface IBody { void Method([BodyParam] int id); }
        private interface IBodyBool { void Method([BodyParam] bool id); }
        private interface IBodyString { void Method([BodyParam] string id); }
        private interface IBodyCustomObject { void Method([BodyParam] BasicEntity entity); }

        private interface IRoute { void Method([RouteParam] int id); }
        private interface IRouteBool { void Method([RouteParam] bool id); }
        private interface IRouteString { void Method([RouteParam] string id); }
        private interface IRouteCustomObject { void Method([RouteParam] BasicEntity entity); }
        private interface IRouteWithName { void Method([RouteParam(Name = "name")] int id); }

        private interface IHeader { void Method([HeaderParam] int id); }
        private interface IHeaderBool { void Method([HeaderParam] bool id); }
        private interface IHeaderString { void Method([HeaderParam] string id); }
        private interface IHeaderCustomObject { void Method([HeaderParam] BasicEntity entity); }
        private interface IHeaderWithName { void Method([HeaderParam(Name = "name")] int id); }

        private interface IOther { void Method([Other] int id); }
        private interface IOtherAndParam { void Method([Other, BodyParam] int id); }
        private interface INotSupported { void Method([NotSupported] int id); }

        public static IEnumerable ValidTestCases = new[]
        {
            new TestCaseData(GetParamInfo<IImplicitQuery>(), new QueryParamAttribute())
                .SetName("With implicit query"),
            new TestCaseData(GetParamInfo<IQuery>(), new QueryParamAttribute())
                .SetName("With query"),
            new TestCaseData(GetParamInfo<IQueryBool>(), new QueryParamAttribute())
                .SetName("With bool query"),
            new TestCaseData(GetParamInfo<IQueryString>(), new QueryParamAttribute())
                .SetName("With string query"),
            new TestCaseData(GetParamInfo<IQueryCustomObject>(), new QueryParamAttribute())
                .SetName("With custom object"),
            new TestCaseData(GetParamInfo<IQueryWithName>(), new QueryParamAttribute { Name = "name" })
                .SetName("With named query"),

            new TestCaseData(GetParamInfo<IImplicitBody>(), new BodyParamAttribute())
                .SetName("With implicit body"),
            new TestCaseData(GetParamInfo<IBody>(), new BodyParamAttribute())
                .SetName("With body"),
            new TestCaseData(GetParamInfo<IBodyBool>(), new BodyParamAttribute())
                .SetName("With bool body"),
            new TestCaseData(GetParamInfo<IBodyString>(), new BodyParamAttribute())
                .SetName("With string body"),
            new TestCaseData(GetParamInfo<IBodyCustomObject>(), new BodyParamAttribute())
                .SetName("With custom body"),

            new TestCaseData(GetParamInfo<IRoute>(), new RouteParamAttribute())
                .SetName("With route"),
            new TestCaseData(GetParamInfo<IRouteBool>(), new RouteParamAttribute())
                .SetName("With bool route"),
            new TestCaseData(GetParamInfo<IRouteString>(), new RouteParamAttribute())
                .SetName("With string route"),
            new TestCaseData(GetParamInfo<IRouteCustomObject>(), new RouteParamAttribute())
                .SetName("With custom route"),
            new TestCaseData(GetParamInfo<IRouteWithName>(), new RouteParamAttribute { Name = "name" })
                .SetName("With named route"),

            new TestCaseData(GetParamInfo<IHeader>(), new HeaderParamAttribute())
                .SetName("With header"),
            new TestCaseData(GetParamInfo<IHeaderBool>(), new HeaderParamAttribute())
                .SetName("With bool header"),
            new TestCaseData(GetParamInfo<IHeaderString>(), new HeaderParamAttribute())
                .SetName("With string header"),
            new TestCaseData(GetParamInfo<IHeaderCustomObject>(), new HeaderParamAttribute())
                .SetName("With custom header"),
            new TestCaseData(GetParamInfo<IHeaderWithName>(), new HeaderParamAttribute { Name = "name" })
                .SetName("With named header"),

            new TestCaseData(GetParamInfo<IOther>(), new QueryParamAttribute())
                .SetName("With other"),
            new TestCaseData(GetParamInfo<IOtherAndParam>(), new BodyParamAttribute())
                .SetName("With other and param"),
            new TestCaseData(GetParamInfo<INotSupported>(), new NotSupportedAttribute())
                .SetName("With not supported")
        };

        private interface INotSupportedAndParam { void Method([NotSupported, QueryParam] int id); }

        public static IEnumerable InvalidTestCases = new[]
        {
            new TestCaseData(GetParamInfo<INotSupportedAndParam>())
                .SetName("With not supported and param")
        };

        private ParamAttributeProvider _paramAttributeProvider = null!;

        [SetUp]
        public void SetUp()
        {
            var attributeMapper = new AttributeMapper();
            var clientRequestExceptionFactory = new ClientValidationExceptionFactory();
            _paramAttributeProvider = new ParamAttributeProvider(attributeMapper, clientRequestExceptionFactory);
        }

        [TestCaseSource(nameof(ValidTestCases))]
        public void Get_ValidTestCase_ParamAttribute(ParameterInfo parameterInfo, ParamAttribute expectedAttribute)
        {
            var actualAttribute = _paramAttributeProvider.Get(parameterInfo);

            actualAttribute.Should().BeEquivalentTo(expectedAttribute);
        }

        [TestCaseSource(nameof(InvalidTestCases))]
        public void Get_InvalidTestCase_ThrowNClientException(ParameterInfo parameterInfo)
        {
            _paramAttributeProvider
                .Invoking(x => x.Get(parameterInfo))
                .Should()
                .Throw<NClientException>();
        }

        private static ParameterInfo GetParamInfo<T>()
        {
            return typeof(T).GetMethods().Single().GetParameters().Single();
        }

        private class OtherAttribute : Attribute { }
        private class NotSupportedAttribute : ParamAttribute { }
    }
}