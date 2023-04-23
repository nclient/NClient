using NClient.Annotations.Http;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NClient.Testing.Common.Clients;
using System.Linq;
using NClient.Exceptions;
using NClient.Standalone.ClientProxy.Validation;

namespace NClient.Standalone.Tests.Validation
{
    [Parallelizable]
    [SuppressMessage("ReSharper", "BadDeclarationBracesLineBreaks")]
    public class ClientValidatorTest
    {
        private readonly ClientValidator _validator = new();
        
        public interface IHiddenMyClient : IMyController
        {
            [GetMethod()]
            int[] Get();

            [PutMethod()]
            int[] Put();

            [PostMethod()]
            int[] Post();

            [DeleteMethod()]
            new void DeleteAsync();
        }

        public interface IMyController
        {
            void DeleteAsync();
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
        public void ClientValidator_WhenExecutingAnyInvalidMethodOnTheType_BuildThrows()
        {
            Action ax = () => _validator.Ensure<IMyClientNoParentType>();
            ax.Should().Throw<ClientValidationException>();
        }

        [Test]
        public void ClientValidator_WhenExecutingHiddenMethodOnTheChildType_InvalidParentNotCheckedNoThrow()
        {
            Action ax = () => _validator.Ensure<IReturnClientWithMetadata>();
            ax.Should().NotThrow<Exception>();
        }

        [Test]
        public void ClientValidator_WhenExecutingHiddenMethodOnTheChildType_OnlyChildMethodIsValidatedNoThrow()
        {
            Action ax = () => _validator.Ensure<IHiddenMyClient>();
            ax.Should().NotThrow<Exception>();
        }

        [Test]
        public void ClientValidator_WhenTypeHasHiddenAndUnhiddenMethods_ReturnUnhiddenMethodsOnly()
        {
            var methods = Core.Helpers.TypeExtensions.GetUnhiddenInterfaceMethods(typeof(IHiddenMyClient), true);
            Assert.IsTrue(methods.Count() == 4);
            Assert.IsTrue(methods[0].Name == "Get");
            Assert.IsTrue(methods[1].Name == "Put");
            Assert.IsTrue(methods[2].Name == "Post");
            Assert.IsTrue(methods[3].Name == "DeleteAsync");
        }

        [Test]
        public void ClientValidator_WhenTypeHasOnlyUnhiddenMethods_ReturnAllUnhiddenMethods()
        {
            var methods = Core.Helpers.TypeExtensions.GetUnhiddenInterfaceMethods(typeof(IMyClientNoParentType), true);
            Assert.IsTrue(methods.Count() == 3);
            Assert.IsTrue(methods[0].Name == "Get");
            Assert.IsTrue(methods[1].Name == "Put");
            Assert.IsTrue(methods[2].Name == "Post");
        }
    }
}
