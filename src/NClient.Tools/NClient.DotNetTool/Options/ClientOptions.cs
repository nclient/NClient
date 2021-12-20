using CommandLine;
using Microsoft.Extensions.Logging;

namespace NClient.DotNetTool.Options
{
    // ReSharper disable once ClassNeverInstantiated.Global
    [Verb("client", HelpText = "Client interface for OpenAPI")]
    public class ClientOptions
    {
        [Verb("generate", HelpText = "Generate client interface for OpenAPI")]
        public class GenerationOptions : CommonGenerationOptions
        {
            [Option(shortName: 'n', longName: "name", Required = false, HelpText = "The client name.", Default = "{client}")]
            public string ClientName { get; }
            
            [Option(longName: "withFacades", Required = false, HelpText = "The flag indicating whether to add facade interfaces.", Default = false)]
            public bool GenerateFacades { get; }

            public GenerationOptions(
                string clientName, bool generateFacades,
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
                ClientName = clientName;
                GenerateFacades = generateFacades;
            }
        }
    }
}