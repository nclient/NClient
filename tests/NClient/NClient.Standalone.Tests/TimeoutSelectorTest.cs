using System;
using System.Collections;
using System.Threading;
using FluentAssertions;
using FluentAssertions.Extensions;
using NClient.Exceptions;
using NClient.Standalone.ClientProxy.Generation.Helpers;
using NClient.Standalone.Exceptions.Factories;
using NUnit.Framework;

namespace NClient.Standalone.Tests
{
    [Parallelizable]
    public class TimeoutSelectorTest
    {
        private IClientValidationExceptionFactory _clientValidationExceptionFactory = null!;
        private TimeoutSelector _timeoutSelector = null!;
        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _clientValidationExceptionFactory = new ClientValidationExceptionFactory();
            _timeoutSelector = new TimeoutSelector(_clientValidationExceptionFactory);
        }
        
        public static IEnumerable ValidTestCases = new[]
        {
            new TestCaseData(Timeout.InfiniteTimeSpan, null, null, 100.Seconds()),
            new TestCaseData(Timeout.InfiniteTimeSpan, 1.Seconds(), null, 1.Seconds()),
            new TestCaseData(Timeout.InfiniteTimeSpan, null, 1.Seconds(), 1.Seconds()),
            new TestCaseData(Timeout.InfiniteTimeSpan, 2.Seconds(), 1.Seconds(), 2.Seconds()),
            new TestCaseData(Timeout.InfiniteTimeSpan, 1.Seconds(), 2.Seconds(), 1.Seconds())
        };

        [TestCaseSource(nameof(ValidTestCases))]
        public void Get_ValidTestCases_ReturnTimeout(
            TimeSpan transportTimeout, TimeSpan? clientTimeout, TimeSpan? staticTimeout, TimeSpan expectedTimeout)
        {
            var actualResult = _timeoutSelector.Get(transportTimeout, clientTimeout, staticTimeout);

            actualResult.Should().Be(expectedTimeout);
        }
        
        public static IEnumerable InvalidTestCases = new[]
        {
            new TestCaseData(1.Seconds(), null, null).SetCategory("The transport timeout is greater"),
            new TestCaseData(2.Seconds(), 1.Seconds(), null).SetCategory("The transport timeout is greater"),
            new TestCaseData(2.Seconds(), null, 1.Seconds()).SetCategory("The transport timeout is greater"),
            new TestCaseData(3.Seconds(), 2.Seconds(), 1.Seconds()).SetCategory("The transport timeout is greater"),
            new TestCaseData(3.Seconds(), 1.Seconds(), 2.Seconds()).SetCategory("The transport timeout is greater"),
            
            new TestCaseData(1.Seconds(), 1.Seconds(), null).SetCategory("The transport timeout is less than or equal"),
            new TestCaseData(1.Seconds(), 2.Seconds(), null).SetCategory("The transport timeout is less than or equal"),
            new TestCaseData(1.Seconds(), null, 1.Seconds()).SetCategory("The transport timeout is less than or equal"),
            new TestCaseData(1.Seconds(), null, 2.Seconds()).SetCategory("The transport timeout is less than or equal"),
            new TestCaseData(1.Seconds(), 2.Seconds(), 3.Seconds()).SetCategory("The transport timeout is less than or equal"),
            new TestCaseData(1.Seconds(), 3.Seconds(), 2.Seconds()).SetCategory("The transport timeout is less than or equal")
        };

        [TestCaseSource(nameof(InvalidTestCases))]
        public void Get_InvalidTestCases_Throw(
            TimeSpan transportTimeout, TimeSpan? clientTimeout, TimeSpan? staticTimeout)
        {
            _timeoutSelector
                .Invoking(x => x.Get(transportTimeout, clientTimeout, staticTimeout))
                .Should()
                .ThrowExactly<ClientValidationException>()
                .WithMessage(_clientValidationExceptionFactory.TransportTimeoutShouldBeInfinite(transportTimeout).Message);
        }
    }
}
