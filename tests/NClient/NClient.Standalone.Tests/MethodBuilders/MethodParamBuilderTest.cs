using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Moq;
using NClient.Annotations.Http;
using NClient.Invocation;
using NClient.Standalone.ClientProxy.Generation.MethodBuilders;
using NClient.Standalone.ClientProxy.Generation.MethodBuilders.Providers;
using NUnit.Framework;

namespace NClient.Standalone.Tests.MethodBuilders
{
    [Parallelizable]
    [SuppressMessage("ReSharper", "BadDeclarationBracesLineBreaks")]
    public class MethodParamBuilderTest
    {
        private interface IBasicClient { int Get(int id); }

        [Test]
        public void Build_BasicClient_Method()
        {
            var methodInfo = typeof(IBasicClient).GetMethods().Single();
            var paramInfo = methodInfo.GetParameters().Single();
            var paramAttribute = new QueryParamAttribute();
            var paramAttributeProvider = new Mock<IParamAttributeProvider>();
            paramAttributeProvider.Setup(x => x.Get(It.IsAny<ParameterInfo>(), It.IsAny<IEnumerable<ParameterInfo>>()))
                .Returns(paramAttribute);
            var methodBuilder = new MethodParamBuilder(paramAttributeProvider.Object);

            var actualResult = methodBuilder.Build(methodInfo, overridingMethods: Array.Empty<MethodInfo>());

            actualResult.Should().BeEquivalentTo(new[] 
            { 
                new MethodParam(paramInfo.Name!, paramInfo.ParameterType, paramAttribute)
            });
        }
    }
}
