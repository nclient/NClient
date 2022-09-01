using NClient.Annotations.Http;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using System.Threading.Tasks;
using NClient.Testing.Common.Clients;

namespace NClient.Standalone.Tests.Validation
{
    [Parallelizable]
    [SuppressMessage("ReSharper", "BadDeclarationBracesLineBreaks")]
    public class ClientValidatorTest
    {
        Uri _uri = new Uri("http://localhost:5000");

        public interface IMyClient: IMyController
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

        public interface IControllerWithParams: IBasicClient
        {
            [GetMethod]
            new Task<int> GetAsync(int id);
        }

        public interface IMyClientNoParentType
        {

            [GetMethod("{missing_get_parameter}")]
            int[] Get();

            [PutMethod("{missing_put_parameter}")]
            int[] Put();

            [PostMethod("{missing_post_parameter}")]
            int[] Post();
        }

        //UnitOfWork_StateUnderTest_ExpectedBehavior 
        [Test]
        public void ClientValidator_WhenExecutingAnyInvalidMethodOnTheType_BuildThrowsTheException()
        {
            var optsBuilder = NClientGallery.Clients.GetRest().For<IMyClientNoParentType>(host: _uri);
            Func<IMyClientNoParentType> buildFunc = () => optsBuilder.Build();
            buildFunc.Should().Throw<Exception>();
        }

        [Test]
        public void ClientValidator_WhenExecutingAnyInvalidMethodOnParentType_BuildThrowsTheException()
        {
            var optsBuilder = NClientGallery.Clients.GetRest().For<IMyClient>(host: _uri);
            Func<IMyClient> buildFunc = () => optsBuilder.Build();
            buildFunc.Should().Throw<Exception>();
        }
    }
}
