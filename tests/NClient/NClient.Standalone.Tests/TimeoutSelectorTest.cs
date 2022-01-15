using System;
using System.Collections;
using System.Threading;
using FluentAssertions;
using FluentAssertions.Extensions;
using NClient.Standalone.ClientProxy.Generation.Helpers;
using NUnit.Framework;

namespace NClient.Standalone.Tests
{
    [Parallelizable]
    public class TimeoutSelectorTest
    {
        private TimeoutSelector _timeoutSelector = null!;
        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _timeoutSelector = new TimeoutSelector();
        }
        
        public static IEnumerable ValidTestCases = new[]
        {
            new TestCaseData(1.Seconds(), null, null, 1.Seconds()),
            new TestCaseData(2.Seconds(), 1.Seconds(), null, 1.Seconds()),
            new TestCaseData(2.Seconds(), null, 1.Seconds(), 1.Seconds()),
            new TestCaseData(3.Seconds(), 2.Seconds(), 1.Seconds(), 2.Seconds()),
            new TestCaseData(3.Seconds(), 1.Seconds(), 2.Seconds(), 1.Seconds()),
            
            new TestCaseData(Timeout.InfiniteTimeSpan, null, null, Timeout.InfiniteTimeSpan),
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
            new TestCaseData(1.Seconds(), 1.Seconds(), null),
            new TestCaseData(1.Seconds(), 2.Seconds(), null),
            new TestCaseData(1.Seconds(), null, 1.Seconds()),
            new TestCaseData(1.Seconds(), null, 2.Seconds()),
            new TestCaseData(1.Seconds(), 2.Seconds(), 3.Seconds()),
            new TestCaseData(1.Seconds(), 3.Seconds(), 2.Seconds())
        };

        [TestCaseSource(nameof(InvalidTestCases))]
        public void Get_InvalidTestCases_Throw(
            TimeSpan transportTimeout, TimeSpan? clientTimeout, TimeSpan? staticTimeout)
        {
            _timeoutSelector
                .Invoking(x => x.Get(transportTimeout, clientTimeout, staticTimeout))
                .Should()
                .ThrowExactly<InvalidOperationException>();
        }
    }
}
