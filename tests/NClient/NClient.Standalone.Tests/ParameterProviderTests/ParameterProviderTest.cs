using Castle.DynamicProxy;
using FluentAssertions;
using NClient.Annotations.Parameters;
using NClient.Core.AspNetRouting;
using NClient.Core.Exceptions;
using NClient.Core.Interceptors;
using NClient.Core.Mappers;
using NClient.Core.RequestBuilders;
using NClient.Core.RequestBuilders.Models;
using NClient.Testing.Common.Entities;
using NUnit.Framework;

namespace NClient.Standalone.Tests.ParameterProviderTests
{
    [Parallelizable]
    public class ParameterProviderTest
    {
        internal ParameterProvider ParameterProvider = null!;
        internal KeepDataInterceptor KeepDataInterceptor = null!;
        private ProxyGenerator _proxyGenerator = null!;
        private RouteTemplate _emptyRouteTemplate = null!;

        [SetUp]
        public void SetUp()
        {
            var attributeMapper = new AttributeMapper();
            ParameterProvider = new ParameterProvider(attributeMapper);

            _proxyGenerator = new ProxyGenerator();
            _emptyRouteTemplate = TemplateParser.Parse("");
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            KeepDataInterceptor = new KeepDataInterceptor();
        }

        public interface IPrimitiveRouteParamWithoutAttribute { void Method(int id); }

        [Test]
        public void Get_PrimitiveRouteParamWithoutAttribute_RouteParam()
        {
            var routeTemplate = TemplateParser.Parse("{id}");
            _proxyGenerator
                .CreateInterfaceProxyWithoutTarget<IPrimitiveRouteParamWithoutAttribute>(KeepDataInterceptor)
                .Method(1);

            var parameters = ParameterProvider.Get(
                routeTemplate,
                KeepDataInterceptor.Invocation!.Method,
                KeepDataInterceptor.Invocation.Arguments);

            parameters.Should().BeEquivalentTo(new Parameter("id", typeof(int), 1, new RouteParamAttribute()));
        }

        public interface IPrimitiveRouteParam { void Method([RouteParam] int id); }

        [Test]
        public void Get_PrimitiveRouteParam_RouteParam()
        {
            _proxyGenerator
                .CreateInterfaceProxyWithoutTarget<IPrimitiveRouteParam>(KeepDataInterceptor)
                .Method(1);

            var parameters = ParameterProvider.Get(
                _emptyRouteTemplate, 
                KeepDataInterceptor.Invocation!.Method, 
                KeepDataInterceptor.Invocation.Arguments);

            parameters.Should().BeEquivalentTo(new Parameter("id", typeof(int), 1, new RouteParamAttribute()));
        }

        public interface ICustomTypeRouteParamWithoutAttribute { void Method(BasicEntity entity); }

        [Test]
        public void Get_CustomTypeRouteParamWithoutAttribute_RouteParam()
        {
            var routeTemplate = TemplateParser.Parse("{entity}");
            var entity = new BasicEntity { Id = 1, Value = 2 };
            _proxyGenerator
                .CreateInterfaceProxyWithoutTarget<ICustomTypeRouteParamWithoutAttribute>(KeepDataInterceptor)
                .Method(entity);

            var parameters = ParameterProvider.Get(
                routeTemplate,
                KeepDataInterceptor.Invocation!.Method,
                KeepDataInterceptor.Invocation.Arguments);

            parameters.Should().BeEquivalentTo(new Parameter("entity", typeof(BasicEntity), entity, new RouteParamAttribute()));
        }

        public interface ICustomTypeRouteParam { void Method([RouteParam] BasicEntity entity); }

        [Test]
        public void Get_CustomTypeRouteParam_RouteParam()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            _proxyGenerator
                .CreateInterfaceProxyWithoutTarget<ICustomTypeRouteParam>(KeepDataInterceptor)
                .Method(entity);

            var parameters = ParameterProvider.Get(
                _emptyRouteTemplate,
                KeepDataInterceptor.Invocation!.Method,
                KeepDataInterceptor.Invocation.Arguments);

            parameters.Should().BeEquivalentTo(new Parameter("entity", typeof(BasicEntity), entity, new RouteParamAttribute()));
        }

        public interface IPrimitiveWithoutAttribute { void Method(int id); }

        [Test]
        public void Get_PrimitiveWithoutAttribute_QueryParam()
        {
            _proxyGenerator
                .CreateInterfaceProxyWithoutTarget<IPrimitiveWithoutAttribute>(KeepDataInterceptor)
                .Method(1);

            var parameters = ParameterProvider.Get(
                _emptyRouteTemplate,
                KeepDataInterceptor.Invocation!.Method,
                KeepDataInterceptor.Invocation.Arguments);

            parameters.Should().BeEquivalentTo(new Parameter("id", typeof(int), 1, new QueryParamAttribute()));
        }

        public interface IPrimitiveQueryParam { void Method([QueryParam] int id); }

