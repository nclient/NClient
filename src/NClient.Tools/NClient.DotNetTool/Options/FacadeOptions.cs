using CommandLine;
using Microsoft.Extensions.Logging;

namespace NClient.DotNetTool.Options
{
    // ReSharper disable once ClassNeverInstantiated.Global
    [Verb("facade", HelpText = "Interface facade for OpenAPI")]
    public class FacadeOptions
    {
        [Verb("generate", HelpText = "Generate interface facade for OpenAPI")]
        public class GenerationOptions : CommonOptions
        {
            [Option(shortName: 'a', longName: "api", Required = true, HelpText = "The OpenAPI spec file to use. Paths are relative to the project directory. You can also set a http URI as path")]
            public string Spec { get; }

            [Option(shortName: 'o', longName: "output", Required = false, HelpText = "The result. File paths are relative to the project directory.", Default = "./Facades.cs")]
            public string OutputPath { get; }

            [Option(shortName: 's', longName: "namespace", Required = false, HelpText = "The namespace for generated files.", Default = "NClient.Facades")]
            public string Namespace { get; }
        
            [Option(shortName: 'n', longName: "name", Required = false, HelpText = "The facade name.", Default = "{controller}")]
            public string FacadeName { get; }
            
            public GenerationOptions(
                string spec, string outputPath, string @namespace, string facadeName,
                LogLevel logLevel) : base(logLevel)
            {
                Spec = spec;
                OutputPath = outputPath;
                Namespace = @namespace;
                FacadeName = facadeName;
            }
        }
    }
}