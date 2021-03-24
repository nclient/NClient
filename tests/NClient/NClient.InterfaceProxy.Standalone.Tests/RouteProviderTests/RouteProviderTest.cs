﻿using System;
using FluentAssertions;
using Microsoft.AspNetCore.Routing.Template;
using NClient.Annotations.Parameters;
using NClient.Core.Exceptions;
using NClient.Core.RequestBuilders;
using NClient.Core.RequestBuilders.Models;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.InterfaceProxy.Standalone.Tests.RouteProviderTests
{
    [Parallelizable]
    public class RouteProviderTest
    {
        internal RouteProvider RouteProvider = null!;

        [SetUp]
        public void SetUp()
        {
            RouteProvider = new RouteProvider();
        }

        [Test]
        public void Build_WithoutTemplate_EmptyString()
        {
            var routeTemplate = TemplateParser.Parse("");

            var route = RouteProvider.Build(
                routeTemplate, 
                clientName: "IClient", 
                methodName: "Method", 
                parameters: Array.Empty<Parameter>());

            route.Should().BeEmpty();
        }

        [Test]
        public void Build_StaticTemplate_TemplateWithoutChanges()
        {
            var routeTemplate = TemplateParser.Parse("api");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IClient",
                methodName: "Method",
                parameters: Array.Empty<Parameter>());

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
                parameters: Array.Empty<Parameter>());

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
                parameters: Array.Empty<Parameter>());

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
                parameters: Array.Empty<Parameter>());

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
                parameters: Array.Empty<Parameter>());

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
                parameters: Array.Empty<Parameter>());

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
                parameters: Array.Empty<Parameter>());

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
                parameters: Array.Empty<Parameter>());

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
                parameters: Array.Empty<Parameter>());

            route.Should().Be("Mycontroller");
        }

        [Test]
        public void Build_StaticPartWithControllerToken_StaticPartWithInterfaceNameWithoutPrefix()
        {
            var routeTemplate = TemplateParser.Parse("api/[controller]");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IClient",
                methodName: "Method",
                parameters: Array.Empty<Parameter>());

            route.Should().Be("api/Client");
        }

        [Test]
        public void Build_ActionToken_MethodName()
        {
            var routeTemplate = TemplateParser.Parse("[action]");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IClient",
                methodName: "Method",
                parameters: Array.Empty<Parameter>());

            route.Should().Be("Method");
        }

        [Test]
        public void Build_StaticPartWithActionToken_StaticPartWithMethodName()
        {
            var routeTemplate = TemplateParser.Parse("api/[action]");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IClient",
                methodName: "Method",
                parameters: Array.Empty<Parameter>());

            route.Should().Be("api/Method");
        }

        [Test]
        public void Build_StaticPartWithControllerTokenWithActionToken_StaticPartWithInterfaceNameWithMethodName()
        {
            var routeTemplate = TemplateParser.Parse("api/[controller]/[action]");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IClient",
                methodName: "Method",
                parameters: Array.Empty<Parameter>());

            route.Should().Be("api/Client/Method");
        }

        [Test]
        public void Build_ParameterToken_MethodParameterValue()
        {
            var routeTemplate = TemplateParser.Parse("{id}");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IClient",
                methodName: "Method",
                parameters: new []
                {
                    new Parameter("id", typeof(int), 1, new RouteParamAttribute())
                });

            route.Should().Be("1");
        }

        [Test]
        public void Build_StaticPartWithParameterToken_IStaticPartWithMethodParameterValue()
        {
            var routeTemplate = TemplateParser.Parse("api/{id}");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IClient",
                methodName: "Method",
                parameters: new[]
                {
                    new Parameter("id", typeof(int), 1, new RouteParamAttribute())
                });

            route.Should().Be("api/1");
        }

        [Test]
        public void Build_ConstrainedParameterToken_MethodParameterValue()
        {
            var routeTemplate = TemplateParser.Parse("{id:int}");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IClient",
                methodName: "Method",
                parameters: new[]
                {
                    new Parameter("id", typeof(int), 1, new RouteParamAttribute())
                });

            route.Should().Be("1");
        }

        [Test]
        public void Build_NotFitConstrainedParameterToken_MethodParameterValue()
        {
            var routeTemplate = TemplateParser.Parse("{id:uint}");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IClient",
                methodName: "Method",
                parameters: new[]
                {
                    new Parameter("id", typeof(int), int.MaxValue, new RouteParamAttribute())
                });

            route.Should().Be(int.MaxValue.ToString());
        }

        [Test]
        public void Build_AllTypesOfTokensWithStaticPart_StaticPartWithReplacedTokens()
        {
            var routeTemplate = TemplateParser.Parse("api/[controller]/[action]/{id}");

            var route = RouteProvider.Build(
                routeTemplate,
                clientName: "IClient",
                methodName: "Method",
                parameters: new[]
                {
                    new Parameter("id", typeof(int), 1, new RouteParamAttribute())
                });

            route.Should().Be("api/Client/Method/1");
        }

        [Test]
        public void Build_WrongControllerToken_ThrowInvalidRouteNClientException()
        {
            var routeTemplate = TemplateParser.Parse("[controller1]");

            RouteProvider
                .Invoking(x => x.Build(
                    routeTemplate,
                    clientName: "IClient",
                    methodName: "Method",
                    parameters: Array.Empty<Parameter>()))
                .Should()
                .Throw<InvalidRouteNClientException>();
        }

        [Test]
        public void Build_WrongActionToken_ThrowInvalidRouteNClientException()
        {
            var routeTemplate = TemplateParser.Parse("[action1]");

            RouteProvider
                .Invoking(x => x.Build(
                    routeTemplate,
                    clientName: "IClient",
                    methodName: "Method",
                    parameters: Array.Empty<Parameter>()))
                .Should()
                .Throw<InvalidRouteNClientException>();
        }

        [Test, Ignore("Use mock for RouteTemplate")]
        public void Build_DuplicateParameterTokens_ThrowInvalidRouteNClientException()
        {
            var routeTemplate = TemplateParser.Parse("{id}/{id}");

            RouteProvider
                .Invoking(x => x.Build(
                    routeTemplate,
                    clientName: "IClient",
                    methodName: "Method",
                    parameters: new[]
                    {
                        new Parameter("id", typeof(int), 1, new RouteParamAttribute())
                    }))
                .Should()
                .Throw<InvalidRouteNClientException>();
        }

        [Test]
        public void Build_NotExistsParameterToken_ThrowInvalidRouteNClientException()
        {
            var routeTemplate = TemplateParser.Parse("{prop}");

            RouteProvider
                .Invoking(x => x.Build(
                    routeTemplate,
                    clientName: "IClient",
                    methodName: "Method",
                    parameters: new[]
                    {
                        new Parameter("id", typeof(int), int.MaxValue, new RouteParamAttribute())
                    }))
                .Should()
                .Throw<InvalidRouteNClientException>();
        }

        [Test]
        public void Build_CustomTypeParameterToken_ThrowInvalidRouteNClientException()
        {
            var routeTemplate = TemplateParser.Parse("{entity}");

            RouteProvider
                .Invoking(x => x.Build(
                    routeTemplate,
                    clientName: "IClient",
                    methodName: "Method",
                    parameters: new[]
                    {
                        new Parameter("entity", typeof(BasicEntity), new BasicEntity { Id = 1, Value = 2 }, new RouteParamAttribute())
                    }))
                .Should()
                .Throw<InvalidRouteNClientException>();
        }
    }
}
