using System;
using System.Collections.Generic;
using System.Net.Http;
using FluentAssertions;
using NClient.Abstractions.HttpClients;
using NClient.Annotations.Methods;
using NClient.Annotations.Parameters;
using NClient.Core.Exceptions;
using NClient.Exceptions;
using NClient.Testing.Common;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.Standalone.Tests.RequestBuilderTests
{
    [Parallelizable]
    public class RequestBuilderQueryTest : RequestBuilderTestBase
    {
        private interface IPrimitiveParameter {[GetMethod] int Get([QueryParam] int id); }

        [Test]
        public void Build_PrimitiveParameter_PrimitiveParameterInQuery()
        {
            var httpRequest = BuildRequest(BuildMethod<IPrimitiveParameter>(), 1);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get,
                parameters: new[] { new HttpParameter("id", 1) });
        }

        private interface IMultiplyPrimitiveParameters {[GetMethod] int Get([QueryParam] int id, [QueryParam] string value); }

        [Test]
        public void Build_MultiplyPrimitiveParameters_PrimitiveParametersInQuery()
        {
            var httpRequest = BuildRequest(BuildMethod<IMultiplyPrimitiveParameters>(), 1, "val");

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get,
                parameters: new[] { new HttpParameter("id", 1), new HttpParameter("value", "val") });
        }


        private interface IPrimitiveParameterWithoutAttribute {[GetMethod] int Get(int id); }

        [Test]
        public void Build_PrimitiveParameterWithoutAttribute_PrimitiveParameterInQuery()
        {
            var httpRequest = BuildRequest(BuildMethod<IPrimitiveParameterWithoutAttribute>(), 1);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get,
                parameters: new[] { new HttpParameter("id", 1) });
        }

        private interface IMultiplyPrimitiveParametersWithoutAttribute {[GetMethod] int Get(int id, string value); }

        [Test]
        public void Build_MultiplyPrimitiveParametersWithoutAttribute_PrimitiveParametersInQuery()
        {
            var httpRequest = BuildRequest(BuildMethod<IMultiplyPrimitiveParametersWithoutAttribute>(), 1, "val");

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get,
                parameters: new[] { new HttpParameter("id", 1), new HttpParameter("value", "val") });
        }

        private interface ICustomTypeParameter {[GetMethod] int Get([QueryParam] BasicEntity entity); }

        [Test]
        public void Build_CustomTypeParameter_PropertiesInQuery()
        {
            var httpRequest = BuildRequest(BuildMethod<ICustomTypeParameter>(), new BasicEntity { Id = 1, Value = 2 });

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get,
                parameters: new[] { new HttpParameter("entity.Id", 1), new HttpParameter("entity.Value", 2) });
        }

        private interface IMultiplyCustomTypeParameters {[GetMethod] int Get([QueryParam] BasicEntity entity1, [QueryParam] BasicEntity entity2); }

        [Test]
        public void Build_MultiplyCustomTypeParameters_PropertiesInQuery()
        {
            var httpRequest = BuildRequest(
                BuildMethod<IMultiplyCustomTypeParameters>(), new BasicEntity { Id = 1, Value = 2 }, new BasicEntity { Id = 2, Value = 3 });

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get,
                parameters: new[]
                {
                    new HttpParameter("entity1.Id", 1),
                    new HttpParameter("entity1.Value", 2),
                    new HttpParameter("entity2.Id", 2),
                    new HttpParameter("entity2.Value", 3)
                });
        }

        private interface IArrayOfPrimitivesParameter {[GetMethod] int Get([QueryParam] int[] ids); }

        [Test]
        public void Build_ArrayOfPrimitivesParameter_ParameterWithSameNameInQuery()
        {
            var httpRequest = BuildRequest(
                BuildMethod<IArrayOfPrimitivesParameter>(),
                arguments: new[] { new[] { 1, 2 } });

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get,
                parameters: new[] { new HttpParameter("ids", 1), new HttpParameter("ids", 2) });
        }

        private interface IArrayOfCustomTypeParameter {[GetMethod] int Get([QueryParam] BasicEntity[] entities); }

        [Test]
        public void Build_ArrayOfCustomTypeParameter_ThrowClientValidationException()
        {
            Func<HttpRequest> buildRequestFunc = () => BuildRequest(
                BuildMethod<IArrayOfCustomTypeParameter>(),
                arguments: new[] { new[] { new BasicEntity { Id = 1, Value = 2 }, new BasicEntity { Id = 2, Value = 3 } } });

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<ClientValidationException>()
                .WithMessage(ClientValidationExceptionFactory.ArrayWithComplexTypeNotSupported().Message);
        }

        private interface IDictionaryOfPrimitivesParameter {[GetMethod] int Get([QueryParam] Dictionary<int, string> dict); }

        [Test]
        public void Build_DictionaryOfPrimitivesParameter_KeyValueParametersInQuery()
        {
            var httpRequest = BuildRequest(
                BuildMethod<IDictionaryOfPrimitivesParameter>(),
                arguments: new[] { new Dictionary<int, string> { [1] = "val1", [2] = "val2" } });

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get,
                parameters: new[] { new HttpParameter("dict[1]", "val1"), new HttpParameter("dict[2]", "val2") });
        }

        private interface IDictionaryOfCustomTypesParameter {[GetMethod] int Get([QueryParam] Dictionary<int, BasicEntity> dict); }

        [Test]
        public void Build_DictionaryOfCustomTypesParameter_ThrowClientValidationException()
        {
            Func<HttpRequest> buildRequestFunc = () => BuildRequest(
                BuildMethod<IDictionaryOfCustomTypesParameter>(),
                arguments: new[] { new Dictionary<int, BasicEntity> { [1] = new() { Id = 1, Value = 2 }, [2] = new() { Id = 2, Value = 3 } } });

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<ClientValidationException>()
                .WithMessage(ClientValidationExceptionFactory.DictionaryWithComplexTypeOfValueNotSupported().Message);
        }

        private interface INestedCustomTypesParameter {[GetMethod] int Get([QueryParam] NestedEntity entity); }

        [Test]
        public void Build_NestedCustomTypesParameter_PropertiesInQuery()
        {
            var httpRequest = BuildRequest(
                BuildMethod<INestedCustomTypesParameter>(),
                new NestedEntity { Id = 1, Value = "val", InnerEntity = new BasicEntity { Id = 2, Value = 3 } });

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get,
                parameters: new[]
                {
                    new HttpParameter("entity.Id", 1),
                    new HttpParameter("entity.Value", "val"),
                    new HttpParameter("entity.InnerEntity.Id", 2),
                    new HttpParameter("entity.InnerEntity.Value", 3)
                });
        }

        private interface ICustomTypeWithArrayParameter {[GetMethod] int Get([QueryParam] EntityWithArray entity); }

        [Test]
        public void Build_CustomTypeWithArrayParameter_PropertiesInQuery()
        {
            var httpRequest = BuildRequest(
                BuildMethod<ICustomTypeWithArrayParameter>(),
                new EntityWithArray { Id = 1, Value = "val", Array = new[] { 1, 2 } });

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get,
                parameters: new[]
                {
                    new HttpParameter("entity.Id", 1),
                    new HttpParameter("entity.Value", "val"),
                    new HttpParameter("entity.Array", 1),
                    new HttpParameter("entity.Array", 2)
                });
        }

        private interface ICustomTypeWithArrayOfCustomTypesParameter {[GetMethod] int Get([QueryParam] EntityWithCustomTypeArray entity); }

        [Test]
        public void Build_ComplexCustomTypeWithCustomTypeArrayQueryParam_ThrowClientValidationException()
        {
            Func<HttpRequest> buildRequestFunc = () => BuildRequest(
                BuildMethod<ICustomTypeWithArrayOfCustomTypesParameter>(),
                new EntityWithCustomTypeArray { Id = 1, Value = "val", Array = new[] { new BasicEntity(), new BasicEntity() } });

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<ClientValidationException>()
                .WithMessage(ClientValidationExceptionFactory.ArrayWithComplexTypeNotSupported().Message);
        }

        private interface ICustomTypeWithDictionaryParameter {[GetMethod] int Get([QueryParam] EntityWithDict entity); }

        [Test]
        public void Build_CustomTypeWithDictionaryParameter_PropertiesInQuery()
        {
            var httpRequest = BuildRequest(
                BuildMethod<ICustomTypeWithDictionaryParameter>(),
                new EntityWithDict { Id = 1, Value = "val", Dict = new Dictionary<int, string> { [1] = "val1", [2] = "val2" } });

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get,
                parameters: new[]
                {
                    new HttpParameter("entity.Id", 1),
                    new HttpParameter("entity.Value", "val"),
                    new HttpParameter("entity.Dict[1]", "val1"),
                    new HttpParameter("entity.Dict[2]", "val2")
                });
        }

        private interface ICustomTypeWithDictionaryOfCustomTypesParameter {[GetMethod] int Get([QueryParam] EntityWithCustomTypeDict entity); }

        [Test]
        public void Build_CustomTypeWithDictionaryOfCustomTypesParameter_ThrowClientValidationException()
        {
            Func<HttpRequest> buildRequestFunc = () => BuildRequest(
                BuildMethod<ICustomTypeWithDictionaryOfCustomTypesParameter>(),
                new EntityWithCustomTypeDict { Id = 1, Value = "val", Dict = new Dictionary<int, BasicEntity> { [1] = new(), [2] = new() } });

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<ClientValidationException>()
                .WithMessage(ClientValidationExceptionFactory.DictionaryWithComplexTypeOfValueNotSupported().Message);
        }
    }
}
