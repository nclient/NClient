using System;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;

namespace NClient.Tests.ClientFactoryTests.Helpers
{
    public abstract class ClientFactoryTestBase
    {
        protected readonly Fixture Fixture = new Lazy<Fixture>(() =>
        {
            var fixture = new Fixture();
            fixture.Inject(new UriScheme("http"));
            return fixture;
        }).Value;
        
        [Test, Order(-1)]
        public void Build_CustomFactory_Validate()
        {
            var factoryName = Fixture.Create<string>();
            
            NClientGallery.ClientFactories.GetRest().For(factoryName)
                .Invoking(builder => builder.Build())
                .Should()
                .NotThrow();
        }
    }
}
