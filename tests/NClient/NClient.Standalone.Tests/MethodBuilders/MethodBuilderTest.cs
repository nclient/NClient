using System;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Moq;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Annotations.Versioning;
using NClient.Core.Interceptors.MethodBuilders;
using NClient.Core.Interceptors.MethodBuilders.Models;
using NClient.Core.Interceptors.MethodBuilders.Providers;
using NUnit.Framework;

namespace NClient.Standalone.Tests.MethodBuilders
{
    [Parallelizable]
    public class MethodBuilderTest
    {
        private interface IBasicClient { int Get(int id); }

        [Test]
        public void Build_BasicClient_Method()
        {
            var clientType = typeof(IBasicClient);
            var methodInfo = clientType.GetMethods().Single();
            var methodAttribute = new GetMethodAttribute();
            var useVersionAttribute = (UseVersionAttribute)null!;
            var pathAttribute = (PathAttribute)null!;
            var headerAttributes = Array.Empty<HeaderAttribute>();
            var methodParams = Array.Empty<MethodParam>();
            var methodAttributeProviderMock = new Mock<IMethodAttributeProvider>();
            methodAttributeProviderMock.Setup(x => x.Get(It.IsAny<MethodInfo>()))
                .Returns(methodAttribute);
            var useVersionAttributeProviderMock = new Mock<IUseVersionAttributeProvider>();
            useVersionAttributeProviderMock.Setup(x => x.Find(It.IsAny<Type>(), It.IsAny<MethodInfo>()))
                .Returns(useVersionAttribute);
            var pathAttributeProviderMock = new Mock<IPathAttributeProvider>();
            pathAttributeProviderMock.Setup(x => x.Find(It.IsAny<Type>()))
                .Returns(pathAttribute);
            var headerAttributeProviderMock = new Mock<IHeaderAttributeProvider>();
            headerAttributeProviderMock.Setup(x => x.Get(It.IsAny<Type>(), It.IsAny<MethodInfo>(), It.IsAny<MethodParam[]>()))
                .Returns(headerAttributes);
            var methodParamBuilderMock = new Mock<IMethodParamBuilder>();
            methodParamBuilderMock.Setup(x => x.Build(It.IsAny<MethodInfo>()))
                .Returns(methodParams);
            var methodBuilder = new MethodBuilder(
                methodAttributeProviderMock.Object,
                useVersionAttributeProviderMock.Object,
                pathAttributeProviderMock.Object,
                headerAttributeProviderMock.Object,
                methodParamBuilderMock.Object);

            var actualResult = methodBuilder.Build(clientType, methodInfo);

            actualResult.Should().BeEquivalentTo(new Method(
                methodInfo.Name,
                clientType.Name,
                methodAttribute,
                methodParams));
        }
    }
}