using FluentAssertions;
using NClient.Packages.Tests.Helpers;
using NUnit.Framework;

namespace NClient.Packages.Tests
{
    [Parallelizable]
    [Category("Packages")]
    public class VersionTest
    {
        [Test]
        public void NClient_Core() =>
            PackagesVersionProvider.GetCurrent("NClient.Core").Should().Be(PackagesVersionProvider.GetNew());

        [Test]
        public void NClient_Providers_Resilience() =>
            PackagesVersionProvider.GetCurrent("NClient.Abstractions").Should().Be(PackagesVersionProvider.GetNew());

        [Test]
        public void NClient_Annotations() =>
            PackagesVersionProvider.GetCurrent("NClient.Annotations").Should().Be(PackagesVersionProvider.GetNew());

        [Test]
        public void NClient() =>
            PackagesVersionProvider.GetCurrent("NClient").Should().Be(PackagesVersionProvider.GetNew());

        [Test]
        public void NClient_Standalone() =>
            PackagesVersionProvider.GetCurrent("NClient.Standalone").Should().Be(PackagesVersionProvider.GetNew());

        [Test]
        public void NClient_AspNetProxy() =>
            PackagesVersionProvider.GetCurrent("NClient.AspNetCore").Should().Be(PackagesVersionProvider.GetNew());
        
        [Test]
        public void NClient_Providers_Api_Rest() =>
            PackagesVersionProvider.GetCurrent("NClient.Providers.Api.Rest").Should().Be(PackagesVersionProvider.GetNew());
        
        [Test]
        public void NClient_Providers_Resilience_Polly() =>
            PackagesVersionProvider.GetCurrent("NClient.Providers.Resilience.Polly").Should().Be(PackagesVersionProvider.GetNew());

        [Test]
        public void NClient_Providers_Results_HttpResponses() =>
            PackagesVersionProvider.GetCurrent("NClient.Providers.Results.HttpResponses").Should().Be(PackagesVersionProvider.GetNew());

        [Test]
        public void NClient_Providers_Results_LanguageExt() =>
            PackagesVersionProvider.GetCurrent("NClient.Providers.Results.LanguageExt").Should().Be(PackagesVersionProvider.GetNew());

        [Test]
        public void NClient_Providers_Serialization_Json_System() =>
            PackagesVersionProvider.GetCurrent("NClient.Providers.Serialization.Json.System").Should().Be(PackagesVersionProvider.GetNew());

        [Test]
        public void NClient_Providers_Serialization_Json_Newtonsoft() =>
            PackagesVersionProvider.GetCurrent("NClient.Providers.Serialization.Json.Newtonsoft").Should().Be(PackagesVersionProvider.GetNew());
        
        [Test]
        public void NClient_Providers_Serialization_Xml_System() =>
            PackagesVersionProvider.GetCurrent("NClient.Providers.Serialization.Xml.System").Should().Be(PackagesVersionProvider.GetNew());

        [Test]
        public void NClient_Providers_Transport_Http_System() =>
            PackagesVersionProvider.GetCurrent("NClient.Providers.Transport.Http.System").Should().Be(PackagesVersionProvider.GetNew());

        [Test]
        public void NClient_Providers_Transport_Http_RestSharp() =>
            PackagesVersionProvider.GetCurrent("NClient.Providers.Transport.Http.RestSharp").Should().Be(PackagesVersionProvider.GetNew());

        [Test]
        public void NClient_Extensions_DependencyInjection() =>
            PackagesVersionProvider.GetCurrent("NClient.Extensions.DependencyInjection").Should().Be(PackagesVersionProvider.GetNew());
    }
}
