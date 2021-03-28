using System.Net.Http;
using Castle.DynamicProxy;
using FluentAssertions;
using NClient.Annotations.Methods;
using NClient.Core.Exceptions;
using NClient.Core.Interceptors;
using NClient.Core.Mappers;
using NClient.Core.RequestBuilders;
using NUnit.Framework;

namespace NClient.Standalone.Tests.HttpMethodProviderTests
{
    [Parallelizable]
    public class HttpMethodProviderTest
    {
        internal HttpMethodProvider HttpMethodProvider = null!;
        internal KeepDataInterceptor KeepDataInterceptor = null!;
        protected ProxyGenerator ProxyGenerator = null!;

        [SetUp]
        public void SetUp()
        {
            var attributeMapper = new AttributeMapper();
            HttpMethodProvider = new HttpMethodProvider(attributeMapper);

            ProxyGenerator = new ProxyGenerator();
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            KeepDataInterceptor = new KeepDataInterceptor();
        }

        public interface IGetMethod { [GetMethod] int Method(); }

        [Test]
        public void Build_MethodWithGetAttribute_GetHttpMethodType()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IGetMethod>(KeepDataInterceptor)
                .Method();

            var httpMethod = HttpMethodProvider.Get(KeepDataInterceptor.Invocation!.Method);

            httpMethod.Should().Be(HttpMethod.Get);
        }

        public interface IPostMethod { [PostMethod] int Method(); }

        [Test]
        public void Build_MethodWithPostAttribute_PostHttpMethodType()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IPostMethod>(KeepDataInterceptor)
                .Method();

            var httpMethod = HttpMethodProvider.Get(KeepDataInterceptor.Invocation!.Method);

            httpMethod.Should().Be(HttpMethod.Post);
        }

        public interface IPutMethod { [PutMethod] int Method(); }

        [Test]
        public void Build_MethodWithPutAttribute_PutHttpMethodType()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IPutMethod>(KeepDataInterceptor)
                .Method();

            var httpMethod = HttpMethodProvider.Get(KeepDataInterceptor.Invocation!.Method);

            httpMethod.Should().Be(HttpMethod.Put);
        }

        public interface IDeleteMethod { [DeleteMethod] int Method(); }

        [Test]
        public void Build_MethodWithDeleteAttribute_DeleteHttpMethodType()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IDeleteMethod>(KeepDataInterceptor)
                .Method();

            var httpMethod = HttpMethodProvider.Get(KeepDataInterceptor.Invocation!.Method);

            httpMethod.Should().Be(HttpMethod.Delete);
        }

        public interface IMultipleAttributeMethod {[DeleteMethod, GetMethod] int Method(); }

        [Test]
        public void Build_MultipleAttributeMethod_ThrowNotSupportedNClientException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IMultipleAttributeMethod>(KeepDataInterceptor)
                .Method();

            HttpMethodProvider
                .Invoking(x => x.Get(KeepDataInterceptor.Invocation!.Method))
                .Should()
                .Throw<NotSupportedNClientException>();
        }

        public interface IWithoutMethodAttribute { int Method(); }

        [Test]
        public void Build_MethodWithoutAttribute_ThrowException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IWithoutMethodAttribute>(KeepDataInterceptor)
                .Method();

            HttpMethodProvider
                .Invoking(x => x.Get(KeepDataInterceptor.Invocation!.Method))
                .Should()
                .Throw<AttributeNotFoundNClientException>();
        }

        public interface IWithNotSupportedMethodAttribute { [NotSupported] int Method(); }

        [Test]
        public void Build_MethodWithNotSupportedAttribute_ThrowException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IWithNotSupportedMethodAttribute>(KeepDataInterceptor)
                .Method();

            HttpMethodProvider
                .Invoking(x => x.Get(KeepDataInterceptor.Invocation!.Method))
                .Should()
                .Throw<NotSupportedNClientException>();
        }

        public class NotSupportedAttribute : MethodAttribute
        {
            public NotSupportedAttribute(string? template = null) : base(template)
            {
            }
        }
    }
}
