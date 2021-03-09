using FluentAssertions;
using NClient.InterfaceProxy.Attributes;
using NClient.InterfaceProxy.Attributes.Methods;
using NUnit.Framework;

namespace NClient.InterfaceProxy.Standalone.Tests.RouteBuilderTests
{
    [Parallelizable]
    public class RouteBuilderTest : RouteBuilderTestBase
    {
        [Api("api/[controller]")] public interface IAllTypesOfTokensWithStaticPart { [AsHttpGet("[action]/{id}")] int Method(int id); }

        [Test]
        public void Build_AllTypesOfTokensWithStaticPart_StaticPartWithReplacedTokens()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IAllTypesOfTokensWithStaticPart>(KeepDataInterceptor)
                .Method(1);

            var route = BuildRoute(KeepDataInterceptor);

            route.Should().Be("api/AllTypesOfTokensWithStaticPart/Method/1");
        }
    }
}
