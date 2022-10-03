﻿using NClient.Annotations.Http;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using System.Threading.Tasks;
using NClient.Testing.Common.Clients;
using Microsoft.Extensions.DependencyModel;
using System.Linq;

namespace NClient.Standalone.Tests.Validation
{
    [Parallelizable]
    [SuppressMessage("ReSharper", "BadDeclarationBracesLineBreaks")]
    public class ClientValidatorTest
    {
        Uri _uri = new Uri("http://localhost:5000");

        public interface IHiddenMyClient : IMyController
        {
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
            var optsBuilder = NClientGallery.Clients.GetRest().For<IMyClientNoParentType>(host: _uri);
            Func<IMyClientNoParentType> buildFunc = () => optsBuilder.Build();
            buildFunc.Should().Throw<Exception>();
        }

        [Test]
        public void ClientValidator_WhenExecutingHiddenMethodOnTheChildType_InvalidParentNotCheckedNoThrow()
        {
            var optsBuilder = NClientGallery.Clients.GetRest().For<IReturnClientWithMetadata>(host: _uri);
            Func<IReturnClientWithMetadata> buildFunc = () => optsBuilder.Build();
            buildFunc.Should().NotThrow<Exception>();
        }

        [Test]
        public void ClientValidator_WhenExecutingHiddenMethodOnTheChildType_OnlyChildMethodIsValidatedNoThrow()
        {
            var optsBuilder = NClientGallery.Clients.GetRest().For<IHiddenMyClient>(host: _uri);
            Func<IHiddenMyClient> buildFunc = () => optsBuilder.Build();
            buildFunc.Should().NotThrow<Exception>();
        }

        [Test]
        public void ClientValidator_WhenTypeHasHiddenAndUnhiddenMethods_ReturnUnhiddenMethodsOnly()
        {
            var methods = NClient.Core.Helpers.TypeExtensions.GetUnhiddenInterfaceMethods(typeof(IHiddenMyClient), true);
            Assert.IsTrue(methods.Count() == 1);
            Assert.IsTrue(methods[0].Name == "DeleteAsync");
        }

        [Test]
        public void ClientValidator_WhenTypeHasOnlyUnhiddenMethods_ReturnAllUnhiddenMethods()
        {
            var methods = NClient.Core.Helpers.TypeExtensions.GetUnhiddenInterfaceMethods(typeof(IMyClientNoParentType), true);
            Assert.IsTrue(methods.Count() == 3);
            Assert.IsTrue(methods[0].Name == "Get");
            Assert.IsTrue(methods[1].Name == "Put");
            Assert.IsTrue(methods[2].Name == "Post");
        }
    }
}