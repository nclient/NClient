using System;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;

namespace NClient.Tests.ClientTests.Helpers
{
    public abstract class ClientTestBase<TClient> where TClient : class
    {
        protected readonly Fixture Fixture = new Lazy<Fixture>(() =>
        {
            var fixture = new Fixture();
            fixture.Inject(new UriScheme("http"));
            return fixture;
        }).Value;
        
        [Test, Order(-1)]
        public virtual void Build_CustomClientType_Validate()
        {
            var uri = Fixture.Create<Uri>();
            
            NClientGallery.Clients.GetRest().For<TClient>(uri)
                .Invoking(builder => builder.Build())
                .Should()
                .NotThrow();
        }
    }
}
