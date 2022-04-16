using System;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using NClient.Providers.Mapping.Results;
using NUnit.Framework;

namespace NClient.Standalone.Tests
{
    [Parallelizable]
    public class ResultDeconstructTest
    {
        // ReSharper disable once InconsistentNaming
        private static readonly Fixture _fixture = new();

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _fixture.Customize(new AutoMoqCustomization());
        }

        [Test]
        public void IResponseWithDataOrError_Deconstruct()
        {
            var resultData = _fixture.Create<Int32>();
            var error = _fixture.Create<string>();
            var response = new Result<Int32, string>(resultData, error);

            var (data, err) = response;
            data.Should().Be(resultData);
            err.Should().Be(error);
        }
    }
}
