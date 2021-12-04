using System;
using System.IO;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.DotNetTool.Loaders;
using NClient.Generation.CodeGenerator;

namespace NClient.DotNetTool
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class Program
    {
        public static async Task<int> Main(string[] args)
        {
            return await Parser.Default.ParseArguments<CommandLineOptions>(args)
                .MapResult(async opts =>
                    {
                        try
                        {
                            await Execution(opts);
                            return 0;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            return -1;
                        }
                    },
                    _ => Task.FromResult(-1));
        }

        private static async Task Execution(CommandLineOptions opts)
        {
            var serviceProvider = BuildServiceProvider();

            var specification = await serviceProvider.GetRequiredService<ILoaderFactory>().Create(opts).Load();

            var specificationHandler = serviceProvider.GetRequiredService<ISpecificationHandler>();
            
            var code = await specificationHandler.Generate(specification, opts.Namespace);

            if (string.IsNullOrEmpty(code))
                return;

            await WriteFacades(opts, code);
        }

        private static async Task WriteFacades(CommandLineOptions opts, string generatedFacades)
        {
            var outputFilePath =
                Path.Combine(
                    new FileInfo(opts.ProjectPath).Directory?.FullName ?? throw new InvalidOperationException(),
                    opts.OutputDirectoryPath);

            if (File.Exists(outputFilePath))
                File.Delete(outputFilePath);

            await File.WriteAllTextAsync(outputFilePath, generatedFacades);
        }

        private static IServiceProvider BuildServiceProvider()
        {
            return new ServiceCollection()
                .AddLogging(x => x.AddConsole().SetMinimumLevel(LogLevel.Trace))
                .AddSingleton<ILoaderFactory>(_ => new LoaderFactory())
                .AddSingleton<ISpecificationHandler>(_ => new SpecificationHandler())
                .BuildServiceProvider();
        }
    }    
}

