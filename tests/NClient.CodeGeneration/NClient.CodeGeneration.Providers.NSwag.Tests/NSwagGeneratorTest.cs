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
using NUnit.Framework;

namespace NClient.CodeGeneration.Providers.NSwag.Tests
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
            var ops = GetOpenApiDoc(openApiSpec).Paths.SelectMany(p => p.Value.Operations).Select(o => o.Value).ToList();
            var notAvailableIds = ops
                .Where(o => o.RequestBody is not null && o.RequestBody.Content.Keys.Contains("multipart", new MultipartComparator()))
                .Select(o => o.OperationId)
                .ToList();
            var availableOps = ops.Where(o => !notAvailableIds.Contains(o.OperationId)).ToList();
            var opsCount = availableOps.Count;

            var source = GenerateSourceCode(openApiSpec);

            var type = Compile(source);
            var methods = type.GetMethods();

            methods.Should().HaveCount(opsCount);
        }

        private Type Compile(string source)
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
                MetadataReference.CreateFromFile(typeof(Newtonsoft.Json.JsonConvert).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.ComponentModel.DataAnnotations.Validator).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Annotations.FacadeAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(GeneratedCodeAttribute).Assembly.Location),
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

            var type = assembly.GetTypes().Single(x => x.IsInterface);
            return type;
        }

        private string LoadSpec(string name)
        {
            var resource = typeof(NSwagGeneratorTest).GetTypeInfo().Assembly
                    .GetManifestResourceStream($"{nameof(NClient)}.{nameof(CodeGeneration)}.{nameof(Providers)}.{nameof(NSwag)}.{nameof(Tests)}.Specifications.{name}") 
                ?? throw new NullReferenceException("no open api spec");
            resource.Should().NotBeNull();
            using var reader = new StreamReader(resource);
            reader.Should().NotBeNull();
            return reader.ReadToEnd();
        }

        private string GenerateSourceCode(string openApiSpec)
        {
            var specificationHandler = new NSwagGenerator(null);
            const string @namespace = "Test";

            var result = specificationHandler.GenerateAsync(openApiSpec, @namespace).GetAwaiter().GetResult();

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
