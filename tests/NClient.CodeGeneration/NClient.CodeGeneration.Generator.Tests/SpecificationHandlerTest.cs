using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace NClient.CodeGeneration.Generator.Tests
{
    [Parallelizable]
    public class SpecificationHandlerTest
    {
        [Test]
        public async Task GenerateAsync_ValidSpecification_InterfaceCode()
        {
            var specificationHandler = new SpecificationHandler();
            var specification = File.ReadAllText("Specs/UberApi.json");

            var interfaceCode = await specificationHandler.GenerateAsync(specification, "Test");

            interfaceCode.Should().NotBeNullOrEmpty();
        }
    }
}
