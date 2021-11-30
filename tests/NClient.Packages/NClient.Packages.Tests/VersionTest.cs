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
        public void NClient_Providers_Mapping_HttpResponses() =>
            PackagesVersionProvider.GetCurrent("NClient.Providers.Mapping.HttpResponses").Should().Be(PackagesVersionProvider.GetNew());

        [Test]
        public void NClient_Providers_Mapping_LanguageExt() =>
            PackagesVersionProvider.GetCurrent("NClient.Providers.Mapping.LanguageExt").Should().Be(PackagesVersionProvider.GetNew());

        [Test]
        public void NClient_Providers_Serialization_SystemTextJson() =>
            PackagesVersionProvider.GetCurrent("NClient.Providers.Serialization.SystemTextJson").Should().Be(PackagesVersionProvider.GetNew());

        [Test]
        public void NClient_Providers_Serialization_NewtonsoftJson() =>
            PackagesVersionProvider.GetCurrent("NClient.Providers.Serialization.NewtonsoftJson").Should().Be(PackagesVersionProvider.GetNew());
        
        [Test]
        public void NClient_Providers_Serialization_MessagePack() =>
            PackagesVersionProvider.GetCurrent("NClient.Providers.Serialization.MessagePack").Should().Be(PackagesVersionProvider.GetNew());
        
        [Test]
        public void NClient_Providers_Serialization_ProtobufNet() =>
            PackagesVersionProvider.GetCurrent("NClient.Providers.Serialization.ProtobufNet").Should().Be(PackagesVersionProvider.GetNew());
        
        [Test]
        public void NClient_Providers_Serialization_SystemXml() =>
            PackagesVersionProvider.GetCurrent("NClient.Providers.Serialization.SystemXml").Should().Be(PackagesVersionProvider.GetNew());

        [Test]
        public void NClient_Providers_Transport_SystemNetHttp() =>
            PackagesVersionProvider.GetCurrent("NClient.Providers.Transport.SystemNetHttp").Should().Be(PackagesVersionProvider.GetNew());

        [Test]
        public void NClient_Providers_Transport_RestSharp() =>
            PackagesVersionProvider.GetCurrent("NClient.Providers.Transport.RestSharp").Should().Be(PackagesVersionProvider.GetNew());

        [Test]
        public void NClient_Extensions_DependencyInjection() =>
            PackagesVersionProvider.GetCurrent("NClient.Extensions.DependencyInjection").Should().Be(PackagesVersionProvider.GetNew());
    }
}
