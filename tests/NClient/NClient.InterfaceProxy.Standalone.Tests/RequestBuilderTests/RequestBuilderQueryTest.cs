using System;
using System.Collections.Generic;
using System.Net.Http;
using FluentAssertions;
using NClient.Annotations.Methods;
using NClient.Annotations.Parameters;
using NClient.Core.Exceptions;
using NClient.Core.Interceptors;
using NClient.Core.Mappers;
using NClient.Providers.HttpClient;
using NClient.Testing.Common;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.InterfaceProxy.Standalone.Tests.RequestBuilderTests
{
    [Parallelizable]
    public class RequestBuilderQueryTest : RequestBuilderTestBase
    {
        [OneTimeSetUp]
        public override void OneTimeSetUp()
        {
            AttributeMapper = new AttributeMapper();
            KeepDataInterceptor = new KeepDataInterceptor();
        }

        public interface IPrimitiveParameter { [GetMethod] int Get([QueryParam] int id); }

        [Test]
        public void Build_PrimitiveParameter_PrimitiveParameterInQuery()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IPrimitiveParameter>(KeepDataInterceptor)
                .Get(1);

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get,
                parameters: new[] { new HttpParameter("id", 1) });
        }

        public interface IMultiplyPrimitiveParameters { [GetMethod] int Get([QueryParam] int id, [QueryParam] string value); }

        [Test]
        public void Build_MultiplyPrimitiveParameters_PrimitiveParametersInQuery()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IMultiplyPrimitiveParameters>(KeepDataInterceptor)
                .Get(1, "val");

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get,
                parameters: new[] { new HttpParameter("id", 1), new HttpParameter("value", "val") });
        }


        public interface IPrimitiveParameterWithoutAttribute { [GetMethod] int Get(int id); }

        [Test]
        public void Build_PrimitiveParameterWithoutAttribute_PrimitiveParameterInQuery()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IPrimitiveParameterWithoutAttribute>(KeepDataInterceptor)
                .Get(1);

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get,
                parameters: new [] { new HttpParameter("id", 1) });
        }

        public interface IMultiplyPrimitiveParametersWithoutAttribute { [GetMethod] int Get(int id, string value); }

        [Test]
        public void Build_MultiplyPrimitiveParametersWithoutAttribute_PrimitiveParametersInQuery()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IMultiplyPrimitiveParametersWithoutAttribute>(KeepDataInterceptor)
                .Get(1, "val");

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get,
                parameters: new[] { new HttpParameter("id", 1), new HttpParameter("value", "val") });
        }

        public interface ICustomTypeParameter { [GetMethod] int Get([QueryParam] BasicEntity entity); }

        [Test]
        public void Build_CustomTypeParameter_PropertiesInQuery()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<ICustomTypeParameter>(KeepDataInterceptor)
                .Get(new BasicEntity { Id = 1, Value = 2 });

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get,
                parameters: new[] { new HttpParameter("entity.Id", 1), new HttpParameter("entity.Value", 2) });
        }

        public interface IMultiplyCustomTypeParameters { [GetMethod] int Get([QueryParam] BasicEntity entity1, [QueryParam] BasicEntity entity2); }

        [Test]
        public void Build_MultiplyCustomTypeParameters_PropertiesInQuery()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IMultiplyCustomTypeParameters>(KeepDataInterceptor)
                .Get(new BasicEntity { Id = 1, Value = 2 }, new BasicEntity { Id = 2, Value = 3 });

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

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

        public interface IArrayOfPrimitivesParameter { [GetMethod] int Get([QueryParam] int[] ids); }

        [Test]
        public void Build_ArrayOfPrimitivesParameter_ParameterWithSameNameInQuery()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IArrayOfPrimitivesParameter>(KeepDataInterceptor)
                .Get(new [] { 1, 2 });

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get,
                parameters: new[] { new HttpParameter("ids", 1), new HttpParameter("ids", 2) });
        }

        public interface IArrayOfCustomTypeParameter { [GetMethod] int Get([QueryParam] BasicEntity[] entities); }

        [Test]
        public void Build_ArrayOfCustomTypeParameter_ThrowNotSupportedNClientException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IArrayOfCustomTypeParameter>(KeepDataInterceptor)
                .Get(new[] { new BasicEntity { Id = 1, Value = 2 }, new BasicEntity { Id = 2, Value = 3 } });

            Func<HttpRequest> buildRequestFunc = () => BuildRequest(KeepDataInterceptor.Invocation!);

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<NotSupportedNClientException>();
        }

        public interface IDictionaryOfPrimitivesParameter { [GetMethod] int Get([QueryParam] Dictionary<int, string> dict); }

        [Test]
        public void Build_DictionaryOfPrimitivesParameter_KeyValueParametersInQuery()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IDictionaryOfPrimitivesParameter>(KeepDataInterceptor)
                .Get(new Dictionary<int, string> { [1] = "val1", [2] = "val2" });

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

            AssertHttpRequest(httpRequest,
                new Uri("http://localhost:5000/"),
                HttpMethod.Get,
                parameters: new[] { new HttpParameter("dict[1]", "val1"), new HttpParameter("dict[2]", "val2") });
        }

        public interface IDictionaryOfPCustomTypesParameter { [GetMethod] int Get([QueryParam] Dictionary<int, BasicEntity> dict); }

        [Test]
        public void Build_DictionaryOfPCustomTypesParameter_ThrowNotSupportedNClientException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<IDictionaryOfPCustomTypesParameter>(KeepDataInterceptor)
                .Get(new Dictionary<int, BasicEntity> { [1] = new() { Id = 1, Value = 2 }, [2] = new() { Id = 2, Value = 3 } });

            Func<HttpRequest> buildRequestFunc = () => BuildRequest(KeepDataInterceptor.Invocation!);

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<NotSupportedNClientException>();
        }

        public interface INestedCustomTypesParameter { [GetMethod] int Get([QueryParam] NestedEntity entity); }

        [Test]
        public void Build_NestedCustomTypesParameter_PropertiesInQuery()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<INestedCustomTypesParameter>(KeepDataInterceptor)
                .Get(new NestedEntity { Id = 1, Value = "val", InnerEntity = new BasicEntity { Id = 2, Value = 3 }});

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

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

        public interface ICustomTypeWithArrayParameter { [GetMethod] int Get([QueryParam] EntityWithArray entity); }

        [Test]
        public void Build_CustomTypeWithArrayParameter_PropertiesInQuery()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<ICustomTypeWithArrayParameter>(KeepDataInterceptor)
                .Get(new EntityWithArray { Id = 1, Value = "val", Array = new[] { 1, 2 } });

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

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

        public interface ICustomTypeWithArrayOfCustomTypesParameter { [GetMethod] int Get([QueryParam] EntityWithCustomTypeArray entity); }

        [Test]
        public void Build_ComplexCustomTypeWithCustomTypeArrayQueryParam_ThrowNotSupportedNClientException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<ICustomTypeWithArrayOfCustomTypesParameter>(KeepDataInterceptor)
                .Get(new EntityWithCustomTypeArray { Id = 1, Value = "val", Array = new[] { new BasicEntity(), new BasicEntity() } });

            Func<HttpRequest> buildRequestFunc = () => BuildRequest(KeepDataInterceptor.Invocation!);

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<NotSupportedNClientException>();
        }

        public interface ICustomTypeWithDictionaryParameter { [GetMethod] int Get([QueryParam] EntityWithDict entity); }

        [Test]
        public void Build_CustomTypeWithDictionaryParameter_PropertiesInQuery()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<ICustomTypeWithDictionaryParameter>(KeepDataInterceptor)
                .Get(new EntityWithDict { Id = 1, Value = "val", Dict = new Dictionary<int, string> { [1] = "val1", [2] = "val2" } });

            var httpRequest = BuildRequest(KeepDataInterceptor.Invocation!);

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

        public interface ICustomTypeWithDictionaryOfCustomTypesParameter { [GetMethod] int Get([QueryParam] EntityWithCustomTypeDict entity); }

        [Test]
        public void Build_CustomTypeWithDictionaryOfCustomTypesParameter_ThrowNotSupportedNClientException()
        {
            ProxyGenerator
                .CreateInterfaceProxyWithoutTarget<ICustomTypeWithDictionaryOfCustomTypesParameter>(KeepDataInterceptor)
                .Get(new EntityWithCustomTypeDict { Id = 1, Value = "val", Dict = new Dictionary<int, BasicEntity> { [1] = new(), [2] = new() } });

            Func<HttpRequest> buildRequestFunc = () => BuildRequest(KeepDataInterceptor.Invocation!);

            buildRequestFunc
                .Invoking(x => x.Invoke())
                .Should()
                .Throw<NotSupportedNClientException>();
        }
    }
}
