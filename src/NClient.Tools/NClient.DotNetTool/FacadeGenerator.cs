using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NClient.CodeGeneration.Abstractions;

namespace NClient.DotNetTool
{
    // ReSharper disable once UnusedType.Global
    public class FacadeGenerator
    {
        private readonly ILogger<FacadeGenerator> _logger;
        private readonly INClientInterfaceGenerator _interfaceGenerator;
        
        public FacadeGenerator(INClientInterfaceGenerator interfaceGenerator, ILogger<FacadeGenerator> logger)
        {
            _logger = logger;
            _interfaceGenerator = interfaceGenerator;
        }

        public async Task<string> GenerateAsync(CommandLineOptions opts, string specification)
        {
            return await _interfaceGenerator.GenerateAsync(specification, opts.Namespace, opts.FacadeName);
        }
    }
}
