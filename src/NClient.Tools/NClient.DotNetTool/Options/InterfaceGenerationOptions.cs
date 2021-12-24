using CommandLine;
using Microsoft.Extensions.Logging;

namespace NClient.DotNetTool.Options
{
    public abstract class InterfaceGenerationOptions : CommonOptions
    {
        [Option(shortName: 'a', longName: "api", Required = true, HelpText = "The OpenAPI spec file to use. Paths are relative to the project directory. You can also set a http URI as path")]
        public virtual string Spec { get; }

        [Option(shortName: 'o', longName: "output", Required = false, HelpText = "The result. File paths are relative to the project directory.", Default = "./Facades.cs")]
        public virtual string OutputPath { get; }

        [Option(shortName: 's', longName: "namespace", Required = false, HelpText = "The namespace for generated files.", Default = "NClient.Facades")]
        public virtual string Namespace { get; }

        [Option(longName: "withModelValidation", Required = false, HelpText = "The flag indicating whether to add model validation attributes.", Default = false)]
        public virtual bool UseModelValidationAttributes { get; }
            
        [Option(longName: "withDto", Required = false, HelpText = "The flag indicating whether to generate DTO classes.", Default = true)]
        public virtual bool GenerateDtoTypes { get; }

        [Option(longName: "withCancellationTokens", Required = false, HelpText = "The flag indicating whether to allow adding cancellation token.", Default = false)]
        public virtual bool UseCancellationToken { get; }

        [Option(longName: "useSystemTextJson", SetName = "serializer", Required = false, HelpText = "The flag indicating the use of the SystemTextJson library for serialization.", Default = false)]
        public virtual bool UseSystemTextJson { get; }
            
        [Option(longName: "useNewtonsoftJson", SetName = "serializer", Required = false, HelpText = "The flag indicating the use of the NewtonsoftJson library for serialization.", Default = false)]
        public virtual bool UseNewtonsoftJson { get; }
            
        [Option(longName: "nullableEnable", Required = false, HelpText = "The flag indicating whether to generate Nullable Reference Type annotations.", Default = false)]
        public virtual bool UseNullableReferenceTypes { get; }
        
        protected InterfaceGenerationOptions(
            string spec, string outputPath, string @namespace,
            bool useModelValidationAttributes, bool useNullableReferenceTypes,
            bool generateDtoTypes, bool useCancellationToken,
            bool useSystemTextJson, bool useNewtonsoftJson,
            LogLevel logLevel) : base(logLevel)
        {
            Spec = spec;
            OutputPath = outputPath;
            Namespace = @namespace;
            UseModelValidationAttributes = useModelValidationAttributes;
            UseNullableReferenceTypes = useNullableReferenceTypes;
            GenerateDtoTypes = generateDtoTypes;
            UseCancellationToken = useCancellationToken;
            UseSystemTextJson = useSystemTextJson;
            UseNewtonsoftJson = useNewtonsoftJson;
        }
    }
}
