//-----------------------------------------------------------------------
// <copyright file="DefaultTemplateFactory.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/RicoSuter/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NJsonSchema.CodeGeneration;
using NSwag;

namespace NClient.CodeGeneration.Generator
{
    /// <summary>The default template factory which loads templates from embedded resources.</summary>
    public class CustomTemplateFactory : DefaultTemplateFactory
    {
        /// <summary>Initializes a new instance of the <see cref="CustomTemplateFactory" /> class.</summary>
        /// <param name="settings">The settings.</param>
        /// <param name="assemblies">The assemblies.</param>
        public CustomTemplateFactory(CodeGeneratorSettingsBase settings, Assembly[] assemblies)
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
            var trimmedTemplate = template.TrimEnd('!');
            var assembly = typeof(SpecificationHandler).GetTypeInfo().Assembly;
            var resourceName = $"NClient.CodeGeneration.Generator.Templates.{trimmedTemplate}.liquid";

            var resourceNames = typeof(SpecificationHandler).GetTypeInfo().Assembly.GetManifestResourceNames();

            if (!resourceNames.Contains(resourceName))
            {
                Console.WriteLine(template);
                return base.GetEmbeddedLiquidTemplate(language, template);
            }

            var resource = assembly.GetManifestResourceStream(resourceName) ?? throw new NullReferenceException($"Assembly {assembly.FullName} doesn't contains resource {trimmedTemplate}");
            using var reader = new StreamReader(resource);
            return reader.ReadToEnd();
        }
    }
}
