using Castle.DynamicProxy;
using FluentAssertions;
using NClient.Core.Exceptions;
using NClient.Core.Interceptors;
using NClient.Core.RequestBuilders;
using NClient.Core.RequestBuilders.Models;
using NClient.InterfaceProxy.Attributes;
using NClient.InterfaceProxy.Attributes.Parameters;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.InterfaceProxy.Standalone.Tests.ParameterProviderTests
{
    [Parallelizable]
    public class ParameterProviderTest
    {
        internal ParameterProvider ParameterProvider = null!;
        internal KeepDataInterceptor KeepDataInterceptor = null!;
        protected ProxyGenerator ProxyGenerator = null!;

        [SetUp]
        public void SetUp()
        {
            var attributeHelper = new AttributeHelper();
            ParameterProvider = new ParameterProvider(attributeHelper);

            ProxyGenerator = new ProxyGenerator();
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            KeepDataInterceptor = new KeepDataInterceptor();
        }

        public interface IPrimitiveWithoutAttribute { void Method(int id); }

        [Test]
        public void Get_PrimitiveWithoutAttribute_QueryParam()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IPrimitiveWithoutAttribute>(KeepDataInterceptor)
                .Method(1);

            var httpMethod = ParameterProvider.Get(KeepDataInterceptor.Invocation.Method, KeepDataInterceptor.Invocation.Arguments);

            httpMethod.Should().BeEquivalentTo(new Parameter("id", 1, new ToQueryAttribute()));
        }

        public interface IPrimitiveQueryParam { void Method([ToQuery] int id); }

        [Test]
        public void Get_PrimitiveQueryParam_QueryParam()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IPrimitiveQueryParam>(KeepDataInterceptor)
                .Method(1);

            var httpMethod = ParameterProvider.Get(KeepDataInterceptor.Invocation.Method, KeepDataInterceptor.Invocation.Arguments);

            httpMethod.Should().BeEquivalentTo(new Parameter("id", 1, new ToQueryAttribute()));
        }

        public interface ICustomTypeQueryParam { void Method([ToQuery] BasicEntity entity); }

        [Test]
        public void Get_CustomTypeQueryParam_QueryParam()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<ICustomTypeQueryParam>(KeepDataInterceptor)
                .Method(entity);

            var httpMethod = ParameterProvider.Get(KeepDataInterceptor.Invocation.Method, KeepDataInterceptor.Invocation.Arguments);

            httpMethod.Should().BeEquivalentTo(new Parameter("entity", entity, new ToQueryAttribute()));
        }

        public interface ICustomTypeWithoutAttribute { void Method(BasicEntity entity); }

        [Test]
        public void Get_CustomTypeWithoutAttribute_BodyParam()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<ICustomTypeWithoutAttribute>(KeepDataInterceptor)
                .Method(entity);

            var httpMethod = ParameterProvider.Get(KeepDataInterceptor.Invocation.Method, KeepDataInterceptor.Invocation.Arguments);

            httpMethod.Should().BeEquivalentTo(new Parameter("entity", entity, new ToBodyAttribute()));
        }

        public interface ICustomTypeBodyParam { void Method([ToBody] BasicEntity entity); }

        [Test]
        public void Get_CustomTypeBodyParam_BodyParam()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<ICustomTypeBodyParam>(KeepDataInterceptor)
                .Method(entity);

            var httpMethod = ParameterProvider.Get(KeepDataInterceptor.Invocation.Method, KeepDataInterceptor.Invocation.Arguments);

            httpMethod.Should().BeEquivalentTo(new Parameter("entity", entity, new ToBodyAttribute()));
        }

        public interface IPrimitiveBodyParam { void Method([ToBody] int id); }

        [Test]
        public void Get_PrimitiveBodyParam_BodyParam()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IPrimitiveBodyParam>(KeepDataInterceptor)
                .Method(1);

            var httpMethod = ParameterProvider.Get(KeepDataInterceptor.Invocation.Method, KeepDataInterceptor.Invocation.Arguments);

            httpMethod.Should().BeEquivalentTo(new Parameter("id", 1, new ToBodyAttribute()));
        }

        public interface IPrimitiveHeaderParam { void Method([ToHeader] int id); }

        [Test]
        public void Get_PrimitiveHeaderParam_HeaderParam()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IPrimitiveHeaderParam>(KeepDataInterceptor)
                .Method(1);

            var httpMethod = ParameterProvider.Get(KeepDataInterceptor.Invocation.Method, KeepDataInterceptor.Invocation.Arguments);

            httpMethod.Should().BeEquivalentTo(new Parameter("id", 1, new ToHeaderAttribute()));
        }

        public interface ICustomTypeHeaderParam { void Method([ToHeader] BasicEntity entity); }

        [Test]
        public void Get_CustomTypeHeaderParam_HeaderParam()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<ICustomTypeHeaderParam>(KeepDataInterceptor)
                .Method(entity);

            var httpMethod = ParameterProvider.Get(KeepDataInterceptor.Invocation.Method, KeepDataInterceptor.Invocation.Arguments);

            httpMethod.Should().BeEquivalentTo(new Parameter("entity", entity, new ToHeaderAttribute()));
        }

        public interface IMultipleAttributeParam { void Method([ToQuery, ToBody] int id); }

        [Test]
        public void Get_MultipleAttributeParam_ThrowNotSupportedNClientException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IMultipleAttributeParam>(KeepDataInterceptor)
                .Method(1);

            ParameterProvider
                .Invoking(x => x.Get(KeepDataInterceptor.Invocation.Method, KeepDataInterceptor.Invocation.Arguments))
                .Should()
                .Throw<NotSupportedNClientException>();
        }
    }
}
