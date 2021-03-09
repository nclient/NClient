using Castle.DynamicProxy;
using NClient.Core.Interceptors;
using NClient.Core.RequestBuilders;
using NClient.InterfaceProxy.Attributes;
using NUnit.Framework;

namespace NClient.InterfaceProxy.Standalone.Tests.RouteBuilderTests
{
    [Parallelizable]
    public abstract class RouteBuilderTestBase
    {
        internal RouteBuilder RouteBuilder = null!;
        internal KeepDataInterceptor KeepDataInterceptor = null!;
        protected ProxyGenerator ProxyGenerator = null!;

        [SetUp]
        public void SetUp()
        {
            var attributeHelper = new AttributeHelper();
            RouteBuilder = new RouteBuilder(attributeHelper);

            ProxyGenerator = new ProxyGenerator();
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            KeepDataInterceptor = new KeepDataInterceptor();
        }

        internal string BuildRoute(KeepDataInterceptor interceptor)
        {
            return RouteBuilder.Build(
                interceptor.Invocation.Method.DeclaringType,
                interceptor.Invocation.Method,
                interceptor.Invocation.Arguments);
        }
    }
}