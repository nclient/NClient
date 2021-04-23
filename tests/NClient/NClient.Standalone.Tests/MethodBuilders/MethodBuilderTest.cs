using System;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Moq;
using NClient.Annotations;
using NClient.Annotations.Methods;
using NClient.Core.MethodBuilders;
using NClient.Core.MethodBuilders.Models;
using NClient.Core.MethodBuilders.Providers;
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
            var pathAttribute = (PathAttribute)null!;
            var methodParams = Array.Empty<MethodParam>();
            var methodAttributeProviderMock = new Mock<IMethodAttributeProvider>();
            methodAttributeProviderMock.Setup(x => x.Get(It.IsAny<MethodInfo>()))
                .Returns(methodAttribute);
            var pathAttributeProviderMock = new Mock<IPathAttributeProvider>();
            pathAttributeProviderMock.Setup(x => x.Find(It.IsAny<Type>()))
                .Returns(pathAttribute);
            var methodParamBuilderMock = new Mock<IMethodParamBuilder>();
            methodParamBuilderMock.Setup(x => x.Build(It.IsAny<MethodInfo>()))
                .Returns(methodParams);
            var methodBuilder = new MethodBuilder(
                methodAttributeProviderMock.Object,
                pathAttributeProviderMock.Object,
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