﻿using System;
using System.Net;
using System.Net.Http;
using FluentAssertions;
using LanguageExt;
using LanguageExt.DataTypes.Serialisation;
using Moq;
using NClient.Abstractions.HttpClients;
using NClient.Abstractions.Serialization;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.Providers.Results.LanguageExt.Tests
{
    [Parallelizable]
    public class EitherBuilderTest
    {
        [Test]
        public void Build_SuccessHttpResponse_EitherWithRight()
        {
            var expectedValue = new BasicEntity { Id = 1, Value = 2 };
            var eitherBuilder = new EitherBuilder();
            var serializerMock = new Mock<ISerializer>();
            serializerMock
                .Setup(x => x.Deserialize(It.IsAny<string>(), It.IsAny<Type>()))
                .Returns(expectedValue);        
            var httpResponse = new HttpResponse(new HttpRequest(Guid.Empty, new Uri("http://localhost"), HttpMethod.Get))
            {
                StatusCode = HttpStatusCode.OK
            };

            var actualResult = eitherBuilder.Build(typeof(Either<string, BasicEntity>), httpResponse, serializerMock.Object);
            
            actualResult.Should().BeEquivalentTo(new Either<string, BasicEntity>(new[] { EitherData.Right<string, BasicEntity>(expectedValue) }));
        }
        
        [Test]
        public void Build_FailureHttpResponse_EitherWithLeft()
        {
            const string expectedError = "Error message.";
            var eitherBuilder = new EitherBuilder();
            var serializerMock = new Mock<ISerializer>();
            serializerMock
                .Setup(x => x.Deserialize(It.IsAny<string>(), It.IsAny<Type>()))
                .Returns(expectedError);        
            var httpResponse = new HttpResponse(new HttpRequest(Guid.Empty, new Uri("http://localhost"), HttpMethod.Get))
            {
                StatusCode = HttpStatusCode.NotFound
            };

            var actualResult = eitherBuilder.Build(typeof(Either<string, BasicEntity>), httpResponse, serializerMock.Object);
            
            actualResult.Should().BeEquivalentTo(new Either<string, BasicEntity>(new[] { EitherData.Left<string, BasicEntity>(expectedError) }));
        }
    }
}
