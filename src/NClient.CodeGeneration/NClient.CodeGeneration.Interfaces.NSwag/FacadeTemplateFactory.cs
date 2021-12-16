using System.IO;
using System.Reflection;
using NJsonSchema.CodeGeneration;
using NSwag;

namespace NClient.CodeGeneration.Interfaces.NSwag
{
    internal class FacadeTemplateFactory : DefaultTemplateFactory
    {
        public FacadeTemplateFactory(CodeGeneratorSettingsBase settings, Assembly[] assemblies)
            : base(settings, assemblies)
        {
        }
        
        protected override string GetToolchainVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version + " (NSwag v" + OpenApiDocument.ToolchainVersion + " (NJsonSchema v" + base.GetToolchainVersion() + "))";
        }
        
        protected override string GetEmbeddedLiquidTemplate(string language, string template)
        {
            template = template.TrimEnd('!') switch
            {
                "Controller.Class.Annotations" => "Facade.Annotations",
                "Controller.Method.Annotations" => "Facade.Method.Annotations",
                _ => template
            };
            
            var assembly = GetLiquidAssembly($"{nameof(NClient)}.{nameof(CodeGeneration)}.{nameof(Interfaces)}.{nameof(NSwag)}");
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
