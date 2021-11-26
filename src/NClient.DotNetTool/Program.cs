using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NClient.DotNetTool.Loaders;

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

            var generatedFacades =
                serviceProvider.GetRequiredService<IGenerator>().Generate(specification, opts.Namespace);

            if (generatedFacades.Count == 0)
                return;

            await WriteFacades(opts, generatedFacades);
        }

        private static async Task WriteFacades(CommandLineOptions opts, Dictionary<string, string> generatedFacades)
        {
            var outputDirectoryPath =
                Path.Combine(
                    new FileInfo(opts.ProjectPath).Directory?.FullName ?? throw new InvalidOperationException(),
                    opts.OutputDirectoryPath);

            if (Directory.Exists(outputDirectoryPath))
                Directory.Delete(outputDirectoryPath, recursive: true);

            Directory.CreateDirectory(outputDirectoryPath);

            foreach (var (fileName, fileContent) in generatedFacades)
            {
                await File.WriteAllTextAsync(Path.Combine(outputDirectoryPath, $"{fileName}.cs"), fileContent);
            }
        }

        private static IServiceProvider BuildServiceProvider()
        {
            return new ServiceCollection()
                .AddLogging(x => x.AddConsole().SetMinimumLevel(LogLevel.Trace))
                .AddSingleton<ILoaderFactory>(_ => new LoaderFactory())
                .AddSingleton<IGenerator>(_ => new Generator())
                .BuildServiceProvider();
        }
    }    
}