        [Test]
        public void Get_PrimitiveQueryParam_QueryParam()
        {
            _proxyGenerator
                .CreateInterfaceProxyWithoutTarget<IPrimitiveQueryParam>(KeepDataInterceptor)
                .Method(1);

            var parameters = ParameterProvider.Get(
                _emptyRouteTemplate,
                KeepDataInterceptor.Invocation!.Method,
                KeepDataInterceptor.Invocation.Arguments);

            parameters.Should().BeEquivalentTo(new Parameter("id", typeof(int), 1, new QueryParamAttribute()));
        }

        public interface ICustomTypeQueryParam { void Method([QueryParam] BasicEntity entity); }

        [Test]
        public void Get_CustomTypeQueryParam_QueryParam()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            _proxyGenerator
                .CreateInterfaceProxyWithoutTarget<ICustomTypeQueryParam>(KeepDataInterceptor)
                .Method(entity);

            var parameters = ParameterProvider.Get(
                _emptyRouteTemplate,
                KeepDataInterceptor.Invocation!.Method,
                KeepDataInterceptor.Invocation.Arguments);

            parameters.Should().BeEquivalentTo(new Parameter("entity", typeof(BasicEntity), entity, new QueryParamAttribute()));
        }

        public interface ICustomTypeWithoutAttribute { void Method(BasicEntity entity); }

        [Test]
        public void Get_CustomTypeWithoutAttribute_BodyParam()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            _proxyGenerator
                .CreateInterfaceProxyWithoutTarget<ICustomTypeWithoutAttribute>(KeepDataInterceptor)
                .Method(entity);

            var parameters = ParameterProvider.Get(
                _emptyRouteTemplate,
                KeepDataInterceptor.Invocation!.Method,
                KeepDataInterceptor.Invocation.Arguments);

            parameters.Should().BeEquivalentTo(new Parameter("entity", typeof(BasicEntity), entity, new BodyParamAttribute()));
        }

        public interface ICustomTypeBodyParam { void Method([BodyParam] BasicEntity entity); }

        [Test]
        public void Get_CustomTypeBodyParam_BodyParam()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            _proxyGenerator
                .CreateInterfaceProxyWithoutTarget<ICustomTypeBodyParam>(KeepDataInterceptor)
                .Method(entity);

            var parameters = ParameterProvider.Get(
                _emptyRouteTemplate,
                KeepDataInterceptor.Invocation!.Method,
                KeepDataInterceptor.Invocation.Arguments);

            parameters.Should().BeEquivalentTo(new Parameter("entity", typeof(BasicEntity), entity, new BodyParamAttribute()));
        }

        public interface IPrimitiveBodyParam { void Method([BodyParam] int id); }

        [Test]
        public void Get_PrimitiveBodyParam_BodyParam()
        {
            _proxyGenerator
                .CreateInterfaceProxyWithoutTarget<IPrimitiveBodyParam>(KeepDataInterceptor)
                .Method(1);

            var parameters = ParameterProvider.Get(
                _emptyRouteTemplate,
                KeepDataInterceptor.Invocation!.Method,
                KeepDataInterceptor.Invocation.Arguments);

            parameters.Should().BeEquivalentTo(new Parameter("id", typeof(int), 1, new BodyParamAttribute()));
        }

        public interface IPrimitiveHeaderParam { void Method([HeaderParam] int id); }

        [Test]
        public void Get_PrimitiveHeaderParam_HeaderParam()
        {
            _proxyGenerator
                .CreateInterfaceProxyWithoutTarget<IPrimitiveHeaderParam>(KeepDataInterceptor)
                .Method(1);

            var parameters = ParameterProvider.Get(
                _emptyRouteTemplate,
                KeepDataInterceptor.Invocation!.Method,
                KeepDataInterceptor.Invocation.Arguments);

            parameters.Should().BeEquivalentTo(new Parameter("id", typeof(int), 1, new HeaderParamAttribute()));
        }

        public interface ICustomTypeHeaderParam { void Method([HeaderParam] BasicEntity entity); }

        [Test]
        public void Get_CustomTypeHeaderParam_HeaderParam()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            _proxyGenerator
                .CreateInterfaceProxyWithoutTarget<ICustomTypeHeaderParam>(KeepDataInterceptor)
                .Method(entity);

            var parameters = ParameterProvider.Get(
                _emptyRouteTemplate,
                KeepDataInterceptor.Invocation!.Method,
                KeepDataInterceptor.Invocation.Arguments);

            parameters.Should().BeEquivalentTo(new Parameter("entity", typeof(BasicEntity), entity, new HeaderParamAttribute()));
        }

        public interface IMultipleAttributeParam { void Method([QueryParam, BodyParam] int id); }

        [Test]
        public void Get_MultipleAttributeParam_ThrowNotSupportedNClientException()
        {
            _proxyGenerator
                .CreateInterfaceProxyWithoutTarget<IMultipleAttributeParam>(KeepDataInterceptor)
                .Method(1);

            ParameterProvider
                .Invoking(x => x.Get(
                    _emptyRouteTemplate,
                    KeepDataInterceptor.Invocation!.Method,
                    KeepDataInterceptor.Invocation.Arguments))
                .Should()
                .Throw<NotSupportedNClientException>();
        }
    }
}
