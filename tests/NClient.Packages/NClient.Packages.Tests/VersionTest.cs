using FluentAssertions;
using NClient.Packages.Tests.Helpers;
using NUnit.Framework;

namespace NClient.Packages.Tests
{
    [Parallelizable]
    [Category("Packages")]
    [Explicit("There must be env variables")]
    public class VersionTest
    {
        [Test]
        public void NClient() => 
            PackagesVersionProvider.GetCurrent("NClient").Should().Be(PackagesVersionProvider.GetNew());

        [Test]
        public void NClient_AspNetProxy() => 
            PackagesVersionProvider.GetCurrent("NClient.AspNetProxy").Should().Be(PackagesVersionProvider.GetNew());

        [Test]
        public void NClient_AspNetProxy_Standalone() => 
            PackagesVersionProvider.GetCurrent("NClient.AspNetProxy.Standalone").Should().Be(PackagesVersionProvider.GetNew());

        [Test]
        public void NClient_Core() => 
            PackagesVersionProvider.GetCurrent("NClient.Core").Should().Be(PackagesVersionProvider.GetNew());

        [Test]
        public void NClient_InterfaceProxy() => 
            PackagesVersionProvider.GetCurrent("NClient.InterfaceProxy").Should().Be(PackagesVersionProvider.GetNew());

        [Test]
        public void NClient_InterfaceProxy_Standalone() => 
            PackagesVersionProvider.GetCurrent("NClient.InterfaceProxy.Standalone").Should().Be(PackagesVersionProvider.GetNew());

        [Test]
        public void NClient_Standalone() => 
            PackagesVersionProvider.GetCurrent("NClient.Standalone").Should().Be(PackagesVersionProvider.GetNew());

        [Test]
        public void NClient_Providers_HttpClient() => 
            PackagesVersionProvider.GetCurrent("NClient.Providers.HttpClient").Should().Be(PackagesVersionProvider.GetNew());

        [Test]
        public void NClient_Providers_HttpClient_RestSharp() => 
            PackagesVersionProvider.GetCurrent("NClient.Providers.HttpClient.RestSharp").Should().Be(PackagesVersionProvider.GetNew());

        [Test]
        public void NClient_Providers_Resilience() => 
            PackagesVersionProvider.GetCurrent("NClient.Providers.Resilience").Should().Be(PackagesVersionProvider.GetNew());

        [Test]
        public void NClient_Providers_Resilience_Polly() => 
            PackagesVersionProvider.GetCurrent("NClient.Providers.Resilience.Polly").Should().Be(PackagesVersionProvider.GetNew());

        [Test]
        public void NClient_Extensions_DependencyInjection() => 
            PackagesVersionProvider.GetCurrent("NClient.Extensions.DependencyInjection").Should().Be(PackagesVersionProvider.GetNew());
    }
}
