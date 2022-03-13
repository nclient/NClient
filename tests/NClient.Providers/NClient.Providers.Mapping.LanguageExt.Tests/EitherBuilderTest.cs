using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LanguageExt;
using LanguageExt.DataTypes.Serialisation;
using Moq;
using NClient.Providers.Serialization;
using NClient.Providers.Transport;
using NClient.Testing.Common.Entities;
using NClient.Testing.Common.Helpers;
using NUnit.Framework;

namespace NClient.Providers.Mapping.LanguageExt.Tests
{
    [Parallelizable]
    public class EitherBuilderTest
    {
        [Test]
        public async Task Build_SuccessHttpResponse_EitherWithRight()
        {
            var expectedValue = new BasicEntity { Id = 1, Value = 2 };
            var eitherBuilder = new ResponseToEitherBuilder();
            var serializerMock = new Mock<ISerializer>();
            serializerMock
                .Setup(x => x.Deserialize(It.IsAny<string>(), It.IsAny<Type>()))
                .Returns(expectedValue);
            var request = new Request(Guid.Empty, resource: "http://localhost:5000".ToUri(), RequestType.Custom);
            var response = new Response(new Request(Guid.Empty, resource: "http://localhost:5000".ToUri(), RequestType.Read))
            {
                StatusCode = (int) HttpStatusCode.OK,
                IsSuccessful = true
            };
            var responseContext = new ResponseContext<IRequest, IResponse>(request, response);

            var actualResult = await eitherBuilder.MapAsync(
                typeof(Either<string, BasicEntity>), responseContext, serializerMock.Object, CancellationToken.None);
            
            actualResult.Should().BeEquivalentTo(new Either<string, BasicEntity>(new[] { EitherData.Right<string, BasicEntity>(expectedValue) }));
        }
        
        [Test]
        public async Task Build_FailureHttpResponse_EitherWithLeft()
        {
            const string expectedError = "Error message.";
            var eitherBuilder = new ResponseToEitherBuilder();
            var serializerMock = new Mock<ISerializer>();
            serializerMock
                .Setup(x => x.Deserialize(It.IsAny<string>(), It.IsAny<Type>()))
                .Returns(expectedError);
            var request = new Request(Guid.Empty, resource: "http://localhost:5000".ToUri(), RequestType.Custom);   
            var response = new Response(new Request(Guid.Empty, resource: "http://localhost".ToUri(), RequestType.Read))
            {
                StatusCode = (int) HttpStatusCode.NotFound,
                IsSuccessful = false
            };
            var responseContext = new ResponseContext<IRequest, IResponse>(request, response);

            var actualResult = await eitherBuilder.MapAsync(
                typeof(Either<string, BasicEntity>), responseContext, serializerMock.Object, CancellationToken.None);
            
            actualResult.Should().BeEquivalentTo(new Either<string, BasicEntity>(new[] { EitherData.Left<string, BasicEntity>(expectedError) }));
        }
    }
}
