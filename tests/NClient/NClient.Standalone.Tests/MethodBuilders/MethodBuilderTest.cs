using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Moq;
using NClient.Annotations;
using NClient.Annotations.Http;
using NClient.Invocation;
using NClient.Standalone.ClientProxy.Generation.MethodBuilders;
using NClient.Standalone.ClientProxy.Generation.MethodBuilders.Providers;
using NUnit.Framework;
using TimeoutAttribute = NClient.Annotations.TimeoutAttribute;

namespace NClient.Standalone.Tests.MethodBuilders
{
    [Parallelizable]
    [SuppressMessage("ReSharper", "BadDeclarationBracesLineBreaks")]
    public class MethodBuilderTest
    {
        private interface IBasicClient { int Get(int id); }

        [Test]
        public void Build_BasicClient_Method()
        {
            var clientType = typeof(IBasicClient);
            var methodInfo = clientType.GetMethods().Single();
            var returnType = methodInfo.ReturnType;
            var methodAttribute = new GetMethodAttribute();
            var useVersionAttribute = (UseVersionAttribute) null!;
            var pathAttribute = (PathAttribute) null!;
            var timeoutAttribute = (TimeoutAttribute) null!;
            var headerAttributes = Array.Empty<HeaderAttribute>();
            var methodParams = Array.Empty<IMethodParam>();
            var methodAttributeProviderMock = new Mock<IOperationAttributeProvider>();
            methodAttributeProviderMock.Setup(x => x.Get(It.IsAny<MethodInfo>(), It.IsAny<IEnumerable<MethodInfo>>()))
                .Returns(methodAttribute);
            var useVersionAttributeProviderMock = new Mock<IUseVersionAttributeProvider>();
            useVersionAttributeProviderMock.Setup(x => x.Find(It.IsAny<Type>(), It.IsAny<MethodInfo>(), It.IsAny<IEnumerable<MethodInfo>>()))
                .Returns(useVersionAttribute);
            var pathAttributeProviderMock = new Mock<IPathAttributeProvider>();
            pathAttributeProviderMock.Setup(x => x.Find(It.IsAny<Type>()))
                .Returns(pathAttribute);
            var timeoutAttributeProviderMock = new Mock<ITimeoutAttributeProvider>();
            timeoutAttributeProviderMock.Setup(x => x.Find(It.IsAny<Type>(), It.IsAny<MethodInfo>(), It.IsAny<IEnumerable<MethodInfo>>()))
                .Returns(timeoutAttribute);
            var headerAttributeProviderMock = new Mock<IHeaderAttributeProvider>();
            headerAttributeProviderMock.Setup(x => x.Find(It.IsAny<Type>(), It.IsAny<MethodInfo>(), It.IsAny<IEnumerable<MethodInfo>>(), It.IsAny<MethodParam[]>()))
                .Returns(headerAttributes);
            var methodParamBuilderMock = new Mock<IMethodParamBuilder>();
            methodParamBuilderMock.Setup(x => x.Build(It.IsAny<MethodInfo>(), It.IsAny<IEnumerable<MethodInfo>>()))
                .Returns(methodParams);
            var methodBuilder = new MethodBuilder(
                methodAttributeProviderMock.Object,
                useVersionAttributeProviderMock.Object,
                pathAttributeProviderMock.Object,
                headerAttributeProviderMock.Object,
                timeoutAttributeProviderMock.Object,
                methodParamBuilderMock.Object);

            var actualResult = methodBuilder.Build(clientType, methodInfo, returnType);

            actualResult.Should().BeEquivalentTo(new Method(
                methodInfo.Name,
                methodInfo,
                clientType.Name,
                clientType,
                methodAttribute,
                methodParams,
                returnType));
        }
    }
}
