using FluentAssertions;
using NClient.Exceptions;
using NClient.Standalone.Exceptions.Factories;
using NClient.Testing.Common.Clients;
using NClient.Tests.ClientTests.Helpers;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    public class ClassClientTest : ClientTestBase<ClassClientWithMetadata>
    {
        [Test, Order(0)]
        public override void Build_CustomClientType_Validate()
        {
            const string anyHost = "http://localhost:5000";
            
            NClientGallery.Clients.GetRest().For<ClassClientWithMetadata>(anyHost)
                .Invoking(builder => builder.Build())
                .Should()
                .ThrowExactly<ClientValidationException>()
                .WithMessage(new ClientValidationExceptionFactory().ClientTypeIsNotInterface(typeof(ClassClientWithMetadata)).Message);
        }
    }
}
