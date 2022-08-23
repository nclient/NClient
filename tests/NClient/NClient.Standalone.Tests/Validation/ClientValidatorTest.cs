using NClient.Annotations.Http;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using NClient.Standalone.ClientProxy.Validation;
using FluentAssertions;

namespace NClient.Standalone.Tests.Validation
{
    [Parallelizable]
    [SuppressMessage("ReSharper", "BadDeclarationBracesLineBreaks")]
    public class ClientValidatorTest
    {
        Uri _uri = new Uri("http://localhost:5000");
        public interface IMyClient : IMyController
        {
        }

        public interface IMyController
        {
            [GetMethod("{missing_get_parameter}")]
            int[] Get();

            [PutMethod("{missing_put_parameter}")]
            int[] Put();

            [PostMethod("{missing_post_parameter}")]
            int[] Post();

        }

        [Test]
        public void ClientValidator_WhenExecutingAnyInvalidMethod_BuildThrowsRelevantException()
        {
            var optsBuilder = NClientGallery.Clients.GetRest().For<IMyClient>(host: _uri);
            Func<IMyClient> buildFunc = () => optsBuilder.Build();
            buildFunc.Should().Throw<Exception>();
        }

        [Test]
        public void ClientValidator_WhenExecutingAnInvalidGetMethod_DoesntThrowAfterBuild()
        {
            var optsBuilder = NClientGallery.Clients.GetRest().For<IMyClient>(host: _uri);
            Func<int[]> getFunc = () => optsBuilder.Build().Get();
            getFunc.Should().NotThrow<Exception>();
        }

        [Test]
        public void ClientValidator_WhenExecutingAnInvalidPostMethod_DoesntThrowAfterBuild()
        {
            var optsBuilder = NClientGallery.Clients.GetRest().For<IMyClient>(host: _uri);
            Func<int[]> getFunc = () => optsBuilder.Build().Post();
            getFunc.Should().NotThrow<Exception>();
        }

        [Test]
        public void ClientValidator_WhenExecutingAnInvalidPutMethod_DoesntThrowAfterBuild()
        {
            var optsBuilder = NClientGallery.Clients.GetRest().For<IMyClient>(host: _uri);
            Func<int[]> getFunc = () => optsBuilder.Build().Put();
            getFunc.Should().NotThrow<Exception>();
        }
    }
}
