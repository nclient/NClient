using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.OpenApi.Readers;
using NClient.Annotations.Http;
using NClient.CodeGeneration.Abstractions;
using NClient.CodeGeneration.Abstractions.Enums;
using NClient.CodeGeneration.Facades.NSwag;
using NUnit.Framework;
using CategoryAttribute = System.ComponentModel.CategoryAttribute;

namespace NClient.CodeGeneration.Interfaces.NSwag.Tests
{
    public class MultipartComparator : IEqualityComparer<string>
    {
        public bool Equals(string? x, string? y)
        {
            if (x is null || y is null)
                return false;
            return x.Contains(y);
        }
        public int GetHashCode(string obj)
        {
            return base.GetHashCode();
        }
    }
    
    [Parallelizable]
    public class NSwagGeneratorTest
    {
        [Test]
        public void GenerateAsync_OpenApiJsonSpec_CodeOfInterfaceAndDtos()
        {
            const string openApiSpecJson = @"swagger.json";

            var openApiSpec = LoadSpec(openApiSpecJson);
            var ops = GetOpenApiDoc(openApiSpec).Paths.SelectMany(p => p.Value.Operations).Select(o => o.Value).ToArray();
            var notAvailableIds = ops
                .Where(o => o.RequestBody is not null && o.RequestBody.Content.Keys.Contains("multipart", new MultipartComparator()))
                .Select(o => o.OperationId)
                .ToArray();
            var availableOps = ops.Where(o => !notAvailableIds.Contains(o.OperationId)).ToArray();

            var source = GenerateSourceCode(openApiSpec);

            var types = Compile(source).ToArray();

            var tags = availableOps.Select(o => o.Tags.First()).Distinct().ToArray();

            foreach (var tag in tags)
            {
                var facadeInterface = types.Where(t => t.IsInterface && t.GetCustomAttributes().FirstOrDefault(a => a.GetType() == typeof(CategoryAttribute)) is not null).ToList();
                var baseInterfaces = facadeInterface
                    .Where(x => x.GetCustomAttribute<HttpFacadeAttribute>() is not null)
                    .Where(fi => string.Equals(typeof(CategoryAttribute)
                        .GetProperty(nameof(CategoryAttribute.Category))!
                        .GetValue(fi.GetCustomAttribute(typeof(CategoryAttribute)))!
                        .ToString()!, tag.Name, StringComparison.InvariantCultureIgnoreCase)).ToArray();

                baseInterfaces.Should().ContainSingle();

                var baseInterface = baseInterfaces.Single();

                var inheritInterfaces = types.Where(t => t.IsInterface && t.GetInterface(baseInterface.FullName!) is not null);

                inheritInterfaces.Should().ContainSingle();
            }
        }

        private IEnumerable<Type> Compile(string source)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(source);

            var assemblyName = Path.GetRandomFileName();
            var ns = Assembly.Load("netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51");
            #if !NETFRAMEWORK
            var rt = Assembly.Load("System.Runtime, Version=4.2.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
            #endif
            
            MetadataReference[] references =
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Runtime.Serialization.EnumMemberAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Text.Json.JsonSerializer).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Newtonsoft.Json.JsonConvert).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.ComponentModel.DataAnnotations.Validator).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.AspNetCore.Mvc.ModelBinding.BindRequiredAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Annotations.FacadeAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Providers.Results.HttpResults.HttpResponse).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(GeneratedCodeAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(CategoryAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(ns.Location),
                #if !NETFRAMEWORK
                MetadataReference.CreateFromFile(rt.Location)
                #endif
            };

            var compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using var ms = new MemoryStream();
            // write IL code into memory
            var result = compilation.Emit(ms);

            if (!result.Success)
            {
                // handle exceptions
                var failures = result.Diagnostics.Where(diagnostic =>
                    diagnostic.IsWarningAsError ||
                    diagnostic.Severity == DiagnosticSeverity.Error).ToList();

                failures.Should().HaveCount(0, "Compilation should be successful");
            }
            ms.Seek(0, SeekOrigin.Begin);
            var assembly = Assembly.Load(ms.ToArray());

            return assembly.GetTypes();
        }

        private string LoadSpec(string name)
        {
            var resource = typeof(NSwagGeneratorTest).GetTypeInfo().Assembly
                    .GetManifestResourceStream($"{nameof(NClient)}.{nameof(CodeGeneration)}.{nameof(Interfaces)}.{nameof(NSwag)}.{nameof(Tests)}.Specifications.{name}") 
                ?? throw new NullReferenceException("No open api spec.");
            resource.Should().NotBeNull();
            using var reader = new StreamReader(resource);
            reader.Should().NotBeNull();
            return reader.ReadToEnd();
        }

        private string GenerateSourceCode(string openApiSpec)
        {
            var specificationHandler = new NSwagFacadeGenerator(null);
            const string @namespace = "Test";

            var generationSettings = new FacadeGenerationSettings(
                name: "{facade}",
                @namespace,
                useModelValidationAttributes: true,
                useNullableReferenceTypes: false,
                useCancellationToken: true,
                useDtoTypes: true,
                serializeType: SerializeType.SystemJsonText);
            var result = specificationHandler.GenerateAsync(openApiSpec, generationSettings).GetAwaiter().GetResult();

            result.Should().NotBeNullOrEmpty();

            return result;
        }

        private Microsoft.OpenApi.Models.OpenApiDocument GetOpenApiDoc(string openApiSpec)
        {
            var openApiDocument = new OpenApiStringReader().Read(openApiSpec, out var diagnostic);

            diagnostic.Errors.Should().HaveCount(0);

            return openApiDocument;
        }
    }
}
