using System.Collections;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using NClient.Providers.Api.Rest.Helpers;
using NUnit.Framework;

namespace NClient.Providers.Api.Rest.Tests.Helpers
{
    [Parallelizable]
    public class FormUrlEncoderTest
    {
        public static IEnumerable ValidParameters = new[]
        {
            new TestCaseData(
                    new Dictionary<string, string?> { ["key"] = "value" }, 
                    "key=value")
                .SetName("Single pair"),
            new TestCaseData(
                    new Dictionary<string, string?> { ["key1"] = "value1", ["key2"] = "value2" }, 
                    "key1=value1&key2=value2")
                .SetName("Multiple pairs"),
            new TestCaseData(
                    new Dictionary<string, string?> { ["key"] = null }, 
                    "key=")
                .SetName("Single pair with null value"),
            new TestCaseData(
                    new Dictionary<string, string?> { ["key1"] = null, ["key2"] = null }, 
                    "key1=&key2=")
                .SetName("Multiple pairs with null value"),
            new TestCaseData(
                    new[] { new KeyValuePair<string?, string?>(null, "value") }, 
                    "=value")
                .SetName("Single pair with null key"),
            new TestCaseData(
                    new[] { new KeyValuePair<string?, string?>(null, "value1"), new KeyValuePair<string?, string?>(null, "value2") }, 
                    "=value1&=value2")
                .SetName("Multiple pairs with null key"),
            new TestCaseData(
                    new[] { new KeyValuePair<string?, string?>(null, null) }, 
                    "=")
                .SetName("Single pair with null key and null value"),
            new TestCaseData(
                    new[] { new KeyValuePair<string?, string?>(null, null), new KeyValuePair<string?, string?>(null, null) }, 
                    "=&=")
                .SetName("Multiple pairs with null key and null value"),
            new TestCaseData(
                    new Dictionary<string, string?> { ["key"] = "value&" }, 
                    "key=value%26")
                .SetName("Value with &"),
            new TestCaseData(
                    new Dictionary<string, string?> { ["key"] = "value value" }, 
                    "key=value+value")
                .SetName("Value with space"),
            new TestCaseData(
                    new Dictionary<string, string?> { ["key"] = "value+value" }, 
                    "key=value%2Bvalue")
                .SetName("Value with plus")
        };

        [TestCaseSource(nameof(ValidParameters))]
        public void GetContentByteArray_ValidParameters_ShouldReturnFormUrlEncoderBytes(
            IEnumerable<KeyValuePair<string?, string?>> keyValuePairs, string expectedString)
        {
            var sut = new FormUrlEncoder();

            var actualResult = sut.GetContentByteArray(keyValuePairs);

            Encoding.UTF8.GetString(actualResult).Should().Be(expectedString);
        }
    }
}
