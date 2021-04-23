using System.Linq;
using System.Reflection;
using FluentAssertions;
using Moq;
using NClient.Annotations.Parameters;
using NClient.Core.MethodBuilders;
using NClient.Core.MethodBuilders.Models;
using NClient.Core.MethodBuilders.Providers;
using NUnit.Framework;

namespace NClient.Standalone.Tests.MethodBuilders
{
    [Parallelizable]
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
            paramAttributeProvider.Setup(x => x.Get(It.IsAny<ParameterInfo>()))
                .Returns(paramAttribute);
            var methodBuilder = new MethodParamBuilder(paramAttributeProvider.Object);
            
            var actualResult = methodBuilder.Build(methodInfo);
            
            actualResult.Should().BeEquivalentTo(new MethodParam(
                paramInfo.Name!, 
                paramInfo.ParameterType,
                paramAttribute));
        }
    }
}