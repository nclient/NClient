using System.IO;
using System.Reflection;
using NJsonSchema.CodeGeneration;
using NSwag;

namespace NClient.CodeGeneration.Providers.NSwag
{
    internal class DefaultTemplateFactory : NJsonSchema.CodeGeneration.DefaultTemplateFactory
    {
        public DefaultTemplateFactory(CodeGeneratorSettingsBase settings, Assembly[] assemblies)
            : base(settings, assemblies)
        {
        }
        
        protected override string GetToolchainVersion()
        {
            return OpenApiDocument.ToolchainVersion + " (NJsonSchema v" + base.GetToolchainVersion() + ")";
        }
        
        protected override string GetEmbeddedLiquidTemplate(string language, string template)
        {
            template = template.TrimEnd('!') switch
            {
                "Controller" => "Interface",
                "Controller.Class.Annotations" => "Interface.Annotations",
                "Controller.Method.Annotations" => "Interface.Method.Annotations",
                _ => template
            };
            
            var assembly = GetLiquidAssembly($"{nameof(NClient)}.{nameof(CodeGeneration)}.{nameof(Providers)}.{nameof(NSwag)}");
            var resourceName = $"{assembly.GetName().Name}.Templates." + template + ".liquid";

            var resource = assembly.GetManifestResourceStream(resourceName);
            if (resource != null)
            {
                using var reader = new StreamReader(resource);
                return reader.ReadToEnd();
            }

            return base.GetEmbeddedLiquidTemplate(language, template);
        }
    }
}
