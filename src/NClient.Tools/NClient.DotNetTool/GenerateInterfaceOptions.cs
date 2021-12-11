using CommandLine;

namespace NClient.DotNetTool
{
    // ReSharper disable once ClassNeverInstantiated.Global
    [Verb("interface", HelpText = "Generate interface facade for openAPI")]
    public class GenerateInterfaceOptions
    {
        public GenerateInterfaceOptions(string spec, string outputPath, string @namespace, string facadeName)
        {
            Spec = spec;
            OutputPath = outputPath;
            Namespace = @namespace;
            FacadeName = facadeName;
        }

        [Option(shortName: 's', longName: "spec", Required = true, HelpText = "The OpenAPI spec file to use. Paths are relative to the project directory. You can also set a http URI as path")]
        public string Spec { get; }

        [Option(shortName: 'o', longName: "output-file", Required = false, HelpText = "The result. Paths are relative to the project directory.", Default = "./Facades.cs")]
        public string OutputPath { get; }

        [Option(shortName: 'n', longName: "namespace", Required = false, HelpText = "The namespace for generated files.", Default = "NClient.Facades")]
        public string Namespace { get; }
        
        [Option(shortName: 'f', longName: "facade", Required = false, HelpText = "The facade name.", Default = "{controller}")]
        public string FacadeName { get; }
    }
}