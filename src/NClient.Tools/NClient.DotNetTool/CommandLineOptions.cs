using CommandLine;

namespace NClient.DotNetTool
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CommandLineOptions
    {
        public CommandLineOptions(string spec, string outputDirectoryPath, string projectPath, string @namespace)
        {
            Spec = spec;
            OutputDirectoryPath = outputDirectoryPath;
            ProjectPath = projectPath;
            Namespace = @namespace;
        }

        [Option(shortName: 's', longName: "spec", Required = true, HelpText = "The OpenAPI spec file to use. Paths are relative to the project directory.")]
        public string Spec { get; }

        [Option(shortName: 'o', longName: "output-file", Required = false, HelpText = "The result. Paths are relative to the project directory.", Default = "Facades.cs")]
        public string OutputDirectoryPath { get; }

        [Option(shortName: 'p', longName: "project", Required = true, HelpText = "The project to use. Defaults to the current working directory.")]
        public string ProjectPath { get; }

        [Option(shortName: 'n', longName: "namespace", Required = false, HelpText = "The namespace for generated files.", Default = "NClient.Facades")]
        public string @Namespace { get; }
    }
}