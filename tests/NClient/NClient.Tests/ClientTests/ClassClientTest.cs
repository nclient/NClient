using FluentAssertions;
using NClient.Exceptions;
using NClient.Standalone.Exceptions.Factories;
using NClient.Testing.Common.Clients;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    public class ClassClientTest
    {
        [Test, Order(0)]
        public void ClassClient_Build_ThrowClientValidationException()
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
