using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NClient.CodeGeneration.Abstractions;

namespace NClient.DotNetTool
{
    // ReSharper disable once UnusedType.Global
    public class GeneratorFacade
    {
        private readonly ILogger<GeneratorFacade> _logger;
        private readonly INClientGenerator _clientGenerator;
        
        public GeneratorFacade(INClientGenerator clientGenerator, ILogger<GeneratorFacade> logger)
        {
            _logger = logger;
            _clientGenerator = clientGenerator;
        }

        public async Task<string?> GenerateAsync(CommandLineOptions opts, string specification)
        {
            return await _clientGenerator.GenerateAsync(specification, opts.Namespace);
        }
    }
}
