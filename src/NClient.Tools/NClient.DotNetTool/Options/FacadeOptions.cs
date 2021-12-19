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
        
            [Option(shortName: 'n', longName: "name", Required = false, HelpText = "The facade name.", Default = "{facade}")]
            public string FacadeName { get; }
            
            [Option(longName: "modelValidation", Required = false, HelpText = "The flag indicating whether to add model validation attributes.", Default = false)]
            public bool UseModelValidationAttributes { get; }
            
            [Option(longName: "dto", Required = false, HelpText = "The flag indicating whether to generate DTO classes.", Default = true)]
            public bool GenerateDtoTypes { get; }

            [Option(longName: "cancellationTokens", Required = false, HelpText = "The flag indicating whether to allow adding cancellation token.", Default = false)]
            public bool UseCancellationToken { get; }
            
            [Option(longName: "nullableEnable", Required = false, HelpText = "The flag indicating whether to generate Nullable Reference Type annotations.", Default = false)]
            public bool UseNullableReferenceTypes { get; }
            
            [Option(longName: "systemTextJson", SetName = "serializer", Required = false, HelpText = "The flag indicating the use of the SystemTextJson library for serialization.", Default = false)]
            public bool UseSystemTextJson { get; }
            
            [Option(longName: "newtonsoftJson", SetName = "serializer", Required = false, HelpText = "The flag indicating the use of the NewtonsoftJson library for serialization.", Default = false)]
            public bool UseNewtonsoftJson { get; }
            
            public GenerationOptions(
                string spec, string outputPath, string @namespace, string facadeName,
                bool useModelValidationAttributes, bool useNullableReferenceTypes,
                bool generateDtoTypes, bool useCancellationToken,
                bool useSystemTextJson, bool useNewtonsoftJson,
                LogLevel logLevel) : base(logLevel)
            {
                Spec = spec;
                OutputPath = outputPath;
                Namespace = @namespace;
                FacadeName = facadeName;
                UseModelValidationAttributes = useModelValidationAttributes;
                UseNullableReferenceTypes = useNullableReferenceTypes;
                GenerateDtoTypes = generateDtoTypes;
                UseCancellationToken = useCancellationToken;
                UseSystemTextJson = useSystemTextJson;
                UseNewtonsoftJson = useNewtonsoftJson;
            }
        }
    }
}