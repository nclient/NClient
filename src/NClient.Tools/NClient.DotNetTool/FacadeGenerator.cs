using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NClient.CodeGeneration.Abstractions;
using NClient.DotNetTool.Options;

namespace NClient.DotNetTool
{
    public interface IFacadeGenerator
    {
        Task<string> GenerateAsync(InterfaceOptions.GenerateOptions generateOptions, string specification);
    }
    
    public class FacadeGenerator : IFacadeGenerator
    {
        private readonly ILogger<FacadeGenerator> _logger;
        private readonly INClientInterfaceGenerator _interfaceGenerator;
        
        public FacadeGenerator(INClientInterfaceGenerator interfaceGenerator, ILogger<FacadeGenerator> logger)
        {
            _logger = logger;
            _interfaceGenerator = interfaceGenerator;
        }

        public async Task<string> GenerateAsync(InterfaceOptions.GenerateOptions generateOptions, string specification)
        {
            return await _interfaceGenerator.GenerateAsync(specification, generateOptions.Namespace, generateOptions.FacadeName);
        }
    }
}
