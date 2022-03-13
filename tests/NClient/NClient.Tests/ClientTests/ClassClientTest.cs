using System.Linq;
using FluentAssertions;
using NClient.Exceptions;
using NClient.Standalone.Exceptions.Factories;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Clients;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    public class ClassClientTest
    {
        [Test]
        public void ClassClient_Build_ThrowClientValidationException()
        {
            const int id = 1;
            using var api = ClassApiMockFactory.MockGetMethod(id);

            NClientGallery.Clients.GetRest().For<ClassClientWithMetadata>(host: api.Urls.First())
                .Invoking(builder => builder.Build())
                .Should()
                .ThrowExactly<ClientValidationException>()
                .WithMessage(new ClientValidationExceptionFactory().ClientTypeIsNotInterface(typeof(ClassClientWithMetadata)).Message);
        }
    }
}
