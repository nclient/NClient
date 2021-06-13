using System;
using System.Net;
using FluentAssertions;
using NClient.Annotations.Parameters;
using NClient.Annotations.Versioning;
using NClient.Core.AspNetRouting;
using NClient.Core.Exceptions;
using NClient.Core.Exceptions.Factories;
using NClient.Core.Helpers.ObjectMemberManagers;
using NClient.Core.RequestBuilders;
using NClient.Core.RequestBuilders.Models;
using NClient.Exceptions;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.Standalone.Tests.RouteProviderTests
{
    [Parallelizable]
    public class RouteProviderTest
    {
        private static readonly ClientValidationExceptionFactory ClientValidationExceptionFactory = new();

        internal RouteProvider RouteProvider = null!;

        [SetUp]
        public void SetUp()
        {
            var objectMemberManager = new ObjectMemberManager(ClientValidationExceptionFactory);
            RouteProvider = new RouteProvider(objectMemberManager, ClientValidationExceptionFactory);
        }

        [Test]
        public void Build_WithoutTemplate_EmptyString()
        {
            var routeTemplate = TemplateParser.Parse("");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IMyClient",
                methodName: "Method",
                parameters: Array.Empty<Parameter>(),
                useVersionAttribute: null);

            route.Should().BeEmpty();
        }

        [Test]
        public void Build_StaticTemplate_TemplateWithoutChanges()
        {
            var routeTemplate = TemplateParser.Parse("api");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IMyClient",
                methodName: "Method",
                parameters: Array.Empty<Parameter>(),
                useVersionAttribute: null);

            route.Should().Be("api");
        }

        [Test]
        public void Build_ControllerTokenForClientInterface_ClientNameWithoutPrefixAndSuffix()
        {
            var routeTemplate = TemplateParser.Parse("[controller]");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IMyClient",
                methodName: "Method",
                parameters: Array.Empty<Parameter>(),
                useVersionAttribute: null);

            route.Should().Be("My");
        }

        [Test]
        public void Build_ControllerTokenForClientInterfaceLowerCase_ClientNameWithoutPrefix()
        {
            var routeTemplate = TemplateParser.Parse("[controller]");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IMyclient",
                methodName: "Method",
                parameters: Array.Empty<Parameter>(),
                useVersionAttribute: null);

            route.Should().Be("Myclient");
        }

        [Test]
        public void Build_ControllerTokenForControllerInterface_ControllerNameWithoutPrefixAndSuffix()
        {
            var routeTemplate = TemplateParser.Parse("[controller]");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IMyController",
                methodName: "Method",
                parameters: Array.Empty<Parameter>(),
                useVersionAttribute: null);

            route.Should().Be("My");
        }

        [Test]
        public void Build_ControllerTokenForControllerInterfaceLowerCase_ControllerNameWithoutPrefix()
        {
            var routeTemplate = TemplateParser.Parse("[controller]");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IMycontroller",
                methodName: "Method",
                parameters: Array.Empty<Parameter>(),
                useVersionAttribute: null);

            route.Should().Be("Mycontroller");
        }

        [Test]
        public void Build_ControllerTokenForClientClass_ClientNameWithoutSuffix()
        {
            var routeTemplate = TemplateParser.Parse("[controller]");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "MyClient",
                methodName: "Method",
                parameters: Array.Empty<Parameter>(),
                useVersionAttribute: null);

            route.Should().Be("My");
        }

        [Test]
        public void Build_ControllerTokenForClientClassLowerCase_ClientName()
        {
            var routeTemplate = TemplateParser.Parse("[controller]");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "Myclient",
                methodName: "Method",
                parameters: Array.Empty<Parameter>(),
                useVersionAttribute: null);

            route.Should().Be("Myclient");
        }

        [Test]
        public void Build_ControllerTokenForControllerClass_ControllerNameWithoutSuffix()
        {
            var routeTemplate = TemplateParser.Parse("[controller]");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "MyController",
                methodName: "Method",
                parameters: Array.Empty<Parameter>(),
                useVersionAttribute: null);

            route.Should().Be("My");
        }

        [Test]
        public void Build_ControllerTokenForControllerClassLowerCase_ControllerNameWithoutSuffix()
        {
            var routeTemplate = TemplateParser.Parse("[controller]");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "Mycontroller",
                methodName: "Method",
                parameters: Array.Empty<Parameter>(),
                useVersionAttribute: null);

            route.Should().Be("Mycontroller");
        }

        [Test]
        public void Build_StaticPartWithControllerToken_StaticPartWithInterfaceNameWithoutPrefix()
        {
            var routeTemplate = TemplateParser.Parse("api/[controller]");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IMyClient",
                methodName: "Method",
                parameters: Array.Empty<Parameter>(),
                useVersionAttribute: null);

            route.Should().Be("api/My");
        }

        [Test]
        public void Build_ActionToken_MethodName()
        {
            var routeTemplate = TemplateParser.Parse("[action]");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IMyClient",
                methodName: "Method",
                parameters: Array.Empty<Parameter>(),
                useVersionAttribute: null);

            route.Should().Be("Method");
        }

        [Test]
        public void Build_StaticPartWithActionToken_StaticPartWithMethodName()
        {
            var routeTemplate = TemplateParser.Parse("api/[action]");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IMyClient",
                methodName: "Method",
                parameters: Array.Empty<Parameter>(),
                useVersionAttribute: null);

            route.Should().Be("api/Method");
        }

        [Test]
        public void Build_StaticPartWithControllerTokenWithActionToken_StaticPartWithInterfaceNameWithMethodName()
        {
            var routeTemplate = TemplateParser.Parse("api/[controller]/[action]");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IMyClient",
                methodName: "Method",
                parameters: Array.Empty<Parameter>(),
                useVersionAttribute: null);

            route.Should().Be("api/My/Method");
        }

        [Test]
        public void Build_VersionToken_VersionAttributeValue()
        {
            const string version = "1.0";
            var routeTemplate = TemplateParser.Parse("{version:apiVersion}");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IMyClient",
                methodName: "Method",
                parameters: Array.Empty<Parameter>(),
                useVersionAttribute: new UseVersionAttribute(version));

            route.Should().Be(version);
        }

        [Test]
        public void Build_PrimitiveParameterToken_MethodParameterValue()
        {
            var intValue = 1;
            var routeTemplate = TemplateParser.Parse("{id}");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IMyClient",
                methodName: "Method",
                parameters: new[]
                {
                    new Parameter("id", intValue.GetType(), intValue, new RouteParamAttribute())
                },
                useVersionAttribute: null);

            route.Should().Be(intValue.ToString());
        }

        [Test]
        public void Build_ParameterTokenWithStaticPart_MethodParameterValueWithStaticPart()
        {
            var intValue = 1;
            var routeTemplate = TemplateParser.Parse("id-{id}");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IMyClient",
                methodName: "Method",
                parameters: new[]
                {
                    new Parameter("id", intValue.GetType(), intValue, new RouteParamAttribute())
                },
                useVersionAttribute: null);

            route.Should().Be($"id-{intValue.ToString()}");
        }

        // TODO: Move type tests to a individual test class
        [Test]
        public void Build_StringParameterToken_MethodParameterValue()
        {
            var stringValue = "str";
            var routeTemplate = TemplateParser.Parse("{id}");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IMyClient",
                methodName: "Method",
                parameters: new[]
                {
                    new Parameter("id", stringValue.GetType(), stringValue, new RouteParamAttribute())
                },
                useVersionAttribute: null);

            route.Should().Be(stringValue);
        }

        [Test]
        public void Build_DecimalParameterToken_MethodParameterValue()
        {
            var decimalValue = 1.2d;
            var routeTemplate = TemplateParser.Parse("{id}");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IMyClient",
                methodName: "Method",
                parameters: new[]
                {
                    new Parameter("id", decimalValue.GetType(), decimalValue, new RouteParamAttribute())
                },
                useVersionAttribute: null);

            route.Should().Be(decimalValue.ToString());
        }

        [Test]
        public void Build_GuidParameterToken_MethodParameterValue()
        {
            var guidValue = Guid.Parse("1328ec48-3622-4fe0-bafc-84a5e12f6e23");
            var routeTemplate = TemplateParser.Parse("{id}");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IMyClient",
                methodName: "Method",
                parameters: new[]
                {
                    new Parameter("id", guidValue.GetType(), guidValue, new RouteParamAttribute())
                },
                useVersionAttribute: null);

            route.Should().Be(guidValue.ToString());
        }

        [Test]
        public void Build_EnumParameterToken_MethodParameterValue()
        {
            var enumValue = HttpStatusCode.OK;
            var routeTemplate = TemplateParser.Parse("{id}");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IMyClient",
                methodName: "Method",
                parameters: new[]
                {
                    new Parameter("id", enumValue.GetType(), enumValue, new RouteParamAttribute())
                },
                useVersionAttribute: null);

            route.Should().Be(enumValue.ToString());
        }

        [Test]
        public void Build_StaticPartWithParameterToken_IStaticPartWithMethodParameterValue()
        {
            var routeTemplate = TemplateParser.Parse("api/{id}");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IMyClient",
                methodName: "Method",
                parameters: new[]
                {
                    new Parameter("id", typeof(int), 1, new RouteParamAttribute())
                },
                useVersionAttribute: null);

            route.Should().Be("api/1");
        }

        [Test]
        public void Build_ConstrainedParameterToken_MethodParameterValue()
        {
            var routeTemplate = TemplateParser.Parse("{id:int}");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IMyClient",
                methodName: "Method",
                parameters: new[]
                {
                    new Parameter("id", typeof(int), 1, new RouteParamAttribute())
                },
                useVersionAttribute: null);

            route.Should().Be("1");
        }

        [Test]
        public void Build_NotFitConstrainedParameterToken_MethodParameterValue()
        {
            var routeTemplate = TemplateParser.Parse("{id:uint}");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IMyClient",
                methodName: "Method",
                parameters: new[]
                {
                    new Parameter("id", typeof(int), int.MaxValue, new RouteParamAttribute())
                },
                useVersionAttribute: null);

            route.Should().Be(int.MaxValue.ToString());
        }

        [Test]
        public void Build_BodyCustomObjectPropertyToken_ObjectPropertyValue()
        {
            const int id = 1;
            var routeTemplate = TemplateParser.Parse("{entity.Id}");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IMyClient",
                methodName: "Method",
                parameters: new[]
                {
                    new Parameter("entity", typeof(BasicEntity), new BasicEntity { Id = id , Value = 2 }, new BodyParamAttribute())
                },
                useVersionAttribute: null);

            route.Should().Be(id.ToString());
        }

        [Test]
        public void Build_BodyCustomObjectPropertyTokenWithCustomName_ObjectPropertyValue()
        {
            const int id = 1;
            var routeTemplate = TemplateParser.Parse("{entity.MyId}");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IMyClient",
                methodName: "Method",
                parameters: new[]
                {
                    new Parameter("entity", typeof(BasicEntity), new BasicEntityWithCustomJsonName { Id = id , Value = 2 }, new BodyParamAttribute())
                },
                useVersionAttribute: null);

            route.Should().Be(id.ToString());
        }

        [Test]
        public void Build_BodyCustomObjectPropertyTokenWithInvalidObjectCase_ThrowClientValidationException()
        {
            var routeTemplate = TemplateParser.Parse("{Entity.Id}");

            RouteProvider
                .Invoking(x => x.Build(
                    routeTemplate,
                    clientName: "IMyClient",
                    methodName: "Method",
                    parameters: new[]
                    {
                        new Parameter("entity", typeof(BasicEntity), new BasicEntity { Id = 1 , Value = 2 }, new BodyParamAttribute())
                    },
                    useVersionAttribute: null))
                .Should()
                .Throw<ClientValidationException>()
                .WithMessage(ClientValidationExceptionFactory.TokenNotMatchAnyMethodParameter("Entity").Message);
        }

        [Test]
        public void Build_BodyCustomObjectPropertyTokenWithInvalidPropertyCase_ThrowClientValidationException()
        {
            var routeTemplate = TemplateParser.Parse("{entity.id}");

            RouteProvider
                .Invoking(x => x.Build(
                    routeTemplate,
                    clientName: "IMyClient",
                    methodName: "Method",
                    parameters: new[]
                    {
                        new Parameter("entity", typeof(BasicEntity), new BasicEntity { Id = 1 , Value = 2 }, new BodyParamAttribute())
                    },
                    useVersionAttribute: null))
                .Should()
                .Throw<ClientValidationException>()
                .WithMessage(ClientValidationExceptionFactory.MemberNotFound("id", "BasicEntity").Message);
        }

        [Test]
        public void Build_QueryCustomObjectPropertyToken_ObjectPropertyValue()
        {
            const int id = 1;
            var routeTemplate = TemplateParser.Parse("{entity.Id}");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IMyClient",
                methodName: "Method",
                parameters: new[]
                {
                    new Parameter("entity", typeof(BasicEntity), new BasicEntity { Id = id , Value = 2 }, new QueryParamAttribute())
                },
                useVersionAttribute: null);

            route.Should().Be(id.ToString());
        }

        [Test]
        public void Build_QueryCustomObjectPropertyTokenWithCustomName_ObjectPropertyValue()
        {
            const int id = 1;
            var routeTemplate = TemplateParser.Parse("{entity.MyId}");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IMyClient",
                methodName: "Method",
                parameters: new[]
                {
                    new Parameter("entity", typeof(BasicEntity), new BasicEntityWithCustomQueryName { Id = id , Value = 2 }, new QueryParamAttribute())
                },
                useVersionAttribute: null);

            route.Should().Be(id.ToString());
        }

        [Test]
        public void Build_QueryCustomObjectPropertyTokenWithCustomAspNetName_ObjectPropertyValue()
        {
            const int id = 1;
            var routeTemplate = TemplateParser.Parse("{entity.MyId}");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IMyClient",
                methodName: "Method",
                parameters: new[]
                {
                    new Parameter("entity", typeof(BasicEntity), new BasicEntityWithCustomFromQueryName { Id = id , Value = 2 }, new QueryParamAttribute())
                },
                useVersionAttribute: null);

            route.Should().Be(id.ToString());
        }

        [Test]
        public void Build_QueryCustomObjectPropertyTokenWithInvalidObjectCase_ThrowClientValidationException()
        {
            var routeTemplate = TemplateParser.Parse("{Entity.Id}");

            RouteProvider
                .Invoking(x => x.Build(
                    routeTemplate,
                    clientName: "IMyClient",
                    methodName: "Method",
                    parameters: new[]
                    {
                        new Parameter("entity", typeof(BasicEntity), new BasicEntity { Id = 1 , Value = 2 }, new QueryParamAttribute())
                    },
                    useVersionAttribute: null))
                .Should()
                .Throw<ClientValidationException>()
                .WithMessage(ClientValidationExceptionFactory.TokenNotMatchAnyMethodParameter("Entity").Message);
        }

        [Test]
        public void Build_QueryCustomObjectPropertyTokenWithInvalidPropertyCase_ThrowClientValidationException()
        {
            var routeTemplate = TemplateParser.Parse("{entity.id}");

            RouteProvider
                .Invoking(x => x.Build(
                    routeTemplate,
                    clientName: "IMyClient",
                    methodName: "Method",
                    parameters: new[]
                    {
                        new Parameter("entity", typeof(BasicEntity), new BasicEntity { Id = 1 , Value = 2 }, new QueryParamAttribute())
                    },
                    useVersionAttribute: null))
                .Should()
                .Throw<ClientValidationException>()
                .WithMessage(ClientValidationExceptionFactory.MemberNotFound("id", "BasicEntity").Message);
        }

        [Test]
        public void Build_ControllerNameConsistsOnlyOfSuffixesAndPrefixes_ThrowClientValidationException()
        {
            var routeTemplate = TemplateParser.Parse("[controller]");

            RouteProvider
                .Invoking(x => x.Build(
                    routeTemplate,
                    clientName: "IClient",
                    methodName: "Method",
                    parameters: Array.Empty<Parameter>(),
                    useVersionAttribute: null))
                .Should()
                .Throw<ClientValidationException>()
                .WithMessage(ClientValidationExceptionFactory.ClientNameConsistsOnlyOfSuffixesAndPrefixes().Message);
        }

        [Test]
        public void Build_AllTypesOfTokensWithStaticPart_StaticPartWithReplacedTokens()
        {
            var routeTemplate = TemplateParser.Parse("api/[controller]/[action]/{id}");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IMyClient",
                methodName: "Method",
                parameters: new[]
                {
                    new Parameter("id", typeof(int), 1, new RouteParamAttribute())
                },
                useVersionAttribute: null);

            route.Should().Be("api/My/Method/1");
        }

        [Test]
        public void Build_WrongControllerToken_ThrowClientValidationException()
        {
            var routeTemplate = TemplateParser.Parse("[controller1]");

            RouteProvider
                .Invoking(x => x.Build(
                    routeTemplate,
                    clientName: "IMyClient",
                    methodName: "Method",
                    parameters: Array.Empty<Parameter>(),
                    useVersionAttribute: null))
                .Should()
                .Throw<ClientValidationException>()
                .WithMessage(ClientValidationExceptionFactory.SpecialTokenFromTemplateNotExists("[controller1]").Message);
        }

        [Test]
        public void Build_WrongActionToken_ThrowClientValidationException()
        {
            var routeTemplate = TemplateParser.Parse("[action1]");

            RouteProvider
                .Invoking(x => x.Build(
                    routeTemplate,
                    clientName: "IMyClient",
                    methodName: "Method",
                    parameters: Array.Empty<Parameter>(),
                    useVersionAttribute: null))
                .Should()
                .Throw<ClientValidationException>()
                .WithMessage(ClientValidationExceptionFactory.SpecialTokenFromTemplateNotExists("[action1]").Message);
        }

        [Test, Ignore("Use mock for RouteTemplate")]
        public void Build_DuplicateParameterTokens_ThrowClientValidationException()
        {
            var routeTemplate = TemplateParser.Parse("{id}/{id}");

            RouteProvider
                .Invoking(x => x.Build(
                    routeTemplate,
                    clientName: "IMyClient",
                    methodName: "Method",
                    parameters: new[]
                    {
                        new Parameter("id", typeof(int), 1, new RouteParamAttribute())
                    },
                    useVersionAttribute: null))
                .Should()
                .Throw<ClientValidationException>()
                .WithMessage("");
        }

        [Test]
        public void Build_NotExistsParameterToken_ThrowClientValidationException()
        {
            var routeTemplate = TemplateParser.Parse("{prop}");

            RouteProvider
                .Invoking(x => x.Build(
                    routeTemplate,
                    clientName: "IMyClient",
                    methodName: "Method",
                    parameters: new[]
                    {
                        new Parameter("id", typeof(int), int.MaxValue, new RouteParamAttribute())
                    },
                    useVersionAttribute: null))
                .Should()
                .Throw<ClientValidationException>()
                .WithMessage(ClientValidationExceptionFactory.RouteParamWithoutTokenInRoute("id").Message);
        }

        [Test]
        public void Build_CustomTypeParameterToken_ThrowClientValidationException()
        {
            var routeTemplate = TemplateParser.Parse("{entity}");

            RouteProvider
                .Invoking(x => x.Build(
                    routeTemplate,
                    clientName: "IMyClient",
                    methodName: "Method",
                    parameters: new[]
                    {
                        new Parameter("entity", typeof(BasicEntity), new BasicEntity { Id = 1, Value = 2 }, new RouteParamAttribute())
                    },
                    useVersionAttribute: null))
                .Should()
                .Throw<ClientValidationException>()
                .WithMessage(ClientValidationExceptionFactory.TemplatePartContainsComplexType("entity").Message);
        }

        [Test]
        public void Build_VersionTokenWithoutVersionAttribute_ThrowClientValidationException()
        {
            var routeTemplate = TemplateParser.Parse("{version:apiVersion}");

            RouteProvider
                .Invoking(x => x.Build(
                    routeTemplate,
                    clientName: "IMyClient",
                    methodName: "Method",
                    parameters: Array.Empty<Parameter>(),
                    useVersionAttribute: null))
                .Should()
                .Throw<ClientValidationException>()
                .WithMessage(ClientValidationExceptionFactory.UsedVersionTokenButVersionAttributeNotFound().Message);
        }

        [Test]
        public void Build_VersionTokenWithoutType_ThrowClientValidationException()
        {
            const string version = "1.0";
            var routeTemplate = TemplateParser.Parse("{version}");

            RouteProvider
                .Invoking(x => x.Build(
                    routeTemplate,
                    clientName: "IMyClient",
                    methodName: "Method",
                    parameters: Array.Empty<Parameter>(),
                    useVersionAttribute: new UseVersionAttribute(version)))
                .Should()
                .Throw<ClientValidationException>()
                .WithMessage(ClientValidationExceptionFactory.TokenNotMatchAnyMethodParameter("version").Message);
        }
    }
}
