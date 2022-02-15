using CommandLine;
using Microsoft.Extensions.Logging;

namespace NClient.DotNetTool.Options
{
    // ReSharper disable once ClassNeverInstantiated.Global
    [Verb("generate", HelpText = "Source code generation")]
    public class GenerationOptions
    {
        [Verb("client", HelpText = "Generate client interface for OpenAPI")]
        public class ClientOptions : InterfaceGenerationOptions
        {
            [Option(shortName: 'n', longName: "name", Required = false, HelpText = "The client name.", Default = "{client}")]
            public string ClientName { get; }

            [Option(longName: "withFacades", Required = false, HelpText = "The flag indicating whether to add facade interfaces.", Default = false)]
            public bool GenerateFacades { get; }

            public ClientOptions(
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
        
        [Verb("facade", HelpText = "Generate facade interface for OpenAPI")]
        public class FacadeOptions : InterfaceGenerationOptions
        {
            [Option(shortName: 'n', longName: "name", Required = false, HelpText = "The facade name.", Default = "{facade}")]
            public string FacadeName { get; }
            
            [Option(longName: "withClients", Required = false, HelpText = "The flag indicating whether to add client interfaces.", Default = false)]
            public bool GenerateClients { get; }

            public FacadeOptions(
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