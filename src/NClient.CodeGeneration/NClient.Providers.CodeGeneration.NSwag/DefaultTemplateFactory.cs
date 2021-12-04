﻿using System.IO;
using System.Reflection;
using NJsonSchema.CodeGeneration;
using NSwag;

namespace NClient.Providers.CodeGeneration.NSwag
{
    /// <summary>The default template factory which loads templates from embedded resources.</summary>
    internal class DefaultTemplateFactory : NJsonSchema.CodeGeneration.DefaultTemplateFactory
    {
        /// <summary>Initializes a new instance of the <see cref="DefaultTemplateFactory" /> class.</summary>
        /// <param name="settings">The settings.</param>
        /// <param name="assemblies">The assemblies.</param>
        public DefaultTemplateFactory(CodeGeneratorSettingsBase settings, Assembly[] assemblies)
            : base(settings, assemblies)
        {
        }

        /// <summary>Gets the current toolchain version.</summary>
        /// <returns>The toolchain version.</returns>
        protected override string GetToolchainVersion()
        {
            return OpenApiDocument.ToolchainVersion + " (NJsonSchema v" + base.GetToolchainVersion() + ")";
        }

        /// <summary>Tries to load an embedded Liquid template.</summary>
        /// <param name="language">The language.</param>
        /// <param name="template">The template name.</param>
        /// <returns>The template.</returns>
        protected override string GetEmbeddedLiquidTemplate(string language, string template)
        {
            template = template.TrimEnd('!');
            var assembly = GetLiquidAssembly($"{nameof(NClient)}.{nameof(Providers)}.{nameof(CodeGeneration)}.{nameof(NSwag)}");
            var resourceName = $"{nameof(NClient)}.{nameof(Providers)}.{nameof(CodeGeneration)}.{nameof(NSwag)}.Templates." + template + ".liquid";

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