using CommandLine;
using Microsoft.Extensions.Logging;

namespace NClient.DotNetTool.Options
{
    // ReSharper disable once ClassNeverInstantiated.Global
    [Verb("facade", HelpText = "Interface facade for OpenAPI")]
    public class FacadeOptions
    {
        [Verb("generate", HelpText = "Generate interface facade for OpenAPI")]
        public class GenerationOptions : CommonGenerationOptions
        {
            [Option(shortName: 'n', longName: "name", Required = false, HelpText = "The facade name.", Default = "{facade}")]
            public string FacadeName { get; }
            
            [Option(longName: "withClients", Required = false, HelpText = "The flag indicating whether to add client interfaces.", Default = false)]
            public bool GenerateClients { get; }

            public GenerationOptions(
                string facadeName, bool generateClients,
                string spec, string outputPath, string @namespace,
                bool useModelValidationAttributes, bool useNullableReferenceTypes,
                bool generateDtoTypes, bool useCancellationToken,
                bool useSystemTextJson, bool useNewtonsoftJson,
                LogLevel logLevel) : base(spec, outputPath, @namespace,
                useModelValidationAttributes, useNullableReferenceTypes,
                generateDtoTypes, useCancellationToken, 
                useSystemTextJson, useNewtonsoftJson,
                logLevel)
            {
                FacadeName = facadeName;
                GenerateClients = generateClients;
            }
        }
    }
}