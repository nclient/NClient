using System;
using FluentAssertions;
using NClient.Annotations;
using NClient.Annotations.Http;
using NClient.AspNetCore.Controllers;
using NUnit.Framework;

// ReSharper disable UnusedType.Global
// ReSharper disable BadDeclarationBracesLineBreaks
// ReSharper disable BadEmptyBracesLineBreaks
namespace NClient.AspNetCore.Tests
{
    [Facade] public interface IEmptyFacade { }
    public interface IInheritorEmptyFacade : IEmptyFacade { }
    [HttpFacade] public interface IEmptyHttpFacade { }
    public interface IInheritorEmptyHttpFacade : IEmptyHttpFacade { }
    public interface IFacadeWithMethod { [PostMethod] void Post(); }
    public interface IInheritorFacadeWithMethod : IFacadeWithMethod { }
    public interface IFacadeWithOperation { [UpdateOperation] void Post(); }
    public interface IInheritorFacadeWithOperation : IFacadeWithOperation { }
    public interface IFacadeWithQuery { int Get([QueryParam] int id); }
    public interface IInheritorFacadeWithQuery : IFacadeWithQuery { }
    public interface IFacadeWithProperty { int Get([PropertyParam] int id); }
    public interface IInheritorFacadeWithProperty : IFacadeWithProperty { }
    
    [Parallelizable]
    public class ControllerQualifierTest
    {
        [TestCase(typeof(IEmptyFacade))]
        [TestCase(typeof(IInheritorEmptyFacade))]
        [TestCase(typeof(IEmptyHttpFacade))]
        [TestCase(typeof(IInheritorEmptyHttpFacade))]
        [TestCase(typeof(IFacadeWithMethod))]
        [TestCase(typeof(IInheritorFacadeWithMethod))]
        [TestCase(typeof(IFacadeWithOperation))]
        [TestCase(typeof(IInheritorFacadeWithOperation))]
        [TestCase(typeof(IFacadeWithQuery))]
        [TestCase(typeof(IInheritorFacadeWithQuery))]
        [TestCase(typeof(IFacadeWithProperty))]
        [TestCase(typeof(IInheritorFacadeWithProperty))]
        public void IsNClientControllerInterface_NClientControllerInterface_True(Type type)
        {
            ControllerQualifier.IsNClientControllerInterface(type).Should().BeTrue();
        }
    }
}
