﻿using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using NClient.Annotations;
using NClient.Annotations.Parameters;
using NClient.Core.Exceptions;
using NClient.Core.MethodBuilders.Models;
using NClient.Core.MethodBuilders.Providers;
using NUnit.Framework;

namespace NClient.Standalone.Tests.MethodBuilders.Providers
{
    public class HeaderAttributeProviderTest
    {
        private interface INoHeaders { void Method(); }

        [Header("client-header", "value")]
        private interface IClientHeader { void Method(); }
        [Header("client-header-1", "value"), Header("client-header-2", "value")]
        private interface IClientHeaders { void Method(); }
        [Header("client-header", "value1"), Header("client-header", "value2")]
        private interface IDuplicateClientHeaders { void Method(); }

        private interface IMethodHeader {[Header("method-header", "value")] void Method(); }
        private interface IMethodHeaders {[Header("method-header-1", "value"), Header("method-header-2", "value")] void Method(); }
        private interface IDuplicateMethodHeaders {[Header("method-header", "value1"), Header("method-header", "value2")] void Method(); }

        [Header("client-header", "value")]
        private interface IClientAndMethodHeader {[Header("method-header", "value")] void Method(); }
        [Header("client-header-1", "value"), Header("client-header-2", "value")]
        private interface IClientAndMethodHeaders {[Header("method-header-1", "value"), Header("method-header-2", "value")] void Method(); }

        [OtherAttribute]
        private interface IOther { void Method(); }
        [OtherAttribute, Header("client-header", "value")]
        private interface IOtherAndHeader { void Method(); }
        [Inherited("client-header", "value")]
        private interface IInherited { void Method(); }
        [Inherited("client-header-1", "value"), Header("client-header-2", "value")]
        private interface IInheritedAndHeader { void Method(); }

        public static IEnumerable ValidTestCases = new[]
        {
            new TestCaseData(typeof(INoHeaders), GetMethodInfo<INoHeaders>(), Array.Empty<MethodParam>(),
                    Array.Empty<HeaderAttribute>())
                .SetName("No client header"),

            new TestCaseData(typeof(IClientHeader), GetMethodInfo<IClientHeader>(), Array.Empty<MethodParam>(),
                    new[] { new HeaderAttribute("client-header", "value") })
                .SetName("Client header"),
            new TestCaseData(typeof(IClientHeaders), GetMethodInfo<IClientHeaders>(), Array.Empty<MethodParam>(),
                    new[] { new HeaderAttribute("client-header-1", "value"), new HeaderAttribute("client-header-2", "value") })
                .SetName("Client headers"),
            new TestCaseData(typeof(IDuplicateClientHeaders), GetMethodInfo<IDuplicateClientHeaders>(), Array.Empty<MethodParam>(),
                    new[] { new HeaderAttribute("client-header", "value2") })
                .SetName("Duplicate client headers"),

            new TestCaseData(typeof(IMethodHeader), GetMethodInfo<IMethodHeader>(), Array.Empty<MethodParam>(),
                    new[] { new HeaderAttribute("method-header", "value") })
                .SetName("Method header"),
            new TestCaseData(typeof(IMethodHeaders), GetMethodInfo<IMethodHeaders>(), Array.Empty<MethodParam>(),
                    new[] { new HeaderAttribute("method-header-1", "value"), new HeaderAttribute("method-header-2", "value") })
                .SetName("Method headers"),
            new TestCaseData(typeof(IDuplicateMethodHeaders), GetMethodInfo<IDuplicateMethodHeaders>(), Array.Empty<MethodParam>(),
                    new[] { new HeaderAttribute("method-header", "value2") })
                .SetName("Duplicate method headers"),

            new TestCaseData(typeof(IClientAndMethodHeader), GetMethodInfo<IClientAndMethodHeader>(), Array.Empty<MethodParam>(),
                    new[] { new HeaderAttribute("client-header", "value"), new HeaderAttribute("method-header", "value") })
                .SetName("Client and method header"),
            new TestCaseData(typeof(IClientAndMethodHeaders), GetMethodInfo<IClientAndMethodHeaders>(), Array.Empty<MethodParam>(),
                    new[] { new HeaderAttribute("client-header-1", "value"), new HeaderAttribute("client-header-2", "value"),
                            new HeaderAttribute("method-header-1", "value"), new HeaderAttribute("method-header-2", "value") })
                .SetName("Client and method headers"),

            new TestCaseData(typeof(IOther), GetMethodInfo<IOther>(), Array.Empty<MethodParam>(),
                    Array.Empty<HeaderAttribute>())
                .SetName("Other attribute"),
            new TestCaseData(typeof(IOtherAndHeader), GetMethodInfo<IOtherAndHeader>(), Array.Empty<MethodParam>(),
                    new[] { new HeaderAttribute("client-header", "value") })
                .SetName("Other and header attribute"),
            new TestCaseData(typeof(IInherited), GetMethodInfo<IInherited>(), Array.Empty<MethodParam>(),
                    new[] { new InheritedAttribute("client-header", "value") })
                .SetName("Inherited attribute"),
            new TestCaseData(typeof(IInheritedAndHeader), GetMethodInfo<IInheritedAndHeader>(), Array.Empty<MethodParam>(),
                    new[] { new InheritedAttribute("client-header-1", "value"), new HeaderAttribute("client-header-2", "value") })
                .SetName("Inherited and header attribute"),
        };

        public static IEnumerable InvalidTestCases = new[]
        {
            new TestCaseData(typeof(IClientHeader), GetMethodInfo<IClientHeader>(),
                    new[] { new MethodParam("client-header", typeof(string), new HeaderParamAttribute { Name ="client-header" }) })
                .SetName("Duplicate client and param headers"),
            new TestCaseData(typeof(IMethodHeader), GetMethodInfo<IMethodHeader>(),
                    new[] { new MethodParam("method-header", typeof(string), new HeaderParamAttribute { Name ="method-header" }) })
                .SetName("Duplicate method and param headers")
        };

        [TestCaseSource(nameof(ValidTestCases))]
        public void Get_ValidTestCase_HeaderAttribute(Type clientType, MethodInfo methodInfo, MethodParam[] methodParams, HeaderAttribute[] expectedAttributes)
        {
            var headerAttributeProvider = new HeaderAttributeProvider();

            var actualAttributes = headerAttributeProvider.Get(clientType, methodInfo, methodParams);

            actualAttributes.Should().BeEquivalentTo(expectedAttributes, config => config.WithoutStrictOrdering());
        }

        [TestCaseSource(nameof(InvalidTestCases))]
        public void Get_InvalidTestCase_ThrowNClientException(Type clientType, MethodInfo methodInfo, MethodParam[] methodParams)
        {
            var headerAttributeProvider = new HeaderAttributeProvider();

            headerAttributeProvider
                .Invoking(x => x.Get(clientType, methodInfo, methodParams))
                .Should()
                .Throw<NClientException>();
        }

        private static MethodInfo GetMethodInfo<T>()
        {
            return typeof(T).GetMethods().Single();
        }

        private class OtherAttribute : Attribute { }

        private class InheritedAttribute : HeaderAttribute
        {
            public InheritedAttribute(string name, string value) : base(name, value)
            {
            }
        }
    }
}